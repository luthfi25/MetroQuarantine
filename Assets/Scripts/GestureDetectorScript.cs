using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using System;
using System.IO;

using PDollarGestureRecognizer;
using TMPro;

public class GestureDetectorScript : MonoBehaviour
{
    public Transform gestureOnScreenPrefab;

	private List<Gesture> trainingSet = new List<Gesture>();

	private List<Point> points = new List<Point>();
	private int strokeId = -1;

	private Vector3 virtualKeyPosition = Vector2.zero;
	private Rect drawArea;
	private Color color;

	private RuntimePlatform platform;
	private int vertexCount = 0;

	private List<LineRenderer> gestureLinesRenderer = new List<LineRenderer>();
	private LineRenderer currentGestureLineRenderer;

	//GUI
	private string message;
	private bool recognized;

    public List<String> requiredClasses = new List<String>();
	String activeClass;
	private String[] originRequiredClasses;

	public Animator GestureAnimation;
	public GameObject referencePicGO;
	public List<Sprite> referencePics;
	
	//for Book
	public String mode;
	public MiniGameController miniGameControllerInstance;

	public GameObject RecognizeButton;
	public bool isDrawing;
	public Animator HandAnimation;
	private bool isAnimating;
	public GestureHolderScript gestureHolderScripInstance;
	public TextMeshProUGUI ScoreText;
	private GameObject[] line;

	public List<AudioClip> clips;
	bool isSwiping;
	bool isClosing;

    // Start is called before the first frame update
    void Start()
    {
        platform = Application.platform;
		miniGameControllerInstance = GameObject.Find("Camera Mini Games").GetComponent<MiniGameController>();    
		originRequiredClasses = new String[6];

		if (mode == "Soap") {
			drawArea = new Rect(15 + Screen.width / 3, Screen.height / 4, Screen.width / 3, Screen.height / 2);
			color = new Color(1,1,1,0.25f);
			gestureHolderScripInstance.MoveDown();
		} else if (mode == "Book"){
			drawArea = new Rect(Screen.width / 8, Screen.height / 8, 3 * Screen.width / 8, 3 * Screen.height / 4);
			color = new Color(1,1,1,0.25f);
			
			//Randomly remove 2 elements from requiredClass
			requiredClasses.RemoveAt(UnityEngine.Random.Range(0, requiredClasses.Count - 1));
			requiredClasses.RemoveAt(UnityEngine.Random.Range(0, requiredClasses.Count - 1));
			ActivateGesture();
		}

		TextAsset[] testGesturesXml = Resources.LoadAll<TextAsset>("Test Gestures/");
		int index = 0;
		while(index < requiredClasses.Count){
			foreach (TextAsset testGestureXml in testGesturesXml) {
				if(testGestureXml.name.Contains(requiredClasses[index])){
					trainingSet.Add(GestureIO.ReadGestureFromXML(testGestureXml.text));
				}
			}
			index++;
		}

		requiredClasses.CopyTo(originRequiredClasses);
		line = new GameObject[]{};
    }

	void OnEnable(){
		if(mode == "Book"){
			referencePicGO.SetActive(true);
			RecognizeButton.SetActive(true);
		}

		isDrawing = true;
		ScoreText.text = "";
		isSwiping = false;
		isAnimating = false;
		isClosing = false;
	}

	void OnDisable(){
		referencePicGO.SetActive(false);
		RecognizeButton.SetActive(false);
		ScoreText.text = "";
	}

    // Update is called once per frame
    [Obsolete]
    void Update()
    {
		if(requiredClasses.Count == 0 && !isClosing){
			if(!isAnimating){
				isClosing = true;
				miniGameControllerInstance.PlaySound(clips[2], false);
				
				if(mode == "Soap"){
					miniGameControllerInstance.CloseMiniGameDelay(this.gameObject, "Sabun", 1f);
				} else if(mode == "Book"){
					miniGameControllerInstance.CloseMiniGameDelay(this.gameObject, "Buku", 1f);
				}
			}
		}

        if (platform == RuntimePlatform.Android || platform == RuntimePlatform.IPhonePlayer) {
			if (Input.touchCount > 0) {
				virtualKeyPosition = new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y);
			}
		} else {
			if (Input.GetMouseButton(0)) {
				virtualKeyPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y);
			}
		}

		if (drawArea.Contains(virtualKeyPosition)) {

			if (Input.GetMouseButtonDown(0)) {

				if (recognized) {

					recognized = false;
					strokeId = -1;

					points.Clear();

					foreach (LineRenderer lineRenderer in gestureLinesRenderer) {

						lineRenderer.SetVertexCount(0);
						Destroy(lineRenderer.gameObject);
					}

					gestureLinesRenderer.Clear();

					if(mode == "Book"){
						drawTest(activeClass);
					}
				}

				++strokeId;
				
				Transform tmpGesture = Instantiate(gestureOnScreenPrefab, transform.position, transform.rotation) as Transform;
				currentGestureLineRenderer = tmpGesture.GetComponent<LineRenderer>();
				
				gestureLinesRenderer.Add(currentGestureLineRenderer);
				
				vertexCount = 0;
			}
			
			if (Input.GetMouseButton(0)) {
				if(!isSwiping){
					isSwiping = true;
					if(mode == "Soap"){
						miniGameControllerInstance.PlaySound(clips[0], false);
					} else if(mode == "Book"){
						miniGameControllerInstance.PlaySound(clips[0], true);
					}
				}

				points.Add(new Point(virtualKeyPosition.x, -virtualKeyPosition.y, strokeId));

				currentGestureLineRenderer.SetVertexCount(++vertexCount);
				currentGestureLineRenderer.SetPosition(vertexCount - 1, Camera.main.ScreenToWorldPoint(new Vector3(virtualKeyPosition.x, virtualKeyPosition.y, 10)));
			}
		}

		if(Input.GetMouseButtonUp(0)){

			if(isSwiping){
				isSwiping = false;
				miniGameControllerInstance.StopSound();
			}

			if(mode == "Soap"){
				recognized = true;

				Gesture candidate = new Gesture(points.ToArray());
				Result gestureResult = PointCloudRecognizer.Classify(candidate, trainingSet.ToArray());

				if(gestureResult.GestureClass == activeClass && gestureResult.Score >= 0.75f){
				// if(gestureResult.GestureClass == activeClass && gestureResult.Score >= 0.25f){

					requiredClasses.Remove(activeClass);
					miniGameControllerInstance.AddProgressTrack(6 - requiredClasses.Count, 6, true);

					recognized = false;
					strokeId = -1;

					points.Clear();

					foreach (LineRenderer lineRenderer in gestureLinesRenderer) {

						lineRenderer.SetVertexCount(0);
						Destroy(lineRenderer.gameObject);
					}

					gestureLinesRenderer.Clear();
					AnimateHand(originRequiredClasses.Length - requiredClasses.Count - 1);

					if(requiredClasses.Count != 0) {
						activeClass = requiredClasses[UnityEngine.Random.Range(0, requiredClasses.Count - 1)];
					}
				} else {
					miniGameControllerInstance.CooldownByMistake();

					recognized = false;
					strokeId = -1;

					points.Clear();

					foreach (LineRenderer lineRenderer in gestureLinesRenderer) {

						lineRenderer.SetVertexCount(0);
						Destroy(lineRenderer.gameObject);
					}

					gestureLinesRenderer.Clear();
					Handheld.Vibrate();
				}
			}
		}
    }

    void OnGUI() {
		if(isDrawing){
			GUI.color = color;
			GUI.Box(drawArea, "");
			//TO DELETE
			// GUI.Label(new Rect(10, Screen.height - 40, 500, 50), message);
		}
	}

	public void CheckDraw(){
		recognized = true;

		Gesture candidate = new Gesture(points.ToArray());
		Dictionary<string, float> gestureResult = PointCloudRecognizer.ClassifyArr(candidate, trainingSet.ToArray());

		recognized = false;
		strokeId = -1;

		points.Clear();

		foreach (LineRenderer lineRenderer in gestureLinesRenderer) {

			lineRenderer.SetVertexCount(0);
			Destroy(lineRenderer.gameObject);
		}

		gestureLinesRenderer.Clear();

		if(gestureResult.ContainsKey(activeClass) && gestureResult[activeClass] <= 1f){
			// Debug.Log(activeClass+" "+gestureResult[activeClass]);
			if(gestureResult[activeClass] >= 0.87f) {
				miniGameControllerInstance.PlaySound(clips[1], false);

				ScoreText.text = "Berhasil :)";
				ScoreText.color = new Color(0.027044f, 0.8113208f, 0f, 1f);

				requiredClasses.Remove(activeClass);
				miniGameControllerInstance.AddProgressTrack(6 - requiredClasses.Count, 6, false);

				if(requiredClasses.Count != 0) {
					activeClass = requiredClasses[UnityEngine.Random.Range(0, requiredClasses.Count - 1)];
					drawTest(activeClass);
				}
			} else {
				Handheld.Vibrate();
				ScoreText.text = "Sedikit Lagi!\n Skor kamu " + (int) (gestureResult[activeClass] * 100) + "%";
				ScoreText.color = new Color(0f, 0f, 0f, 1f);
			}
		} else {
			Handheld.Vibrate();
			ScoreText.text = "Coba lagi :(";
			ScoreText.color = new Color(1f, 0f, 0f, 1f);
		}

		ScoreText.gameObject.GetComponent<Animator>().SetTrigger("Expand");
	}

	void drawTest(string test){
		switch(test){
			case "fireplace":
				referencePicGO.GetComponent<Image>().sprite = referencePics[0];
				break;
			case "butterfly":
				referencePicGO.GetComponent<Image>().sprite = referencePics[1];
				break;
			case "flamingo":
				referencePicGO.GetComponent<Image>().sprite = referencePics[2];
				break;
			case "lightbulb":
				referencePicGO.GetComponent<Image>().sprite = referencePics[3];
				break;
			case "mask":
				referencePicGO.GetComponent<Image>().sprite = referencePics[4];
				break;
			case "leaf":
				referencePicGO.GetComponent<Image>().sprite = referencePics[5];
				break;
			case "paperclip":
				referencePicGO.GetComponent<Image>().sprite = referencePics[6];
				break;
			case "rabbit":
				referencePicGO.GetComponent<Image>().sprite = referencePics[7];
				break;
			default:
				referencePicGO.GetComponent<Image>().sprite = referencePics[0];
				break;
		}
	}

	public void RestartGesture(){
		requiredClasses = new List<string>(originRequiredClasses);
		points.Clear();

		foreach (LineRenderer lineRenderer in gestureLinesRenderer) {

			lineRenderer.SetVertexCount(0);
			Destroy(lineRenderer.gameObject);
		}

		gestureLinesRenderer.Clear();
		activeClass = requiredClasses[UnityEngine.Random.Range(0, requiredClasses.Count - 1)];
		
		if(mode == "Soap"){
			GestureAnimation.SetTrigger(activeClass);
			HandAnimation.SetTrigger("toIdle");
		} else if(mode == "Book"){
			drawTest(activeClass);
			ScoreText.text = "";
		}
	}

	public void DisableGUI(){
		if(mode == "Book"){
			line = GameObject.FindGameObjectsWithTag("Gesture");
			foreach(GameObject l in line){
				l.SetActive(false);
			}
		}

		isDrawing = false;
	}

	public void EnableGUI(){
		if(!isAnimating){
			isDrawing = true;

			if(mode == "Book"){
				if(line.Length > 0){
					foreach(GameObject l in line){
						l.SetActive(true);
					}
					Array.Clear(line, 0, line.Length);
				}
			}
		}
	}

	public void ActivateGesture(){
		activeClass = requiredClasses[UnityEngine.Random.Range(0, requiredClasses.Count - 1)];

		if(mode == "Soap"){
			GestureAnimation.SetTrigger(activeClass);
		} else if (mode == "Book"){
			drawTest(activeClass);

		}
	}

	public void AnimateHand(int i){
		string[] triggers = new String[] {"1st Step","2nd Step","3rd Step","4th Step","5th Step","6th Step"};
		GestureAnimation.gameObject.SetActive(false);
		HandAnimation.SetTrigger(triggers[i]);
		isAnimating = true;
		DisableGUI();
		gestureHolderScripInstance.MoveUp();
		miniGameControllerInstance.PlaySound(clips[1], true);
		StartCoroutine(disableAnimate());
	}

	IEnumerator disableAnimate(){
		yield return new WaitForSeconds(2.0125f);
		isAnimating = false;
		EnableGUI();
		miniGameControllerInstance.StopSound();
		if(requiredClasses.Count > 0){
			gestureHolderScripInstance.MoveDown();
		}
	}

	public void Undo(){
		Debug.Log(strokeId);

		if(strokeId < 0){
			return;
		}

		Debug.Log("masup");
		recognized = false;
		strokeId--;

		points.RemoveAt(points.Count-1);
		
		int lastIndexGesture = gestureLinesRenderer.Count - 1;
		gestureLinesRenderer[lastIndexGesture].SetVertexCount(0);
		Destroy(gestureLinesRenderer[lastIndexGesture].gameObject);
		gestureLinesRenderer.RemoveAt(lastIndexGesture);

		if(mode == "Book"){
			drawTest(activeClass);
		}
	}
}

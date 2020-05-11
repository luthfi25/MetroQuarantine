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
	private bool isDrawing;

	public GameObject[] Tutorial;
	public Animator HandAnimation;
	private bool isAnimating;
	public GestureHolderScript gestureHolderScripInstance;
	public TextMeshProUGUI ScoreText;

    // Start is called before the first frame update
    void Start()
    {
        platform = Application.platform;

		//Load pre-made gestures
		// TextAsset[] gesturesXml = Resources.LoadAll<TextAsset>("GestureSet/10-stylus-MEDIUM/");
		// foreach (TextAsset gestureXml in gesturesXml)
		// 	trainingSet.Add(GestureIO.ReadGestureFromXML(gestureXml.text));

		if (mode == "Soap") {
			drawArea = new Rect(15 + Screen.width / 3, Screen.height / 4, Screen.width / 3, Screen.height / 2);
			color = new Color(1,1,1,0.25f);
			originRequiredClasses = new String[6];
		} else if (mode == "Book"){
			drawArea = new Rect(Screen.width / 8, Screen.height / 8, 3 * Screen.width / 8, 3 * Screen.height / 4);
			color = new Color(1,1,1,0.25f);
			originRequiredClasses = new String[6];
			
			//Randomly remove 2 elements from requiredClass
			requiredClasses.RemoveAt(UnityEngine.Random.Range(0, requiredClasses.Count - 1));
			requiredClasses.RemoveAt(UnityEngine.Random.Range(0, requiredClasses.Count - 1));
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
    }

	void OnEnable(){
		if(mode == "Book"){
			referencePicGO.SetActive(true);
			RecognizeButton.SetActive(true);
			Tutorial[0].SetActive(true);
		} else if(mode == "Soap") {
			Tutorial[1].SetActive(true);
		}

		isDrawing = true;
		ScoreText.text = "";
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
		if(requiredClasses.Count == 0){
			if(!isAnimating){
				if(mode == "Soap"){
					miniGameControllerInstance.CloseMiniGame(this.gameObject, "Sabun");
				} else if(mode == "Book"){
					miniGameControllerInstance.CloseMiniGame(this.gameObject, "Buku");
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
				points.Add(new Point(virtualKeyPosition.x, -virtualKeyPosition.y, strokeId));

				currentGestureLineRenderer.SetVertexCount(++vertexCount);
				currentGestureLineRenderer.SetPosition(vertexCount - 1, Camera.main.ScreenToWorldPoint(new Vector3(virtualKeyPosition.x, virtualKeyPosition.y, 1000)));
			}
		}

		if(mode == "Soap"){
			if(Input.GetMouseButtonUp(0)){
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
		Result gestureResult = PointCloudRecognizer.Classify(candidate, trainingSet.ToArray());

		if(gestureResult.GestureClass == activeClass){
			if(gestureResult.Score >= 0.85f) {
				ScoreText.text = "Berhasil :)";
				ScoreText.color = new Color(0.027044f, 0.8113208f, 0f, 1f);

				requiredClasses.Remove(activeClass);
				miniGameControllerInstance.AddProgressTrack(6 - requiredClasses.Count, 6, false);
				recognized = false;
				strokeId = -1;

				points.Clear();

				foreach (LineRenderer lineRenderer in gestureLinesRenderer) {

					lineRenderer.SetVertexCount(0);
					Destroy(lineRenderer.gameObject);
				}

				gestureLinesRenderer.Clear();

				if(requiredClasses.Count == 0) {
					miniGameControllerInstance.CloseMiniGame(this.gameObject, "Buku");
				} else {
					activeClass = requiredClasses[UnityEngine.Random.Range(0, requiredClasses.Count - 1)];
					drawTest(activeClass);
				}
			} else {
				ScoreText.text = "Sedikit Lagi!\n Skor kamu " + (int) (gestureResult.Score * 100) + "%";
				ScoreText.color = new Color(0f, 0f, 0f, 1f);
			}
		} else {
			ScoreText.text = "Coba lagi :(";
			ScoreText.color = new Color(1f, 0f, 0f, 1f);
		}

		ScoreText.gameObject.GetComponent<Animator>().SetTrigger("Expand");
		Debug.Log(gestureResult.GestureClass + " " + gestureResult.Score + " " + activeClass);
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

	public void ToggleGUI(){
		isDrawing = !isDrawing;
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
		HandAnimation.SetTrigger(triggers[i]);
		isAnimating = true;
		ToggleGUI();
		gestureHolderScripInstance.MoveUp();
		StartCoroutine(disableAnimate());
	}

	IEnumerator disableAnimate(){
		yield return new WaitForSeconds(2.0125f);
		isAnimating = false;
		ToggleGUI();
		GestureAnimation.gameObject.SetActive(false);
		gestureHolderScripInstance.MoveDown();
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using System;
using System.IO;

using PDollarGestureRecognizer;

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

    public MovementScript movementScriptInstance;
    public List<String> requiredClasses = new List<String>();
	private String[] originRequiredClasses;

	public GameObject GestureAnimation;
	public GameObject referencePicGO;
	public List<Sprite> referencePics;
	
	//for Book
	public String mode;
	public MiniGameController miniGameControllerInstance;

	public GameObject RecognizeButton;

    // Start is called before the first frame update
    void Start()
    {
        platform = Application.platform;

		//Load pre-made gestures
		TextAsset[] gesturesXml = Resources.LoadAll<TextAsset>("GestureSet/10-stylus-MEDIUM/");
		foreach (TextAsset gestureXml in gesturesXml)
			trainingSet.Add(GestureIO.ReadGestureFromXML(gestureXml.text));

		//Load user custom gestures
		string[] filePaths = Directory.GetFiles(Application.persistentDataPath, "*.xml");
		foreach (string filePath in filePaths)
			trainingSet.Add(GestureIO.ReadGestureFromFile(filePath));

		if (mode == "Soap") {
			drawArea = new Rect(15 + Screen.width / 3, Screen.height / 4, Screen.width / 3, Screen.height / 2);
			color = new Color(1,1,1,0.25f);
			originRequiredClasses = new String[6];
		} else if (mode == "Book"){
			drawArea = new Rect(Screen.width / 8, Screen.height / 8, 3 * Screen.width / 8, 3 * Screen.height / 4);
			color = new Color(1,1,1,0.75f);
			originRequiredClasses = new String[8];
		}
			
		requiredClasses.CopyTo(originRequiredClasses);
		GestureAnimation.GetComponent<Animator>().SetTrigger(requiredClasses[0]);
    }

	void OnEnable(){
		if(mode == "Book"){
			referencePicGO.SetActive(true);
			RecognizeButton.SetActive(true);
		}
	}

	void OnDisable(){
		referencePicGO.SetActive(false);
		RecognizeButton.SetActive(false);
	}

    // Update is called once per frame
    [Obsolete]
    void Update()
    {
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
						drawTest(requiredClasses[0]);
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
				
				message = gestureResult.GestureClass + " " + gestureResult.Score;

				if(gestureResult.GestureClass == requiredClasses[0] && gestureResult.Score >= 0.9f){
					requiredClasses.RemoveAt(0);
					miniGameControllerInstance.AddProgressTrack(6 - requiredClasses.Count, 6);

					recognized = false;
					strokeId = -1;

					points.Clear();

					foreach (LineRenderer lineRenderer in gestureLinesRenderer) {

						lineRenderer.SetVertexCount(0);
						Destroy(lineRenderer.gameObject);
					}

					gestureLinesRenderer.Clear();

					if(requiredClasses.Count == 0) {
						miniGameControllerInstance.CloseMiniGame(this.gameObject, "Sabun");
					} else {
						GestureAnimation.GetComponent<Animator>().SetTrigger(requiredClasses[0]);
					}
				}
        	}
		} else if(mode == "Book"){
			if(requiredClasses.Count == 0) {
				miniGameControllerInstance.CloseMiniGame(this.gameObject, "Buku");
			} else {
				drawTest(requiredClasses[0]);
			}
		}   
    }

    void OnGUI() {
		GUI.color = color;
		GUI.Box(drawArea, "");
	}

	public void CheckDraw(){
		recognized = true;

		// Gesture candidate = new Gesture(points.ToArray());
		// Result gestureResult = PointCloudRecognizer.Classify(candidate, trainingSet.ToArray());
		
		// message = gestureResult.GestureClass + " " + gestureResult.Score;

		// if(gestureResult.GestureClass == requiredClasses[0] && gestureResult.Score >= 0.9f){
		requiredClasses.RemoveAt(0);
		miniGameControllerInstance.AddProgressTrack(8 - requiredClasses.Count, 8);
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
			drawTest(requiredClasses[0]);
		}
		// }
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
		GestureAnimation.GetComponent<Animator>().SetTrigger(requiredClasses[0]);
	}
}

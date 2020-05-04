using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.IO;

using PDollarGestureRecognizer;

public class GestureDetectorScript : MonoBehaviour
{
    public Transform gestureOnScreenPrefab;

	private List<Gesture> trainingSet = new List<Gesture>();
	private List<Point[]> testSet = new List<Point[]>();
	private bool testDrawn = false;

	private List<Point> points = new List<Point>();
	private int strokeId = -1;

	private Vector3 virtualKeyPosition = Vector2.zero;
	private Rect drawArea;
	private Rect referenceArea;

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
	public List<AnimationClip> GestureAnimationClip;
	
	//for Book
	public String mode;
	public MiniGameController miniGameControllerInstance;

    // Start is called before the first frame update
    void Start()
    {
        platform = Application.platform;
		drawArea = new Rect(15 + Screen.width / 3, Screen.height / 4, Screen.width / 3, Screen.height / 2);

		if(mode == "Book"){
			referenceArea = new Rect((Screen.width / 2) + 10, 0, (Screen.width / 2)-10, Screen.height);
		}

		//Load pre-made gestures
		TextAsset[] gesturesXml = Resources.LoadAll<TextAsset>("GestureSet/10-stylus-MEDIUM/");
		foreach (TextAsset gestureXml in gesturesXml)
			trainingSet.Add(GestureIO.ReadGestureFromXML(gestureXml.text));

		//Load user custom gestures
		string[] filePaths = Directory.GetFiles(Application.persistentDataPath, "*.xml");
		foreach (string filePath in filePaths)
			trainingSet.Add(GestureIO.ReadGestureFromFile(filePath));
		
		//For Book only, load test set
		int index = 0;
		while(index < requiredClasses.Count){
			foreach (string filePath in filePaths) {
				if(filePath.Contains(requiredClasses[index])){
					testSet.Add(GestureIO.ReadPointsFromFile(filePath));
					break;
				}
			}
			index++;
		}

		originRequiredClasses = new String[6];
		requiredClasses.CopyTo(originRequiredClasses);
		GestureAnimation.GetComponent<Animator>().SetTrigger(requiredClasses[0]);
    }

	void OnEnable(){
		GestureAnimation.SetActive(true);
	}

	void OnDisable(){
		GestureAnimation.SetActive(false);
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
						testDrawn = false;
						drawTest(testSet[0]);
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
					recognized = false;
					strokeId = -1;

					points.Clear();

					foreach (LineRenderer lineRenderer in gestureLinesRenderer) {

						lineRenderer.SetVertexCount(0);
						Destroy(lineRenderer.gameObject);
					}

					gestureLinesRenderer.Clear();

					if(requiredClasses.Count == 0) {
						this.gameObject.SetActive(false);
						movementScriptInstance.Unpause();
						miniGameControllerInstance.CloseMiniGame();
					}

					GestureAnimation.GetComponent<Animator>().SetTrigger(requiredClasses[0]);
					miniGameControllerInstance.AddProgressTrack(6 - requiredClasses.Count, 7);
				}
        	}
		} else if(mode == "Book"){
			if(requiredClasses.Count == 0) {
				this.gameObject.SetActive(false);
				movementScriptInstance.Unpause();
				miniGameControllerInstance.CloseMiniGame();
			}

			drawTest(testSet[0]);
		}   
    }

    void OnGUI() {
		GUI.color = new Color(1,1,1,0.25f); // half transparent 
		GUI.Box(drawArea, "");

		if(mode == "Book") {
			GUI.Label(new Rect(10, Screen.height - 40, 100, 50), message);

			if (GUI.Button(new Rect(Screen.width - 100, 10, 100, 30), "Recognize")) {
				recognized = true;

				Gesture candidate = new Gesture(points.ToArray());
				Result gestureResult = PointCloudRecognizer.Classify(candidate, trainingSet.ToArray());
				
				message = gestureResult.GestureClass + " " + gestureResult.Score;

				if(gestureResult.GestureClass == requiredClasses[0] && gestureResult.Score >= 0.9f){
					requiredClasses.RemoveAt(0);
					testSet.RemoveAt(0);
					recognized = false;
					strokeId = -1;

					points.Clear();

					foreach (LineRenderer lineRenderer in gestureLinesRenderer) {

						lineRenderer.SetVertexCount(0);
						Destroy(lineRenderer.gameObject);
					}

					gestureLinesRenderer.Clear();

					if(requiredClasses.Count == 0) {
						this.gameObject.SetActive(false);
						movementScriptInstance.Unpause();
						miniGameControllerInstance.CloseMiniGame();
					}

					testDrawn = false;
					drawTest(testSet[0]);
					GestureAnimation.GetComponent<Animator>().SetTrigger(requiredClasses[0]);
					miniGameControllerInstance.AddProgressTrack(5 - requiredClasses.Count, 7);
				}
			}
		}
	}

	void drawTest(Point[] toDraw){
		if(!testDrawn){
			testDrawn = true;
			++strokeId;
			
			Transform tmpGesture = Instantiate(gestureOnScreenPrefab, transform.position, transform.rotation) as Transform;
			currentGestureLineRenderer = tmpGesture.GetComponent<LineRenderer>();
			
			gestureLinesRenderer.Add(currentGestureLineRenderer);
			vertexCount = 0;

			foreach(Point p in toDraw){
				points.Add(new Point(p.X, p.Y, strokeId));

				currentGestureLineRenderer.SetVertexCount(++vertexCount);
				currentGestureLineRenderer.SetPosition(vertexCount - 1, miniGameControllerInstance.CameraMiniGame.GetComponent<Camera>().ScreenToWorldPoint(new Vector3((2 * Screen.width / 5)+p.X, -p.Y, 1000)));
			}
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

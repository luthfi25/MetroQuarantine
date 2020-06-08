using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PuzzleScript : MonoBehaviour
{
    public List<GameObject> PuzzlePieces;
    public List<GameObject> PuzzlePlaces;
    private int counter;
    private bool isClosing;
    public MiniGameController miniGameControllerInstance;
    private List<Vector2> puzzlePiecesPositions;
    private bool isEstablished;

    [SerializeField] private Image potentialImage;
    [SerializeField] private List<Sprite> potentialImages;
    [SerializeField] private List<Sprite> potentialPuzzlePieces;
    private List<Sprite> chosenPuzzlePieces;
    
    [SerializeField] private AudioClip hitClip;
    [SerializeField] private AudioClip winClip;

    // Start is called before the first frame update
    void Start()
    {
        puzzlePiecesPositions = new List<Vector2>();
        isEstablished = false;
        counter = 0;
        miniGameControllerInstance = GameObject.Find("Camera Mini Games").GetComponent<MiniGameController>();
        isClosing = false;

        generateRandomImage();
    }

    void generateRandomImage(){
        int randImageIndex = Random.Range(0, potentialImages.Count);
        potentialImage.sprite = potentialImages[randImageIndex];
        chosenPuzzlePieces = potentialPuzzlePieces.GetRange(randImageIndex*9, 9);
    }

    // Update is called once per frame
    void Update()
    {
        if(!isEstablished){
            isEstablished = true;
            EstablishPuzzlePieces();
        }

        if(counter >= PuzzlePlaces.Count && !isClosing){
            isClosing = true;
            miniGameControllerInstance.PlaySound(winClip, false);
            miniGameControllerInstance.CloseMiniGameDelay(this.gameObject, "Sapu", 2f);
        }
    }

    void EstablishPuzzlePieces(){
        List<int> pieces = new List<int>(){0, 1, 2, 3, 4, 5, 6, 7, 8};
        int i = 0;
        while(pieces.Count > 0){
            int randIndex = Random.Range(0, pieces.Count);
            PuzzlePieces[i].GetComponentInChildren<TextMeshProUGUI>().text = pieces[randIndex].ToString();
            pieces.RemoveAt(randIndex);

            Image image;
            if(PuzzlePieces[i].TryGetComponent<Image>(out image)){
                image.sprite = chosenPuzzlePieces[randIndex];
                chosenPuzzlePieces.RemoveAt(randIndex);
            }
            i++;
        }

        foreach(GameObject pp in PuzzlePieces){
            puzzlePiecesPositions.Add(pp.transform.position);
        }
    }

    public bool ValidPosition(int id, GameObject curGo){
        GameObject puzzlePlace = PuzzlePlaces[id];
        Vector3 curPos = curGo.transform.position;

        if(Mathf.Abs(curPos.x - puzzlePlace.transform.position.x) <= 0.5f &&
            Mathf.Abs(curPos.y - puzzlePlace.transform.position.y) <= 0.5f){
            counter++;
            miniGameControllerInstance.AddProgressTrack(counter, PuzzlePieces.Count, true);
            miniGameControllerInstance.PlaySound(hitClip, false);
            curGo.transform.position = puzzlePlace.transform.position;
            return true;
        } else  {
            return false;
        }
    }

    public void RestartPuzzle(){
        for(int i = 0; i < PuzzlePieces.Count; i++){
            PuzzlePieces[i].transform.position = puzzlePiecesPositions[i];
        }

        generateRandomImage();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PuzzleScript : MonoBehaviour
{
    public List<GameObject> PuzzlePieces;
    public List<GameObject> PuzzlePlaces;
    private int counter;
    private bool isClosing;
    public MiniGameController miniGameControllerInstance;
    public List<AudioClip> clips; 
    private List<Vector2> puzzlePiecesPositions;
    private bool isEstablished;

    // Start is called before the first frame update
    void Start()
    {
        puzzlePiecesPositions = new List<Vector2>();
        isEstablished = false;
        counter = 0;
        miniGameControllerInstance = GameObject.Find("Camera Mini Games").GetComponent<MiniGameController>();
        isClosing = false;
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
            miniGameControllerInstance.PlaySound(clips[0], false);
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
            i++;
        }

        foreach(GameObject pp in PuzzlePieces){
            puzzlePiecesPositions.Add(pp.transform.position);
        }
    }

    public bool ValidPosition(int id, Vector2 curPos){
        GameObject puzzlePlace = PuzzlePlaces[id];

        if(Mathf.Abs(curPos.x - puzzlePlace.transform.position.x) <= 0.5f &&
            Mathf.Abs(curPos.y - puzzlePlace.transform.position.y) <= 0.5f){
            counter++;
            miniGameControllerInstance.AddProgressTrack(counter, PuzzlePieces.Count, true);
            return true;
        } else  {
            return false;
        }
    }

    public void RestartPuzzle(){
        for(int i = 0; i < PuzzlePieces.Count; i++){
            PuzzlePieces[i].transform.position = puzzlePiecesPositions[i];
        }
    }
}

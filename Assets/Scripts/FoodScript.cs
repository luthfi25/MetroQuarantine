using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FoodScript : MonoBehaviour
{
    public TextMeshProUGUI[] charText;
    public GameObject charParent;
    public GameObject answerParent;
    public GameObject answerPlaceHolder;
    private List<string> foods;
    private string onScreenChar;
    private bool clearButton;
    const string ALPHABET = "abcdefghijklmnopqrstuvwxyz";

    private List<TextMeshProUGUI> activeAnswers;

    // Start is called before the first frame update
    void Start()
    {
        activeAnswers = new List<TextMeshProUGUI>();
        clearButton = true;
    }

    void OnEnable(){
        foods = new List<string> (new string[] {"sate", "bakso", "ayam goreng", "burger", "pizza"}); 
        onScreenChar = "";
        charParent.SetActive(true);
        answerParent.SetActive(true);
    }

    void OnDisable(){
        charParent.SetActive(false);
        answerParent.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {   
        if(clearButton){
            foreach(TextMeshProUGUI c in charText){
                c.transform.parent.gameObject.GetComponent<Button>().interactable = true;
            }
            clearButton = false;
        }

        if(onScreenChar.Length > 1){
            return;
        }

        onScreenChar = shuffleChar();
        for(int i = 0; i < onScreenChar.Length; i++){
            charText[i].text = "" + onScreenChar[i];
        }

        generateAnswerSpace();          
    }

    string shuffleChar(){
        string shuffledChar = foods[0].Replace(" ", "");

        while (shuffledChar.Length < 15){
            shuffledChar = shuffledChar + ALPHABET[Random.Range(0, ALPHABET.Length-1)];
        }


        string newString = "";
        while (newString.Length < 15){
            int randomIndex = Random.Range(0, shuffledChar.Length-1);
            newString = newString + shuffledChar[randomIndex];
            shuffledChar = shuffledChar.Remove(randomIndex, 1);
        }

        return newString;
    }

    void generateAnswerSpace(){
        int len = foods[0].Length;
        for(int i = 0; i < len; i++){
            if(foods[0][i].ToString() == " "){
                continue;
            }

            GameObject answerPh = Instantiate(answerPlaceHolder, answerParent.transform.position, transform.rotation);
            answerPh.transform.Translate(new Vector3((i * 56), 0f, 0f)); 
            answerPh.transform.SetParent(answerParent.transform, false);
            activeAnswers.Add(answerPh.GetComponentInChildren<TextMeshProUGUI>());
        }
    }

    public void FillAnswer(GameObject charGo){
        string answer = "";
        string realAnswer = foods[0].Replace(" ", "");

        foreach(TextMeshProUGUI a in activeAnswers){
            answer += a.text;

            if(a.text == ""){
                a.text = charGo.GetComponent<TextMeshProUGUI>().text;
                answer += a.text;
                break;
            }
        }

        if (answer.Length == realAnswer.Length){
            if(answer == realAnswer){
                foods.RemoveAt(0);
                onScreenChar = "";

                foreach(TextMeshProUGUI a in activeAnswers){
                    Destroy(a.gameObject.transform.parent.gameObject);
                }

                activeAnswers.RemoveRange(0, activeAnswers.Count);
            } else {
                foreach(TextMeshProUGUI a in activeAnswers){
                    a.text = "";
                }
            }

            clearButton = true;
        }
    }
}

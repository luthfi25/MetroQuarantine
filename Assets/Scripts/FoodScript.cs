using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FoodScript : MonoBehaviour
{
    public TextMeshProUGUI[] charText;
    public GameObject charParent;
    private string[] foods;
    private string onScreenChar;
    const string ALPHABET = "abcdefghijklmnopqrstuvwxyz";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnEnable(){
        foods = new string[] {"sate", "bakso", "ayamgoreng", "burger", "pizza"}; 
        onScreenChar = "";
        charParent.SetActive(true);
    }

    void OnDisable(){
        charParent.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {   if(onScreenChar.Length > 1){
            return;
        }

        onScreenChar = shuffleChar();
        for(int i = 0; i <= onScreenChar.Length; i++){
            charText[i].text = "" + onScreenChar[i];
        }
    }

    string shuffleChar(){
        string shuffledChar = foods[0];
        while (shuffledChar.Length <= 15){
            shuffledChar = shuffledChar + ALPHABET[Random.Range(0, ALPHABET.Length-1)];
        }

        string newString = "";
        while (newString.Length <= 15){
            int randomIndex = Random.Range(0, shuffledChar.Length-1);
            newString = newString + shuffledChar[randomIndex];
            shuffledChar.Remove(randomIndex);
        }
        return newString;
    }
}

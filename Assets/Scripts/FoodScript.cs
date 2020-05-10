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
    public MiniGameController miniGameControllerInstance;
    public GameObject BackspaceButton;

    private Stack<GameObject> pressedChar;
    public List<Sprite> CurrentFoodSprite;
    private string activeFood;
    public Image CurrentFood;

    public GameObject Tutorial;

    public TextMeshProUGUI HintText;

    const float NEW_LINE = -75f;
    private bool clearAnswer;

    // Start is called before the first frame update
    void Start()
    {
        activeAnswers = new List<TextMeshProUGUI>();
        pressedChar = new Stack<GameObject>();
        clearButton = true;
    }

    void OnEnable(){
        foods = new List<string> (new string[] {"sate ayam", "bakso kuah", "ayam goreng", "beef burger", "cheese pizza"}); 
        onScreenChar = "";
        Tutorial.SetActive(true);
        clearAnswer = false;
    }

    void OnDisable(){
        charParent.SetActive(false);
        answerParent.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {   
        if(foods.Count <= 0){
            miniGameControllerInstance.CloseMiniGame(this.gameObject, "Makan");
            return;
        }

        if(clearButton){
            foreach(TextMeshProUGUI c in charText){
                c.transform.parent.gameObject.GetComponent<Button>().interactable = true;
            }
            clearButton = false;
        }

        if (miniGameControllerInstance.CooldownMistake){
            clearAnswer = true;
            return;
        }

        if(clearAnswer){
            foreach(TextMeshProUGUI a in activeAnswers){
                a.text = "";
            }

            clearAnswer = false;
        }

        if(onScreenChar.Length > 1 || miniGameControllerInstance.CoolingDown){
            return;
        }

        activeFood = foods[Random.Range(0, foods.Count - 1)];
        onScreenChar = shuffleChar();
        for(int i = 0; i < onScreenChar.Length; i++){
            charText[i].text = "" + onScreenChar[i];
        }

        generateAnswerSpace();         
        MapFoodSprite(activeFood);
    }

    string shuffleChar(){
        string shuffledChar = activeFood.Replace(" ", "");

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
        foreach(TextMeshProUGUI a in activeAnswers){
            Destroy(a.gameObject.transform.parent.gameObject);
        }

        activeAnswers.RemoveRange(0, activeAnswers.Count);

        int len = activeFood.Length;
        int i = 0;
        
        bool foundSpace = false;
        int spaceMark = 0;

        for(; i < len; i++){
            if(activeFood[i].ToString() == " "){
                foundSpace = true;
                spaceMark = i;
                continue;
            }

            GameObject answerPh = Instantiate(answerPlaceHolder, answerParent.transform.position, transform.rotation);

            if(foundSpace){
                answerPh.transform.Translate(new Vector3(((i - spaceMark - 1) * 56), NEW_LINE, 0f)); 
            } else {
                answerPh.transform.Translate(new Vector3((i * 56), 0f, 0f)); 
            }

            answerPh.transform.SetParent(answerParent.transform, false);
            activeAnswers.Add(answerPh.GetComponentInChildren<TextMeshProUGUI>());
        }
    }

    public void FillAnswer(GameObject charGo){
        pressedChar.Push(charGo.transform.parent.gameObject);
        string answer = "";
        string realAnswer = activeFood.Replace(" ", "");

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
                CurrentFood.color = new Color(1f, 1f, 1f, 1f);
                CurrentFood.gameObject.GetComponent<Animator>().enabled = true;
                foods.Remove(activeFood);
                onScreenChar = "";
                miniGameControllerInstance.AddProgressTrack(5 - foods.Count, 5);
            } else {
                miniGameControllerInstance.CooldownByMistake();
            }

            clearButton = true;
        }
    }

    public void RestartFood(){
        foods = new List<string> (new string[] {"sate ayam", "bakso kuah", "ayam goreng", "beef burger", "cheese pizza"});
        onScreenChar = "";
        clearButton = true;

        foreach(TextMeshProUGUI a in activeAnswers){
            Destroy(a.gameObject.transform.parent.gameObject);
        }

        activeAnswers.RemoveRange(0, activeAnswers.Count);
    }

    public void Backspace(){
        if (pressedChar.Count > 0){
            pressedChar.Pop().GetComponent<Button>().interactable = true;
        }

        foreach(TextMeshProUGUI a in activeAnswers){
            if(a.text == ""){
                int index = activeAnswers.IndexOf(a);
                if(index != 0){
                    activeAnswers[index-1].text = "";   
                }
                break;
            }
        }
    }

    public void MapFoodSprite(string name){
        switch(name){
			case "sate ayam":
				CurrentFood.sprite = CurrentFoodSprite[0];
                HintText.text = "Biasanya dipotong <b>kecil-kecil</b>.\n\nHidangan internasional yang menyerupai adalah <b>yakitori</b> dari Jepang.\n\nPeringkat <b>ke-14</b> dalam World's 50 Most Delicious Foods (50 Hidangan Paling Lezat di Dunia).";
				break;
			case "bakso kuah":
				CurrentFood.sprite = CurrentFoodSprite[1];
                HintText.text = "Dalam Bahasa Hokkien berarti <b>'daging giling'</b>.\n\nTerbuat dari <b>daging halal</b> seperti daging sapi, ikan, atau ayam.\n\nUmumnya disajikan panas-panas dengan <b>kuah kaldu sapi</b> bening dicampur <b>mi</b> dan ditaburi <b>bawang goreng</b>.";
				break;
			case "ayam goreng":
				CurrentFood.sprite = CurrentFoodSprite[2];
                HintText.text = "Beberapa <b>rumah makan siap saji</b> secara khusus menghidangkan makanan ini.\n\nTerbuat dari <b>daging unggas</b>.\n\nIdentik dengan karakter <b>Ipin</b> dalam serial TV anak Upin & Ipin.";
				break;
			case "beef burger":
				CurrentFood.sprite = CurrentFoodSprite[3];
                HintText.text = "Makanan ini dilengkapi dengan <b>keju, daun selada, saus, mayones dan tomat</b>.\n\nMakanan ini berasal dari <b>kota kedua terbesar</b> di Jerman.";
				break;
			case "cheese pizza":
				CurrentFood.sprite = CurrentFoodSprite[4];
                HintText.text = "Berasal dari <b>Italia</b>.\n\nJenis bahan biasanya terdapat <b>daging dan saus</b>.\n\nDitemukan sekitar tahun <b>1600</b>.";
				break;
			default:
				CurrentFood.sprite = CurrentFoodSprite[5];
                HintText.text = "";
				break;
		}

        CurrentFood.color = new Color(0.1f, 0.1f, 0.1f, 1f);
        CurrentFood.gameObject.GetComponent<Animator>().enabled = false;
    }
}

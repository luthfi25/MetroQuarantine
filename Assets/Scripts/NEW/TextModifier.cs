using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextModifier : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeText(string newText){
        TextMeshProUGUI textMeshProUGUI;
        if(TryGetComponent<TextMeshProUGUI>(out textMeshProUGUI)){
            textMeshProUGUI.text = newText;
        }
    }
}

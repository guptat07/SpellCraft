using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WordConstructionScript : MonoBehaviour
{
    public TextMeshProUGUI constructedWord;
    public TMP_InputField input;
    public GameObject welcomeTitle;
    public GameObject welcomeSubtitle;
    // Start is called before the first frame update
    void Start()
    {
        input.text = "";
        input.ActivateInputField();
        //gameObject.GetComponent<CanvasRenderer>().cull = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.anyKey)
        {
            //Debug.Log("pop");
            constructedWord.text = input.text;
            welcomeTitle.SetActive(false);
            welcomeSubtitle.SetActive(false);
        }
        if(constructedWord.text == "")
        {
            //Debug.Log("dop");
            //gameObject.GetComponent<CanvasRenderer>().cull = false;
            welcomeTitle.SetActive(true);
            welcomeSubtitle.SetActive(true);
        }
    }
}

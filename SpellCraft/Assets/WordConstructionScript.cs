using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Net.Http;


public class WordConstructionScript : MonoBehaviour
{
    public TextMeshProUGUI constructedWord;
    public TMP_InputField input;
    public GameObject welcomeTitle;
    public GameObject welcomeSubtitle;
    public TextMeshProUGUI definition;

    private static readonly HttpClient client = new HttpClient();

    string previous = "";


    // Start is called before the first frame update
    void Start()
    {
        definition.text = "";
        input.text = "";
        input.ActivateInputField();
        //gameObject.GetComponent<CanvasRenderer>().cull = false;
    }
        public static string getBetween(string strSource, string strStart, string strEnd)
    {
        if (strSource.Contains(strStart) && strSource.Contains(strEnd))
        {
            int Start, End;
            Start = strSource.IndexOf(strStart, 0) + strStart.Length;
            End = strSource.IndexOf(strEnd, Start);
            return strSource.Substring(Start, End - Start);
        }

        return "";
    }
    async void call()
    {
        string url = "https://api.dictionaryapi.dev/api/v2/entries/en/" + constructedWord.text;
        try
        {
            var response = await client.GetStringAsync(url);
            definition.text = getBetween(response, "\"definition\":\"", ".\"");
        }
        catch (HttpRequestException e)
        {
            definition.text = "";
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        if(Input.anyKey)
        {
            constructedWord.text = input.text;
            if (constructedWord.text != previous)
            {
                previous = constructedWord.text;
                call();
            }
            welcomeTitle.SetActive(false);
            welcomeSubtitle.SetActive(false);
        }


        if (constructedWord.text == "")
        {
            welcomeTitle.SetActive(true);
            welcomeSubtitle.SetActive(true);
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Net.Http;
using UnityEngine.Networking;
using UnityEngine.UI;


public class WordConstructionScript : MonoBehaviour
{
    public TextMeshProUGUI constructedWord;
    public TMP_InputField input;
    public GameObject welcomeTitle;
    public GameObject welcomeSubtitle;
    public TextMeshProUGUI definition;
    public GameObject image;


    private static readonly HttpClient client = new HttpClient();
    private static readonly HttpClient clientImage = new HttpClient();

    string previous = "";
    string imagejson = "";


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
        //string url = "https://www.merriam-webster.com/dictionary/" + constructedWord.text;
        //string url = "https://kids.wordsmyth.net/we/?ent=" + constructedWord.text;
        try
        {
            var response = await client.GetStringAsync(url);
            Debug.Log(response);
            string tempdef = getBetween(response, "\"definition\":\"", ",\"");
            definition.text = tempdef.Substring(0,tempdef.Length-2);

        }
        catch (HttpRequestException e)
        {
            definition.text = "";
        }
    }
    async void callimg() 
    {

        //string url2 = "https://www.googleapis.com/customsearch/v1?key=AIzaSyBUzs5vWXhaZzXnfngxMlmL4SKEwxFNbvM&cx=91c356bcaa3b847d8&searchType=image&q=" + constructedWord.text;
        string url2 = "https://www.google.com/search?tbm=isch&q=" + constructedWord.text;
        // var response2 = await clientImage.GetStringAsync(url2);
        // Debug.Log(response2);

        try {
            var response2 = await clientImage.GetStringAsync(url2);
            imagejson = getBetween(response2, "alt=\"\" src=\"", "\"");
            Debug.Log(imagejson);
                StartCoroutine(createImage(imagejson));
                                Debug.Log("step");
        }
        catch (HttpRequestException e) 
        {

        }
    }

    IEnumerator createImage(string tempurl)
    {
         Debug.Log("testing);");
         var imagetemp = image.GetComponent<Image>();
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(tempurl);
        Debug.Log("image gotten");
        yield return request.SendWebRequest();
        if (request.isNetworkError || request.isHttpError)
        {
            Debug.LogWarning(request.error);
            yield break;
        }        
        Debug.Log("createimage2");

        var texture = DownloadHandlerTexture.GetContent(request);
        var sprite1 = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), Vector2.one * 0.5f);
        imagetemp.sprite = sprite1;
        Debug.Log(imagetemp);
        Debug.Log("sprite set");
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
                callimg();

            }
            welcomeTitle.SetActive(false);
            welcomeSubtitle.SetActive(false);
            Debug.Log("definition" + previous);
        }


        if (constructedWord.text == "")
        {
            welcomeTitle.SetActive(true);
            welcomeSubtitle.SetActive(true);
        }

    }
}

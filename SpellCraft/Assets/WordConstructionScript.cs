using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Net.Http;
using UnityEngine.Networking;
using UnityEngine.UI;


public class WordConstructionScript : MonoBehaviour
{
    // Declare all variables needed to run the script. The script should be attached to a GameObject, and GameObjects will be assigned to these variables inside Unity Editor.
    public TextMeshProUGUI constructedWord;
    public TMP_InputField input;
    public GameObject welcomeTitle;
    public GameObject welcomeSubtitle;
    public TextMeshProUGUI definition;
    public GameObject image;

    public GameObject imageBg;

    public Camera cam;
    public Dictionary<string, string> RFIDToLetters = new Dictionary<string, string>();
    public string[] receivedRFIDs;
    public string wordFromRFID = "";

    public List<string> imageWords;

    public List<string> badWords;

    // These are for networking/internet API calling.
    private static readonly HttpClient client = new HttpClient();
    private static readonly HttpClient clientImage = new HttpClient();

    string previous = "";
    string imagejson = "";


    // Start is called before the first frame update
    void Start()
    {
        cam.backgroundColor = new Color(.145f,.157f,.572f);
        definition.text = "";
        input.text = "";
        // Activating the InputField so that user doesn't have to click on it
        input.ActivateInputField();
        //gameObject.GetComponent<CanvasRenderer>().cull = false;

        // Adding all unique RFID address to the dictionary, and mapping them to their corresponding letter
        // Have to hard-code this since each ID is unique and ties to a real-world/tangible input
        RFIDToLetters.Add("53b6e744010001", "A");
        RFIDToLetters.Add("5363e744010001", "A");
        RFIDToLetters.Add("53ace744010001", "B");
        RFIDToLetters.Add("53abe744010001", "C");
        RFIDToLetters.Add("537ee744010001", "C");
        RFIDToLetters.Add("53ade744010001", "D");
        RFIDToLetters.Add("53aee744010001", "E");
        RFIDToLetters.Add("536ee744010001", "E");
        RFIDToLetters.Add("53a3e744010001", "F");
        RFIDToLetters.Add("53a4e744010001", "G");
        RFIDToLetters.Add("53a5e744010001", "H");
        RFIDToLetters.Add("53a6e744010001", "I");
        RFIDToLetters.Add("5374e744010001", "I");
        RFIDToLetters.Add("539be744010001", "J");
        RFIDToLetters.Add("539ce744010001", "K");
        RFIDToLetters.Add("539de744010001", "L");
        RFIDToLetters.Add("5376e744010001", "L");
        RFIDToLetters.Add("539ee744010001", "M");
        RFIDToLetters.Add("533a5846010001", "N");
        RFIDToLetters.Add("5375e744010001", "N");
        RFIDToLetters.Add("5393e744010001", "O");
        RFIDToLetters.Add("5373e744010001", "O");
        RFIDToLetters.Add("5394e744010001", "P");
        RFIDToLetters.Add("5395e744010001", "Q");
        RFIDToLetters.Add("538ee744010001", "R");
        RFIDToLetters.Add("536de744010001", "R");
        RFIDToLetters.Add("538de744010001", "S");
        RFIDToLetters.Add("536ce744010001", "S");
        RFIDToLetters.Add("538ce744010001", "T");
        RFIDToLetters.Add("536be744010001", "T");
        RFIDToLetters.Add("538be744010001", "U");
        RFIDToLetters.Add("537be744010001", "V");
        RFIDToLetters.Add("5386e744010001", "W");
        RFIDToLetters.Add("5385e744010001", "X");
        RFIDToLetters.Add("5384e744010001", "Y");
        RFIDToLetters.Add("537de744010001", "Y");
        RFIDToLetters.Add("5383e744010001", "Z");

        // Whitelist for words to produce an image for
        imageWords.Add("cat");
        imageWords.Add("dog");
        imageWords.Add("fish");
        imageWords.Add("sheep");
        imageWords.Add("lamb");
        imageWords.Add("pig");
        imageWords.Add("cow");
        imageWords.Add("hen");
        imageWords.Add("whale");
        imageWords.Add("shark");
        imageWords.Add("zebra");
        imageWords.Add("mouse");
        imageWords.Add("rat");
        imageWords.Add("duck");
        imageWords.Add("goose");
        imageWords.Add("eagle");
        imageWords.Add("snail");
        imageWords.Add("horse");
        imageWords.Add("deer");
        imageWords.Add("crow");
        imageWords.Add("bird");
        imageWords.Add("water");

        // Blacklist of words to show nothing for
        badWords.Add("fuck");
        badWords.Add("shit");
        badWords.Add("ass");
        badWords.Add("cunt");
        badWords.Add("hell");
        badWords.Add("bitch");
        badWords.Add("bitc");
        badWords.Add("whore");
        badWords.Add("slut");
        badWords.Add("penis");
        badWords.Add("crap");
        badWords.Add("damn");
        badWords.Add("dick");
        badWords.Add("cock");
        badWords.Add("nig");
        badWords.Add("coon");
        badWords.Add("jap");
        badWords.Add("gook");
        badWords.Add("heck");
        badWords.Add("c");
        badWords.Add("ca");
        badWords.Add("fu");
        badWords.Add("fuc");
        badWords.Add("d");
        badWords.Add("do");
        badWords.Add("f");
        badWords.Add("fi");
        badWords.Add("fis");
        badWords.Add("s");
        badWords.Add("sh");
        badWords.Add("she");
        badWords.Add("shee");
        badWords.Add("l");
        badWords.Add("la");
        badWords.Add("lam");
        badWords.Add("p");
        badWords.Add("co");
        badWords.Add("h");
        badWords.Add("he");
        badWords.Add("w");
        badWords.Add("wh");
        badWords.Add("wha");
        badWords.Add("whal");
        badWords.Add("sha");
        badWords.Add("shar");
        badWords.Add("z");
        badWords.Add("ze");
        badWords.Add("zeb");
        badWords.Add("m");
        badWords.Add("mo");
        badWords.Add("mou");
        badWords.Add("r");
        badWords.Add("ra");
        badWords.Add("du");
        badWords.Add("duc");
        badWords.Add("g");
        badWords.Add("e");
        badWords.Add("ea");
        badWords.Add("eag");
        badWords.Add("eagl");
        badWords.Add("sn");
        badWords.Add("sna");
        badWords.Add("snai");
        badWords.Add("ho");
        badWords.Add("hor");
        badWords.Add("hors");
        badWords.Add("de");
        badWords.Add("dee");
        badWords.Add("cro");

    }
    
    // Method to process return from API Calls (getting data from JSON)
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
    
    // API Call for Dictionary Definition
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
    
    // API Call for Image
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

    // Taking the result of the API call made in callimg() and processing it into the actual picture
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
    
    // Ardity Required Method to Automatically Read Serial Messages
    // Invoked when a line of data is received from the serial device.
    void OnMessageArrived(string msg)
    {
        // Ensuring the data transfers!
        Debug.Log("Arrived: " + msg);

        // Process the string
        receivedRFIDs = msg.Split(',');
        foreach (string RFID in receivedRFIDs)
        {
            Debug.Log("RFID: " + RFID);
            if (RFID.Trim() == "EMPTY" || RFID.Trim() == "")
            {
                // Print the debug info and assign the variables in case of early exit
                input.text = wordFromRFID;
                Debug.Log("wordFromRFID is " + wordFromRFID + " and input.text is " + input.text);
                wordFromRFID = "";
                return;
            }
            else
            {
                wordFromRFID += RFIDToLetters[RFID.Trim()];
            }
        }

        // Print the debug info and assign the variables in case of max-letter-length word
        input.text = wordFromRFID;
        Debug.Log("wordFromRFID is " + wordFromRFID + " and input.text is " + input.text);
        wordFromRFID = "";
        return;
    }


    // Ardity Required Method to Connect and Disconnect
    // Invoked when a connect/disconnect event occurs. The parameter 'success' will be 'true' upon connection
    // The parameter 'success' will be 'false' upon disconnection or failure to connect.
    void OnConnectionEvent(bool success)
    {
        Debug.Log(success ? "Device connected" : "Device disconnected");
    }

    // Update is called once per frame
    void Update()
    {
        
        //if(Input.anyKey)
        //{
        //     constructedWord.text = input.text;
        //     if (constructedWord.text != previous)
        //     {
        //         previous = constructedWord.text;
        //         call();
        //         callimg();

        //     }
        //     welcomeTitle.SetActive(false);
        //     welcomeSubtitle.SetActive(false);
        //     // Debug.Log("definition" + previous);
        // //}


        // if (constructedWord.text == "")
        // {
        //     welcomeTitle.SetActive(true);
        //     welcomeSubtitle.SetActive(true);
        // }
        constructedWord.text = input.text;
        if (constructedWord.text == "")
        {
            welcomeTitle.SetActive(true);
            welcomeSubtitle.SetActive(true);
            image.SetActive(false);
            imageBg.SetActive(false);
            definition.enabled = false;
            cam.backgroundColor = new Color(.114f,.133f,.325f);
        } else
        {

            if (constructedWord.text != previous)
            {
                previous = constructedWord.text;
                if (imageWords.Contains(constructedWord.text.ToLower()))
                {
                    definition.enabled = true;
                    definition.transform.position = new Vector3(-300, 900);
                    definition.alignment = TextAlignmentOptions.Left;
                    image.SetActive(true);
                    imageBg.SetActive(true);
                    call();
                    callimg();
                    cam.backgroundColor = new Color(.0f,.522f,.263f);
                }
                else if (!badWords.Contains(constructedWord.text.ToLower()))
                {
                    definition.enabled = true;
                    definition.transform.position = new Vector3(0, 830);
                    definition.alignment = TextAlignmentOptions.Center; 
                    image.SetActive(false);
                    imageBg.SetActive(false);
                    call();
                    cam.backgroundColor = new Color(.0f,.522f,.263f);
                }
                else
                {
                    definition.enabled = false;
                    image.SetActive(false);
                    imageBg.SetActive(false);
                    cam.backgroundColor = new Color(.114f,.133f,.325f);
                }
            }
            welcomeTitle.SetActive(false);
            welcomeSubtitle.SetActive(false);
        }

    }
}

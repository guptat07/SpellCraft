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
    public Dictionary<string, string> RFIDToLetters = new Dictionary<string, string>();
    public string[] receivedRFIDs;
    public string wordFromRFID = "";

    // These are for networking/internet API calling.
    private static readonly HttpClient client = new HttpClient();
    private static readonly HttpClient clientImage = new HttpClient();

    string previous = "";
    string imagejson = "";


    // Start is called before the first frame update
    void Start()
    {
        definition.text = "";
        input.text = "";
        // Activating the InputField so that user doesn't have to click on it
        input.ActivateInputField();
        //gameObject.GetComponent<CanvasRenderer>().cull = false;

        // Adding all unique RFID address to the dictionary, and mapping them to their corresponding letter
        // Kind of have to hard-code this since each ID is unique and ties to a real-world/tangible input
        RFIDToLetters.Add("53b6e744010001", "A");
        RFIDToLetters.Add("53ace744010001", "B");
        RFIDToLetters.Add("53abe744010001", "C");
        RFIDToLetters.Add("53ade744010001", "D");
        RFIDToLetters.Add("53aee744010001", "E");
        RFIDToLetters.Add("53a3e744010001", "F");
        RFIDToLetters.Add("53a4e744010001", "G");
        RFIDToLetters.Add("53a5e744010001", "H");
        RFIDToLetters.Add("53a6e744010001", "I");
        RFIDToLetters.Add("539be744010001", "J");
        RFIDToLetters.Add("539ce744010001", "K");
        RFIDToLetters.Add("539de744010001", "L");
        RFIDToLetters.Add("539ee744010001", "M");
        RFIDToLetters.Add("533a5846010001", "N");
        RFIDToLetters.Add("5393e744010001", "O");
        RFIDToLetters.Add("5394e744010001", "P");
        RFIDToLetters.Add("5395e744010001", "Q");
        RFIDToLetters.Add("538ee744010001", "R");
        RFIDToLetters.Add("538de744010001", "S");
        RFIDToLetters.Add("538ce744010001", "T");
        RFIDToLetters.Add("538be744010001", "U");
        RFIDToLetters.Add("537be744010001", "V");
        RFIDToLetters.Add("5386e744010001", "W");
        RFIDToLetters.Add("5385e744010001", "X");
        RFIDToLetters.Add("5384e744010001", "Y");
        RFIDToLetters.Add("5383e744010001", "Z");
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
            if (RFID.Trim() == "EMPTY")
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
        //}


        if (constructedWord.text == "")
        {
            welcomeTitle.SetActive(true);
            welcomeSubtitle.SetActive(true);
        }

    }
}

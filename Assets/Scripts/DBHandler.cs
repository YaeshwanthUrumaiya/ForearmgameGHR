using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class DBHandler : MonoBehaviour
{

    public string userName = "Yaeshwanth Urumaiya";
    public string gameName = "ForearmSpaceGame";
    public string dateTime = "2024-08-03"; // Format YYYY-MM-DD
    public bool TriggerAPICall = false;
    public Dictionary<string, object> valKEYData = new Dictionary<string, object>
    {   
        ["avgleftrangeangle"] = 0,
        ["avgrightrangeangle"] = 0
    };

    // Start is called before the first frame update
    void Start()
    {
        
    }
    void Update(){
        if(TriggerAPICall){
            StartCoroutine(APITest());
            string ValData = DictionaryToStringConverter.ConvertDictionaryToString(valKEYData);
            // Debug.Log(result);
            StartCoroutine(APISendData(userName, gameName, dateTime, ValData));
            TriggerAPICall = false;
        }
    }

    private IEnumerator APITest()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get("http://yu8.pythonanywhere.com/GameDB/db-test/"))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(webRequest.error);
            }
            else
            {
                string jsonResponse = webRequest.downloadHandler.text;
                Debug.Log(jsonResponse);
            }
        }
    }
    private IEnumerator APISendData(string UserName, string GameName, string DateTime, string ValueData)
    {
        using (UnityWebRequest webRequest = new UnityWebRequest("http://yu8.pythonanywhere.com/GameDB/add-data/", "POST"))
        {
            webRequest.SetRequestHeader("Content-Type", "text/plain");

            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes("{\"UserName\" : \""+UserName+"\", \"GameName\": \""+GameName+"\", \"DateTime\": \""+DateTime+"\", \"Values\": {"+ValueData+"} }");
            
            webRequest.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);

            yield return webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error");
            }
            else
            {
                Debug.Log("Added");
            }
        }
    }
}

public class DictionaryToStringConverter
{
    public static string ConvertDictionaryToString(Dictionary<string, object> dataToSend)
    {
        var sb = new System.Text.StringBuilder();

        int currentIndex = 0;
        int totalCount = dataToSend.Count;

        foreach (var kvp in dataToSend)
        {
            sb.Append($"\"{kvp.Key}\"");

            if (kvp.Value == null)
            {
                sb.Append(": null");
            }
            else
            {
                if (kvp.Value is string stringValue)
                {
                    sb.Append($": \"{stringValue}\"");
                }
                else if (kvp.Value is int intValue)
                {
                    sb.Append($": {intValue}");
                }
                else if (kvp.Value is float floatValue)
                {
                    sb.Append($": {floatValue}");
                }
                else if (kvp.Value is double doubleValue)
                {
                    sb.Append($": {doubleValue}");
                }
                else
                {
                    sb.Append(": \"Other\"");
                }
            }

            if (currentIndex < totalCount - 1)
            {
                sb.Append(", ");
            }

            currentIndex++;
        }

        return sb.ToString();
    }
}

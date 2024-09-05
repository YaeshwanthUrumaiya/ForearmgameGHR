using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarControl : MonoBehaviour
{
    public UpdDataHander updDataHander; // Ensure this script exists and is accessible
    public float data;
    public bool IsKeyBoard = true;
    public float carSpeed;
    public float leftlimitofroad = -4.4f;
    public float rightlimitofroad  = 4.4f;
    Vector3 position;
    public bool GameOver = false;
    public float currentleftrangeangle = 0f;
    public float currentrightrangeangle = 0f;
    public float avgleftrangeangle = 0f;
    public float avgrightrangeangle = 0f;
    public float countleftrangeangle = 0f;
    public float countrightrangeangle = 0f;
    public DBHandler DBAPICaller;


    // Start is called before the first frame update
    void Start()
    {
        position = transform.position; 
    }

    // Update is called once per frame
    void Update()
    {   
        if(IsKeyBoard){
            position.x += Input.GetAxis("Horizontal") * carSpeed * Time.deltaTime;
            position.x = Mathf.Clamp(position.x, leftlimitofroad, rightlimitofroad);
            transform.position = position;
        }else{
            List<double> values = SplitAndConvert(updDataHander.data);
        double MoveData = values[0];
        double RangeOfMotion = values[1];
        data = (float) MoveData; // Make sure updDataHander.data is not null or empty
        // Debug.Log(updDataHander.data);
        // Here,  Input.GetAxis("Horizontal") is the variable which we need to change. it denotes if your car is moving left or not. So when moving left, the value is negative and the value is postive when moving right.
        // When moving the left or right. just sending -1 or +1 should be fine.
        position.x += data * carSpeed * Time.deltaTime;
        position.x = Mathf.Clamp(position.x, leftlimitofroad, rightlimitofroad);
        transform.position = position;
        if(data == 1){
            // right
            currentrightrangeangle += (float)RangeOfMotion;
            countrightrangeangle++;
            avgrightrangeangle = currentrightrangeangle / countrightrangeangle;

        }
        if(data == -1){
            // left
            currentleftrangeangle += (float)RangeOfMotion;
            countleftrangeangle++;
            avgleftrangeangle = currentleftrangeangle / countleftrangeangle;

        }
        // Debug.Log("Right Angle Average: " + avgrightrangeangle + ";Left Angle Average: " + avgleftrangeangle);
        // Debug.Log(SplitAndConvert(updDataHander.data));

        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("ECAR"))
        {
            GameOver = true;
            DBAPICaller.valKEYData["avgrightrangeangle"] = avgrightrangeangle;
            DBAPICaller.valKEYData["avgleftrangeangle"] = avgleftrangeangle;
            DBAPICaller.TriggerAPICall = true;
            Debug.Log("Right Angle Average: " + avgrightrangeangle + ";Left Angle Average: " + avgleftrangeangle);
            StartCoroutine(DelayedDeletion());
        }
    }

    IEnumerator DelayedDeletion()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }

    public static List<double> SplitAndConvert(string input)
    {
        // Split the string into an array of substrings
        string[] valuesArray = input.Split(',');

        // Initialize a list to hold the parsed values
        List<double> valuesList = new List<double>();

        // Iterate through the array and add each value to the list after parsing
        foreach (var valueString in valuesArray)
        {
            // Trim the string to remove leading/trailing spaces
            var trimmedValue = valueString.Trim();

            // Attempt to convert the trimmed string to a double
            double value;
            if (double.TryParse(trimmedValue, out value))
            {
                // Successfully parsed, add to the list
                valuesList.Add(value);
            }
            else
            {
                Debug.Log($"Failed to parse '{trimmedValue}'");
            }
        }

        return valuesList;
    }
}

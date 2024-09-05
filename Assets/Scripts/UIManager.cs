using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text scoreText;
    public GameObject car; 
    public int score;

    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        InvokeRepeating("UpdateScore", 1.0f, 0.5f); // Corrected parameter to float
    }

    // Update is called once per frame
    void Update()
    {
        if (!car.GetComponent<CarControl>().GameOver) 
        {
            scoreText.text = "Score: " + score.ToString();
        }
    }

    void UpdateScore()
    {
        score++; 
    }
}

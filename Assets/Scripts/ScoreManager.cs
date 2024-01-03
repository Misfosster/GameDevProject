using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public Text scoreText;
    public Text totalText;
    private float scoreCount = 1000f;  // Initial score value
    private float totalCount = 500f;  // Initial score value
    private float timeElapsed = 0f;
    public float subtractInterval = 1f;  // Time interval to subtract score

    void Update()
    {
        // Update the time elapsed
        timeElapsed += Time.deltaTime;

        // Check if the subtractInterval has passed
        if (timeElapsed >= subtractInterval)
        {
            // Subtract the score
            subtractScore();

            // Reset the timeElapsed for the next interval
            timeElapsed = 0f;
        }

        // Update the score text
        scoreText.text = "Score: " + Mathf.Round(scoreCount);
        totalText.text= "Total: "+ totalCount;
    }

    void subtractScore()
    {
        // Subtract 3 from the scoreCount
        scoreCount -= 3f;
        if(scoreCount==0){
            scoreCount =0;
        }
    }
    void totalHandler(){
        totalCount+=scoreCount;
    }
   public void scoreReset(){
        scoreCount=1000f;
    }
    void subtractTotal(float price){
        totalCount-=price;
    }
    public void addTotal(){
        totalCount+=scoreCount;
    }

    public void addScore(){
        scoreCount+=50f;
    }
}

using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Transform ball;
    public float startSpeed = 3f;
    public GoalTrigger leftGoalTrigger;
    public GoalTrigger rightGoalTrigger;

    int leftPlayerScore;
    int rightPlayerScore;
    
    public TextMeshProUGUI scoreText;
    
    Vector3 ballStartPos;

    const int scoreToWin = 11;
    
    //---------------------------------------------------------------------------
    void Start()
    {
        scoreText.text = leftPlayerScore + " : " + rightPlayerScore;
        ballStartPos = ball.position;
        Rigidbody ballBody = ball.GetComponent<Rigidbody>();
        ballBody.linearVelocity = new Vector3(1f, 0f, 0f) * startSpeed;
    }

    //---------------------------------------------------------------------------
    public void OnGoalTrigger(GoalTrigger trigger)
    {
        // If the ball entered a goal area, increment the score, check for win, and reset the ball
        scoreText.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);

        if (trigger == leftGoalTrigger)
        {
            rightPlayerScore++;
            Debug.Log($"Right player scored: {rightPlayerScore}");
            scoreText.text = leftPlayerScore + " : " + rightPlayerScore;
            
            if (rightPlayerScore == scoreToWin) {
                Debug.Log("Right player wins!");
                ResetBall(-1f);
                leftPlayerScore = 0;
                rightPlayerScore = 0;
                scoreText.text = leftPlayerScore + " : " + rightPlayerScore;

            } else {
                ResetBall(-1f);
            }
        }
        else if (trigger == rightGoalTrigger)
        {
            leftPlayerScore++;
            Debug.Log($"Left player scored: {leftPlayerScore}");
            scoreText.text = leftPlayerScore + " : " + rightPlayerScore;
            
            if (leftPlayerScore == scoreToWin) {
                Debug.Log("Right player wins!");
                ResetBall(1f);
                rightPlayerScore = 0;
                leftPlayerScore = 0;
                scoreText.text = leftPlayerScore + " : " + rightPlayerScore;

            }
            else {
                ResetBall(1f);
            }
        }
    }

    //---------------------------------------------------------------------------
    void ResetBall(float directionSign)
    {
        ball.position = ballStartPos;

        // Start the ball within 20 degrees off-center toward direction indicated by directionSign
        directionSign = Mathf.Sign(directionSign);
        Vector3 newDirection = new Vector3(directionSign, 0f, 0f) * startSpeed;
        newDirection = Quaternion.Euler(0f, Random.Range(-20f, 20f), 0f) * newDirection;

        var rbody = ball.GetComponent<Rigidbody>();
        rbody.linearVelocity = newDirection;
        rbody.angularVelocity = new Vector3();

        // We are warping the ball to a new location, start the trail over
        ball.GetComponent<TrailRenderer>().Clear();
    }
}

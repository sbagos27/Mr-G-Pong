using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

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
    
    public GameObject leftPaddle;
    public GameObject rightPaddle;
    
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
                startSpeed = -3;
                ResetBall(1f);
                leftPlayerScore = 0;
                rightPlayerScore = 0;
                scoreText.text = leftPlayerScore + " : " + rightPlayerScore;

            } else {
                ResetBall(1f);
            }
        }
        else if (trigger == rightGoalTrigger)
        {
            leftPlayerScore++;
            Debug.Log($"Left player scored: {leftPlayerScore}");
            scoreText.text = leftPlayerScore + " : " + rightPlayerScore;
            
            if (leftPlayerScore == scoreToWin) {
                Debug.Log("Right player wins!");
                startSpeed = -3;
                ResetBall(-1f);
                rightPlayerScore = 0;
                leftPlayerScore = 0;
                scoreText.text = leftPlayerScore + " : " + rightPlayerScore;

            }
            else {
                ResetBall(-1f);
            }
        }
        if (leftPlayerScore % 3 == 0 && leftPlayerScore != 0)
        {
            ChangePaddleSize(leftPaddle);
        }
        if (rightPlayerScore % 3 == 0 && rightPlayerScore != 0)
        {
            ChangePaddleSize(rightPaddle);
        }

        if (leftPlayerScore + rightPlayerScore == 10)
        {
            ChangePaddleSpeed(leftPaddle);
            ChangePaddleSpeed(rightPaddle);
        }

        ResetPitch(leftPaddle);
        ResetPitch(rightPaddle);
    }

    void ResetPitch(GameObject paddle)
    {
        AudioSource audioSource = paddle.GetComponent<AudioSource>();
        audioSource.pitch = 1f;
    }

    void ChangePaddleSpeed(GameObject paddle)
    {
        Paddle pad = paddle.GetComponent<Paddle>();
        pad.speed = 12;
    }
    
    void ChangePaddleSize(GameObject paddle)
    {
        paddle.transform.localScale = new Vector3(0.75f, 1f, 6f);
        StartCoroutine(ResetPaddleSize(paddle));
    }

    IEnumerator ResetPaddleSize(GameObject paddle)
    {
        yield return new WaitForSeconds(5f);
        paddle.transform.localScale = new Vector3(0.75f, 1f, 4f); // Reset to original size
        
    }

    //---------------------------------------------------------------------------
    void ResetBall(float directionSign)
    {
        ball.position = ballStartPos;

        // Start the ball within 20 degrees off-center toward direction indicated by directionSign
        directionSign = Mathf.Sign(directionSign);
        Vector3 newDirection = new Vector3(directionSign, 0f, 0f) * startSpeed--;
        newDirection = Quaternion.Euler(0f, Random.Range(-20f, 20f), 0f) * newDirection;

        var rbody = ball.GetComponent<Rigidbody>();
        rbody.linearVelocity = newDirection;
        rbody.angularVelocity = new Vector3();

        // We are warping the ball to a new location, start the trail over
        ball.GetComponent<TrailRenderer>().Clear();
    }
}

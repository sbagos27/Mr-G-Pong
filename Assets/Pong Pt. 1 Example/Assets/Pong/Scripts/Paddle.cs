using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Paddle : MonoBehaviour
{
    public float maxTravelHeight;
    public float minTravelHeight;
    
    public float speed;
    public float collisionBallSpeedUp = 1.5f;

    public AudioClip paddleHit;
    
    public InputActionReference move;
    private void OnEnable()
    {
        move.action.Enable();
    }
    //THIS IS NEEDED TO HAVE BOTH MOVE
    private void OnDisable()
    {
        move.action.Disable();
    }
    //-----------------------------------------------------------------------------
    void Update()
    {   
        Vector3 paddleDirection = move.action.ReadValue<Vector3>();
        Vector3 newPosition = transform.position + paddleDirection * speed * Time.deltaTime;
        newPosition.z = Mathf.Clamp(newPosition.z, minTravelHeight, maxTravelHeight);

        transform.position = newPosition;
    }

    //-----------------------------------------------------------------------------
    void OnCollisionEnter(Collision other)
    {
        AudioSource audioSrc = GetComponent<AudioSource>();
        audioSrc.clip = paddleHit;
        audioSrc.Play();
        audioSrc.pitch += 0.2f;
        //Play Audio
        
        var paddleBounds = GetComponent<BoxCollider>().bounds;
        float maxPaddleHeight = paddleBounds.max.z;
        float minPaddleHeight = paddleBounds.min.z;

        // Get the percentage height of where it hit the paddle (0 to 1) and then remap to -1 to 1 so we have symmetry
        float pctHeight = (other.transform.position.z - minPaddleHeight) / (maxPaddleHeight - minPaddleHeight);
        float bounceDirection = (pctHeight - 0.5f) / 0.5f;
        // Debug.Log($"pct {pctHeight} + bounceDir {bounceDirection}");

        // flip the velocity and rotation direction
        Vector3 currentVelocity = other.relativeVelocity;
        float newSign = -Math.Sign(currentVelocity.x);
        float newRotSign = -newSign;;

        // Change the velocity between -60 to 60 degrees based on where it hit the paddle
        float newSpeed = currentVelocity.magnitude * collisionBallSpeedUp;
        Vector3 newVelocity = new Vector3(newSign, 0f, 0f) * newSpeed;
        newVelocity = Quaternion.Euler(0f, newRotSign * 60f * bounceDirection, 0f) * newVelocity;
        other.rigidbody.linearVelocity = newVelocity;

        // Debug.DrawRay(other.transform.position, newVelocity, Color.yellow);
        // Debug.Break();
    }
}

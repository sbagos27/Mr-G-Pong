using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GoalTrigger : MonoBehaviour
{
    public GameManager gameManager;
    public CameraShake cameraShake;

    //---------------------------------------------------------------------------
    void OnTriggerEnter(Collider other)
    {
        StartCoroutine(cameraShake.Shake(0.15f, 0.4f));
        gameManager.OnGoalTrigger(this);
    }
}

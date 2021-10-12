using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    Rigidbody thisBallBody;

    private void Start()
    {
        thisBallBody = GetComponent<Rigidbody>();
    }

    void StopVelocity()
    {
        thisBallBody.velocity = Vector3.zero;
        thisBallBody.angularVelocity = Vector3.zero;
    }
}
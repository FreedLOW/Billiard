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

    private void Update()
    {
        ClampPosition();
    }

    void StopVelocity()
    {
        thisBallBody.velocity = Vector3.zero;
        thisBallBody.angularVelocity = Vector3.zero;
    }

    private void ClampPosition()
    {
        var yPos = Mathf.Clamp(transform.position.y, 31, 34);
        transform.position = new Vector3(transform.position.x, yPos, transform.position.z);
    }
}
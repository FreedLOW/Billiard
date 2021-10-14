using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private Rigidbody thisBallBody;
    private int minY = 31, maxY = 34;

    private void Start()
    {
        thisBallBody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        ClampPosition();
    }

    private void ClampPosition()
    {
        var yPos = Mathf.Clamp(transform.position.y, minY, maxY);
        thisBallBody.position = new Vector3(thisBallBody.position.x, yPos, thisBallBody.position.z);
    }
}
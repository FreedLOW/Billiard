using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBall : MonoBehaviour
{
    [SerializeField] private float forceSpeed = 10f;
    [SerializeField] GameObject stick;  //его отодвигать

    private Rigidbody ballBody;
    private LineRenderer lineRenderer;

    private Vector3 direction;

    [SerializeField] Transform ball;

    private void Start()
    {
        ballBody = GetComponent<Rigidbody>();
        //lineRenderer = GetComponent<LineRenderer>();

        Invoke("AddForceToBall", 2f);
    }

    private void FixedUpdate()
    {

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Debug.DrawLine(transform.position, ball.position);
        Gizmos.DrawSphere(ball.position, 1f);
    }

    private void AddForceToBall()
    {
        //рассчитывать направление и его примень к скорости:
        direction = -Vector3.forward;

        //ballBody.AddForce(-Vector3.forward * forceSpeed);
        //ballBody.velocity = direction * forceSpeed;
    }

    private void DrawLine()
    {

    }

    public void ResetValues()
    {
        ballBody.velocity = Vector3.zero;
        ballBody.angularVelocity = Vector3.zero;
    }
}
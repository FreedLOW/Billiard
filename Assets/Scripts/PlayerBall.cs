using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBall : MonoBehaviour
{
    [SerializeField] private float forceSpeed = 10f;
    [SerializeField] GameObject stick;  //его отодвигать
    [SerializeField] Transform ghostBall;
    private Transform targetBall;

    private const float Speed = 10f;
    private Rigidbody ballBody;
    private LineRenderer lineRenderer;

    private Vector3 direction;

    [SerializeField] Transform ball;

    private void Start()
    {
        ballBody = GetComponent<Rigidbody>();
        lineRenderer = GetComponent<LineRenderer>();

        //Invoke("AddForceToBall", 2f);
    }

    private void FixedUpdate()
    {
        DrawLine();
    }

    public void AddForceToBall(Vector3 forceDirection, float force)
    {
        //рассчитывать направление и его примень к скорости:
        //direction = -Vector3.forward;
        direction = forceDirection;

        //ballBody.AddForce(-Vector3.forward * forceSpeed);
        //ballBody.velocity = direction * forceSpeed;
        ballBody.velocity = direction * (Speed * force);
    }

    private void DrawLine()
    {
        Ray playerBallRay = new Ray(transform.position, -transform.forward);
        Ray ballRay;
        RaycastHit hit;
        var hitBall = Hit(playerBallRay, out ballRay, out hit);

        if (hitBall)
        {
            Debug.DrawLine(playerBallRay.origin, hit.point);
            Debug.DrawLine(ballRay.origin, ballRay.origin + 3 * ballRay.direction);

            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, playerBallRay.origin);
            lineRenderer.SetPosition(2, hit.point);
            lineRenderer.SetPosition(3, ballRay.origin);
            lineRenderer.SetPosition(4, ballRay.origin + 3 * ballRay.direction);
        }

        //Ray origin = new Ray(transform.position, -transform.forward);
        //Ray left = new Ray(transform.position + (-transform.right * 0.5f), -transform.forward);
        //Ray right = new Ray(transform.position + (transform.right * 0.5f), -transform.forward);

        //bool hiting = AimToBall(origin);
        //if (!hiting)
        //{
        //    hiting = AimToBall(left);

        //    if (!hiting)
        //        hiting = AimToBall(right);
        //}
    }

    bool Hit(Ray ray, out Ray deflected, out RaycastHit hit)
    {
        if(Physics.Raycast(ray, out hit))
        {
            if (hit.collider.GetComponent<Ball>())
            {
                lineRenderer.enabled = true;
                Vector3 normal = hit.normal;
                Vector3 deflect = Vector3.Reflect(ray.direction, normal);
                deflected = new Ray(hit.point, deflect);
                ghostBall.position = hit.point;
                return true;
            }
            else
            {
                lineRenderer.enabled = false;
                ghostBall.position = Vector3.zero;
            }
        }

        deflected = new Ray(Vector3.zero, Vector3.zero);
        return false;
    }

    bool AimToBall(Ray ray)
    {
        RaycastHit hit;
        if(Physics.Raycast(ray,out hit))
        {
            targetBall = hit.transform;

            Debug.DrawLine(ray.origin, hit.point);

            Vector3 pointer = transform.position - hit.transform.position;

            float b = Vector3.Dot(-transform.right, pointer);
            float a = Mathf.Sqrt(Mathf.Pow(2 * 0.5f, 2) - b * b);

            ghostBall.position = hit.transform.position;
            Vector3 pos = transform.TransformPoint(new Vector3(-b, transform.position.y, -a));
            ghostBall.Translate(pos);

            return true;
        }

        return false;
    }

    public void ResetValues()
    {
        ballBody.velocity = Vector3.zero;
        ballBody.angularVelocity = Vector3.zero;
    }
}
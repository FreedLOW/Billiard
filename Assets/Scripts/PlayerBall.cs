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

    [SerializeField] private Vector3 direction;
    private Quaternion originRotation;
    float horizontalRotation;
    [SerializeField] float rotationSpeed = 0.5f;

    [SerializeField] private bool hitBall;

    [SerializeField] Transform ball;

    private void Start()
    {
        ballBody = GetComponent<Rigidbody>();
        lineRenderer = GetComponent<LineRenderer>();

        originRotation = transform.rotation;

        //Invoke("AddForceToBall", 2f);
    }

    private void Update()
    {
        //DrawLine();
        //ClampPosition();
        KickBall();
    }

    private void ClampPosition()
    {
        var yPos = Mathf.Clamp(transform.position.y, 33, 34);
        transform.position = new Vector3(transform.position.x, yPos, transform.position.z);
    }

    public void AddForceToBall(float force)
    {
        //рассчитывать направление и его примень к скорости:
        //direction = -Vector3.forward;
        //direction = forceDirection;

        //ballBody.AddForce(direction * (Speed * force));
        //ballBody.velocity = direction * forceSpeed;
        ballBody.velocity = direction * (Speed * force);
    }

    private void KickBall()
    {
        if (Input.touchCount > 0)
        {
            DrawLine();

            var touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                ResetValues();
                transform.rotation = Quaternion.identity;
            }

            if (touch.phase == TouchPhase.Moved)
            {
                //тут ещё настроить вращение по У, может блочит вращени по x, z:
                horizontalRotation += touch.deltaPosition.y * rotationSpeed * Time.deltaTime;
                Quaternion rotationY = Quaternion.AngleAxis(horizontalRotation, Vector3.up);
                transform.rotation = originRotation * rotationY;
            }

            if (touch.phase == TouchPhase.Ended && hitBall)
            {
                //тут вычитывать силу от шара до точки натяга и прикладывать силу в соответствующем направлении:

                //Vector3 forceDirection = (transform.position + (Vector3)touch.deltaPosition).normalized;

                //правильно рассчитать силу:
                //var force = transform.position.z + touch.deltaPosition.y;
                AddForceToBall(4f);
            }
        }
        else
        {
            lineRenderer.enabled = false;
            ghostBall.position = Vector3.zero;
        }
}

    private void DrawLine()
    {
        //выпускать луч в противоположную сторону тача:
        //if (Input.touchCount > 0)
        //{
        //    Touch touch = Input.GetTouch(0);
        //    Ray playerRay = new Ray(transform.position, -touch.position);
        //    Debug.DrawRay(transform.position, -touch.position, Color.black);
        //}

        Ray playerBallRay = new Ray(transform.position, -transform.forward);
        Ray ballRay;
        RaycastHit hit;
        hitBall = Hit(playerBallRay, out ballRay, out hit);
        direction = playerBallRay.direction;

        if (hitBall)
        {
            //убрать это потом:
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
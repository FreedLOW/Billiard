using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBall : MonoBehaviour
{
    [SerializeField] private float forceSpeed = 10f;
    [SerializeField] GameObject stick;  //его отодвигать
    [SerializeField] Transform ghostBall;
    private Transform targetBall;

    private const float Speed = 5f;
    private Rigidbody ballBody;
    private LineRenderer lineRenderer;

    [SerializeField] private Vector3 direction;
    private Quaternion originRotation;
    float horizontalRotation;
    [SerializeField] float rotationSpeed = 0.5f;

    [SerializeField] private bool hitBall;
    [SerializeField] private bool canHit;

    [SerializeField] Transform ball;

    private void Start()
    {
        ballBody = GetComponent<Rigidbody>();
        lineRenderer = GetComponent<LineRenderer>();

        originRotation = transform.rotation;
    }

    private void FixedUpdate()
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

    private void AddForceToBall(float force)
    {
        //рассчитывать направление и его примень к скорости:

        //ballBody.AddForce(direction * (Speed * force));
        ballBody.velocity = direction * (Speed * force);// * Time.deltaTime);
    }

    private void KickBall()
    {
        if (Input.touchCount > 0)
        {
            DrawLine();

            var touch = Input.GetTouch(0);
            var touchposition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var worldTouchPosition = new Vector3(touchposition.x, transform.position.y, touchposition.z);
            if (touch.phase == TouchPhase.Began)
            {
                ResetValues();
                transform.rotation = Quaternion.identity;
            }

            if (touch.phase == TouchPhase.Moved)
            {
                //тут ещё настроить вращение по У, может блочит вращени по x, z:

                //horizontalRotation += touch.deltaPosition.y * rotationSpeed * Time.deltaTime;
                //Quaternion rotationY = Quaternion.AngleAxis(horizontalRotation, Vector3.up);
                //transform.rotation = originRotation * rotationY;

                //var rotY = Quaternion.Euler(0f, touch.deltaPosition.x * rotationSpeed * Time.deltaTime, 0f);
                //transform.rotation = rotY * transform.rotation;

                transform.LookAt(worldTouchPosition);
            }

            if (touch.phase == TouchPhase.Ended && canHit)
            {
                //тут вычитывать силу от шара до точки натяга и прикладывать силу в соответствующем направлении:

                //Vector3 forceDirection = (transform.position + (Vector3)touch.deltaPosition).normalized;

                //правильно рассчитать силу:
                forceSpeed = Vector3.Distance(transform.position, worldTouchPosition);
                AddForceToBall(forceSpeed);
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
        if (Input.touchCount > 0)
        {
            //Touch touch = Input.GetTouch(0);
            //var touch = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //var mewTouch = new Vector3(touch.x, transform.position.y, touch.z);
            //Debug.Log("ge = " + mewTouch);
            //Ray playerRay = new Ray(transform.position, mewTouch);
            //Debug.DrawLine(transform.position, mewTouch, Color.black);
        }

        Ray playerBallRay = new Ray(transform.position, -transform.forward);
        Ray ballRay;
        RaycastHit hit;
        hitBall = Hit(playerBallRay, out ballRay, out hit);
        direction = playerBallRay.direction;

        if (hitBall && hit.collider.GetComponent<Ball>())
        {
            canHit = true;

            //убрать это потом:
            Debug.DrawLine(playerBallRay.origin, hit.point, Color.red);
            Debug.DrawLine(ballRay.origin, ballRay.origin + 3 * ballRay.direction);

            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, playerBallRay.origin);
            lineRenderer.SetPosition(2, hit.point);
            lineRenderer.SetPosition(3, ballRay.origin);
            lineRenderer.SetPosition(4, ballRay.origin + 3 * ballRay.direction);
        }
        else
        {
            lineRenderer.enabled = false;
            canHit = false;
        }
    }

    bool Hit(Ray ray, out Ray deflected, out RaycastHit hit)
    {
        if(Physics.Raycast(ray, out hit))
        {
            lineRenderer.enabled = true;
            Vector3 normal = hit.normal;
            Vector3 deflect = Vector3.Reflect(ray.direction, normal);
            deflected = new Ray(hit.point, deflect);
            ghostBall.position = hit.point;
            return true;
        }

        deflected = new Ray(Vector3.zero, Vector3.zero);
        return false;
    }

    public void ResetValues()
    {
        ballBody.velocity = Vector3.zero;
        ballBody.angularVelocity = Vector3.zero;
    }
}
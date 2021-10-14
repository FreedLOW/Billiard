using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBall : MonoBehaviour
{
    [SerializeField] private float forceSpeed;
    [SerializeField] Transform ghostBall;

    private const float Speed = 5f;
    private Rigidbody ballBody;
    private LineRenderer lineRenderer;
    private int minY = 32, maxY = 34;

    [SerializeField] private Vector3 direction;

    [SerializeField] private bool hitBall;

    private void Start()
    {
        ballBody = GetComponent<Rigidbody>();
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void FixedUpdate()
    {
        ClampPosition();
        KickBall();
    }

    private void ClampPosition()
    {
        var yPos = Mathf.Clamp(transform.position.y, minY, maxY);
        ballBody.position = new Vector3(ballBody.position.x, yPos, ballBody.position.z);
    }

    private void AddForceToBall(float force)
    {
        //рассчитывать направление и его примень к скорости:
        ballBody.velocity = direction * (Speed * force);
    }

    private void KickBall()
    {
        if (Input.touchCount > 0)
        {
            DrawLine();

            var touch = Input.GetTouch(0);
            var touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var worldTouchPosition = new Vector3(touchPosition.x, transform.position.y, touchPosition.z);

            if (touch.phase == TouchPhase.Began)
            {
                ResetValues();
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

            if (touch.phase == TouchPhase.Ended && hitBall)
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
            hitBall = false;
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
        Ray ghostBallRay;
        RaycastHit hit;
        hitBall = Hit(playerBallRay, out ghostBallRay, out hit);
        direction = playerBallRay.direction;
        
        if (hitBall && hit.collider.GetComponent<Ball>())
        {
            //убрать это потом:
            Debug.DrawLine(playerBallRay.origin, hit.point, Color.red);
            Debug.DrawLine(ghostBallRay.origin, ghostBallRay.origin + 3 * ghostBallRay.direction);
            Debug.DrawLine(hit.point, ghostBallRay.origin - 3 * hit.normal, Color.black);

            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, playerBallRay.origin);
            lineRenderer.SetPosition(2, hit.point);
            lineRenderer.SetPosition(3, ghostBallRay.origin);
            lineRenderer.SetPosition(4, ghostBallRay.origin + 3 * ghostBallRay.direction);
            lineRenderer.SetPosition(5, hit.point);
            lineRenderer.SetPosition(6, ghostBallRay.origin - 3 * hit.normal);
        }
        else lineRenderer.enabled = false;
    }

    private bool Hit(Ray ray, out Ray deflected, out RaycastHit hit)
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
        ballBody.rotation = Quaternion.identity;
    }
}
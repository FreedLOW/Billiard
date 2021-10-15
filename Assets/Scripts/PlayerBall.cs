using UnityEngine;

public class PlayerBall : MonoBehaviour
{
    [SerializeField] Transform ghostBall;

    private const float Speed = 8f;
    private const int LenghtDrawLine = 4;

    private Rigidbody ballBody;
    private LineRenderer lineRenderer;

    private int minY = 32, maxY = 34;

    private Vector3 direction;
    private float forceSpeed;

    private bool hitBall;

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
        ballBody.velocity = direction * (Speed * force);
    }

    private void KickBall()
    {
        if (Input.touchCount > 0 && HUD.Instance.CanPlay)
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
                transform.LookAt(worldTouchPosition);
            }

            if (touch.phase == TouchPhase.Ended && hitBall)
            {
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
        Ray playerBallRay = new Ray(transform.position, -transform.forward);
        Ray ghostBallRay;
        RaycastHit hit;
        hitBall = Hit(playerBallRay, out ghostBallRay, out hit);
        direction = playerBallRay.direction;
        
        if (hitBall && hit.collider.GetComponent<Ball>())
        {
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, playerBallRay.origin);
            lineRenderer.SetPosition(2, hit.point);
            lineRenderer.SetPosition(3, ghostBallRay.origin);
            lineRenderer.SetPosition(4, ghostBallRay.origin + LenghtDrawLine * ghostBallRay.direction);
            lineRenderer.SetPosition(5, hit.point);
            lineRenderer.SetPosition(6, ghostBallRay.origin - LenghtDrawLine * hit.normal);
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
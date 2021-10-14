using System.Collections;
using UnityEngine;

public class StickController : MonoBehaviour
{
    [SerializeField] private Transform ball;

    private Vector3 originPosition;
    private Quaternion originRotation;

    private void Start()
    {
        originPosition = transform.position;
        originRotation = transform.rotation;
    }

    private void Update()
    {
        GetDirection();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerBall>())
        {
            print("here ball");
        }
    }

    private void GetDirection()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            var touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (touch.phase == TouchPhase.Began)
            {
                ball.GetComponent<PlayerBall>().ResetValues();
            }

            if (touch.phase == TouchPhase.Moved)
            {
                transform.position = new Vector3(touchPosition.x,
                                                transform.position.y,
                                                touchPosition.z);

                var yRot = ball.eulerAngles.y;
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, yRot, transform.eulerAngles.z);
            }

            if (touch.phase == TouchPhase.Ended)
            {
                StartCoroutine(KickAndIdleRoutine());
            }
        }
    }

    IEnumerator KickAndIdleRoutine()
    {
        transform.position = Vector3.Lerp(transform.position, ball.position, 1f);
        yield return new WaitForSeconds(0.5f);
        transform.position = originPosition;
        transform.rotation = originRotation;
    }
}
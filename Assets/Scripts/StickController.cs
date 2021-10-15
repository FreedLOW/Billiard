using System.Collections;
using UnityEngine;

public class StickController : MonoBehaviour
{
    [SerializeField] private Transform ball;
    
    private const float TimePlayAnim = 1f;

    private Vector3 originPosition;
    private Quaternion originRotation;

    private void Start()
    {
        originPosition = transform.position;
        originRotation = transform.rotation;
    }

    private void FixedUpdate()
    {
        GetDirection();
    }

    private void GetDirection()
    {
        if (Input.touchCount > 0 && HUD.Instance.CanPlay)
        {
            Touch touch = Input.GetTouch(0);
            var touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (touch.phase == TouchPhase.Began)
            {
                ball.GetComponent<PlayerBall>().ResetValues();
            }

            if (touch.phase == TouchPhase.Moved)
            {
                transform.position = new Vector3(touchPosition.x, transform.position.y, touchPosition.z);
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
        transform.position = Vector3.Lerp(transform.position, ball.position, TimePlayAnim);
        yield return new WaitForSeconds(TimePlayAnim);
        transform.position = originPosition;
        transform.rotation = originRotation;
    }
}
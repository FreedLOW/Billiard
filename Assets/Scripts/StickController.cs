using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickController : MonoBehaviour
{
    [SerializeField] private PlayerBall playerBall;
    [SerializeField] float followSpeed = 2f;

    private Vector3 direction;

    private float distanceForce;
    private float distance;

    private void Start()
    {
        distance = Mathf.Clamp(-3 / 2, -3, 0);
    }

    private void Update()
    {
        GetDirection();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<PlayerBall>())
        {
        }
    }

    private void GetDirection()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            //if (touch.phase == TouchPhase.Moved)
            //{
            //    transform.position = new Vector3(transform.position.x + touch.deltaPosition.x * followSpeed,
            //                                    transform.position.y,
            //                                    transform.position.z + touch.deltaPosition.y * followSpeed);
            //    transform.LookAt(playerBall.transform);
            //}
        }
    }
}
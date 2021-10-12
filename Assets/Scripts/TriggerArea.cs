using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerArea : MonoBehaviour
{
    [SerializeField] private Transform playerStartPoint;

    private const int Score = 1;

    private PlayerBall playerBall;
    private Ball ball;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Ball>())
        {
            ball = other.GetComponent<Ball>();
            print("other ball");
            //уничтожать этот мяч
            //и мож юи очков сделать
            HUD.Instance.UpdateScore(Score);
        }

        if (other.GetComponent<PlayerBall>())
        {
            playerBall = other.GetComponent<PlayerBall>();

            print("player ball");
            //если игрок попал в лузу, то возвращать его в точку спавна

            HUD.Instance.UpdateScore(-Score);
            //сбрасывать всю скорость через метод

            playerBall.ResetValues();
            playerBall.transform.position = playerStartPoint.position;
        }
    }
}
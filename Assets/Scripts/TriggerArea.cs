using UnityEngine;

public class TriggerArea : MonoBehaviour
{
    [SerializeField] private Transform playerStartPoint;

    private const int Score = 1;

    private PlayerBall playerBall;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Ball>())
        {
            HUD.Instance.UpdateScore(Score);
            HUD.Instance.AddToListBall(other.gameObject);
            HUD.Instance.CheckWin();
        }

        if (other.GetComponent<PlayerBall>())
        {
            playerBall = other.GetComponent<PlayerBall>();
            HUD.Instance.UpdateScore(-Score);
            playerBall.ResetValues();
            playerBall.transform.position = playerStartPoint.position;
        }
    }
}
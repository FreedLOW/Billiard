using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class HUD : MonoBehaviour
{
    private static HUD instance;

    [SerializeField] private List<GameObject> balls = new List<GameObject>();

    [SerializeField] private GameObject winPanel;

    [SerializeField] private TMP_Text scoreText;
    private int score = 0;

    private const int countToWin = 15;

    public static HUD Instance { get => instance; set => instance = value; }
    public bool CanPlay { get; set; } = false;

    private void Awake()
    {
        if (instance == null)
            instance = this;

        scoreText.text = "Score: " + score;
    }

    public void UpdateScore(int addScore)
    {
        score += addScore;
        scoreText.text = "Score: " + score;
    }

    public void StartPlay()
    {
        CanPlay = true;
    }

    public void CheckWin()
    {
        if (balls.Count == countToWin && balls.Count > 0)
        {
            winPanel.SetActive(true);
            CanPlay = false;
        }
        else return;
    }

    public void AddToListBall(GameObject ball)
    {
        balls.Add(ball);
    }
}
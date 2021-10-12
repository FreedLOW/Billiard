using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUD : MonoBehaviour
{
    private static HUD instance;

    [SerializeField] private TMP_Text scoreText;
    private int score = 0;

    public static HUD Instance { get => instance; set => instance = value; }

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
}
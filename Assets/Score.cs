using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    public TMP_Text scoreText; // Thành phần TMP_Text để hiển thị điểm

    void Start()
    {
        // Lấy điểm từ PlayerPrefs
        int score = PlayerPrefs.GetInt("Score", 0);

        // Hiển thị điểm trên TextMeshPro
        scoreText.text = "Your score: " + score.ToString();
    }
}

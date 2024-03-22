using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreDisplay : MonoBehaviour
{
    public TMP_Text scoreText; // Thành phần TMP_Text để hiển thị điểm

    void Start()
    {
        // Lấy điểm từ PlayerPrefs
        int newScore = PlayerPrefs.GetInt("Score", 0);

        // Lưu điểm vào danh sách và lưu lại danh sách
        SaveScore(newScore);

        // Hiển thị top 3 điểm trên TextMeshPro
        DisplayTopScores();
    }

    void SaveScore(int newScore)
    {
        List<int> scores = new List<int>();

        // Lấy danh sách điểm từ PlayerPrefs
        string scoreString = PlayerPrefs.GetString("Scores", "");

        // Nếu có điểm được lưu trữ, chuyển đổi thành List<int>
        if (!string.IsNullOrEmpty(scoreString))
        {
            scores = new List<int>(Array.ConvertAll(scoreString.Split(','), int.Parse));
        }

        // Thêm điểm mới vào danh sách
        scores.Add(newScore);

        // Sắp xếp danh sách điểm theo thứ tự giảm dần
        scores.Sort((a, b) => b.CompareTo(a));

        // Chỉ giữ lại 3 điểm cao nhất
        if (scores.Count > 3)
        {
            scores = scores.GetRange(0, 3);
        }

        // Lưu lại danh sách điểm vào PlayerPrefs
        PlayerPrefs.SetString("Scores", string.Join(",", scores));
        PlayerPrefs.Save();
    }

    void DisplayTopScores()
    {
        // Lấy danh sách điểm từ PlayerPrefs
        string scoreString = PlayerPrefs.GetString("Scores", "");
        List<int> scores = new List<int>(Array.ConvertAll(scoreString.Split(','), int.Parse));

        // Hiển thị top 3 điểm trên TextMeshPro
        scoreText.text = "";

        // Hiển thị chỉ 3 điểm đầu tiên
        for (int i = 0; i < Mathf.Min(scores.Count, 3); i++)
        {
            scoreText.text += (i + 1) + ". " + scores[i] + "\n";
        }
    }


}

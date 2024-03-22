using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public TMP_Text scoreText; // Thành phần TMP_Text để hiển thị điểm
    public int score = 0; // Điểm hiện tại
    private float elapsedTime = 0f; // Thời gian đã trôi qua
    private float scoreTimer = 0f; // Thời gian đã trôi qua từ lần cộng điểm trước

    void Update()
    {
        // Tăng thời gian đã trôi qua mỗi frame
        elapsedTime += Time.deltaTime;
        scoreTimer += Time.deltaTime;

        // Kiểm tra nếu đã đủ 10 giây
        if (scoreTimer >= 5f)
        {
            // Cộng điểm
            score += 20;
            Debug.Log("Score: " + score);

            // Cập nhật giá trị của thành phần TMP_Text với điểm số mới
            scoreText.text = "Score: " + score;

            // Đặt lại thời gian đã trôi qua từ lần cộng điểm trước
            scoreTimer = 0f;
        }
    }
}


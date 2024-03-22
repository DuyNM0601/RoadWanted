using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelTrail : MonoBehaviour
{
    TrailRenderer trailRenderer;

    void Start()
    {
        trailRenderer = GetComponent<TrailRenderer>();
        trailRenderer.emitting = false; // Tắt trail renderer khi bắt đầu
    }

    void Update()
    {
        // Lấy tham chiếu đến PlayerController
        PlayerController playerController = GetComponentInParent<PlayerController>();

        // Kiểm tra xem người chơi có đang cua không, nếu có thì bật trail renderer
        if (playerController != null && playerController.isTurning)
        {
            trailRenderer.emitting = true;
        }
        else
        {
            trailRenderer.emitting = false;
        }
    }
}

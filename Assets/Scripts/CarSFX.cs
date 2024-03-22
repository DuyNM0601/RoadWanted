using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSFX : MonoBehaviour
{
    [Header("Audio sources")]
    public AudioSource driftAudio;
    public AudioSource engineAudio;
    public AudioSource carHit;

    float desiredEnginePitch = 0.5f;
    float driftPitch = 0.5f;

    PlayerController playerController;

    void Awake()
    {
        playerController = GetComponentInParent<PlayerController>();
    }

    void Start()
    {
        
        engineAudio.Play();
        engineAudio.loop = true;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateEngineSFX();
        UpdateDrift();
    }

    void UpdateEngineSFX()
    {
        // Lấy vận tốc của xe từ PlayerController
        float velocity = playerController.GetVelocity();

        float desiredVolume = velocity * 0.05f;
        desiredVolume = Mathf.Clamp(desiredVolume, 0.2f, 1.0f);

        // Tăng giảm âm lượng của âm thanh động cơ dựa trên vận tốc
        engineAudio.volume = Mathf.Lerp(engineAudio.volume, desiredVolume, Time.deltaTime * 10);

        desiredEnginePitch = velocity * 0.2f;
        desiredEnginePitch = Mathf.Clamp(desiredEnginePitch, 0.5f, 2f);
        engineAudio.pitch = Mathf.Lerp(engineAudio.pitch, desiredEnginePitch, Time.deltaTime * 1.5f);
    }

    void UpdateDrift()
    {
        if (playerController != null && playerController.isTurning)
        {
            // Khi đang drift, tăng âm lượng và giảm pitch của âm thanh drift
            if (!driftAudio.isPlaying) // Kiểm tra xem âm thanh drift đã đang phát hay chưa
            {
                driftAudio.Play(); // Nếu chưa, bắt đầu phát
            }
            driftAudio.volume = Mathf.Lerp(driftAudio.volume, 1.0f, Time.deltaTime * 10);
            driftPitch = Mathf.Lerp(driftPitch, 0.5f, Time.deltaTime * 10);
            driftAudio.pitch = driftPitch;
        }
        else
        {
            // Khi không drift, giảm âm lượng của âm thanh drift về 0
            driftAudio.volume = Mathf.Lerp(driftAudio.volume, 0, Time.deltaTime * 10);
            if (driftAudio.isPlaying) // Kiểm tra xem âm thanh drift đang phát hay không
            {
                driftAudio.Stop(); // Nếu đang phát, dừng lại
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Khi xe va chạm, phát âm thanh va chạm
        carHit.Play();
    }

}

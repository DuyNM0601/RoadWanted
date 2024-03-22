using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelSmoke : MonoBehaviour
{
    float particleEmmissionRate = 0;

    PlayerController controller;

    ParticleSystem myParticleSystem;
    ParticleSystem.EmissionModule EmissionModule;

    bool hasChangedSprite = false; // Biến để kiểm tra xem sprite đã được thay đổi chưa

    void Awake()
    {
        controller = GetComponentInParent<PlayerController>();

        myParticleSystem = GetComponent<ParticleSystem>();

        EmissionModule = myParticleSystem.emission;

        EmissionModule.rateOverTime = 0;
    }
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    

    void Update()
    {

        // Nếu sprite chưa được thay đổi, tiếp tục kiểm tra xem người chơi có đang cua không
        particleEmmissionRate = Mathf.Lerp(particleEmmissionRate, 0, Time.deltaTime * 5);
        EmissionModule.rateOverTime = particleEmmissionRate;

        PlayerController playerController = GetComponentInParent<PlayerController>();

        if (playerController != null && playerController.isTurning)
        {
            particleEmmissionRate = 30;
        }
    }



}

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CarDestroy : MonoBehaviour
{
    ParticleSystem explosionEffect; // Hiệu ứng nổ xe
    public GameObject smokeEffect; // GameObject chứa hệ thống hạt của khói
    public float explosionDuration = 0f; // Thời gian kéo dài của hiệu ứng nổ trước khi xe biến mất
    ParticleSystem.EmissionModule emissionModule;
    public ChangeScence changeScence;

    // Tham chiếu đến script ChangeScene
   /* public ChangeScence changeSceneScript;*/

    

    public void ActivateSmokeAndExplode()
    {
        
        StartCoroutine(ExplodeCar());
        changeScence.ActivateSceneChange();
    }

    IEnumerator ExplodeCar()
    {
        // Đợi một khoảng thời gian trước khi phá hủy xe
        yield return new WaitForSeconds(explosionDuration);

        // Ẩn xe
        gameObject.SetActive(false);

        // Kích hoạt hệ thống hạt của khói
        if (smokeEffect != null)
        {
            smokeEffect.SetActive(true);
        }
    }
/*
    // Update is called once per frame
    void Update()
    {
        // Kiểm tra xem hiệu ứng khói đã được kích hoạt và đã đợi đủ thời gian chưa
        if (smokeActivated && changeSceneScript != null)
        {
            changeSceneScript.ActivateSceneChange(); // Kích hoạt việc chuyển scene
            smokeActivated = false; // Đặt cờ trở lại false để tránh việc chuyển scene nhiều lần
        }
    }*/
}

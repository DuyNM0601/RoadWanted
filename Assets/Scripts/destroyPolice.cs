using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroyPolice : MonoBehaviour
{
   
    public Sprite newSprite; // Sprite mới sau khi va chạm với Untagged

    public float explosionDuration = 10f; // Thời gian kéo dài của hiệu ứng nổ trước khi xe biến mất
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Untagged"))
        {
            // Thay đổi sprite của xe
            GetComponent<SpriteRenderer>().sprite = newSprite;

            // Bắt đầu Coroutine để đợi một khoảng thời gian rồi sau đó tiến hành xóa đối tượng xe
            StartCoroutine(DestroyCar());
        }
    }

    IEnumerator DestroyCar()
    {
        // Chờ đợi cho đến khi thời gian explosionDuration kết thúc
        yield return new WaitForSeconds(explosionDuration);

        // Tắt đối tượng xe
        Destroy(gameObject);
    }

}

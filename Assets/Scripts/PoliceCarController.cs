using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poll : MonoBehaviour
{
    

    [SerializeField] float moveSpeed;
    [SerializeField] float maxSpeed; // Giới hạn tốc độ tối đa
    [SerializeField] float acceleration; // Độ gia tốc

    Coroutine speedIncreaseCoroutine;

    private Transform playerTransform;
    private Rigidbody2D rb;
    private bool isColliding;

    void Start()
    {
        // Lấy tham chiếu đến transform của người chơi
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        
    }

    void Update()
    {
        // Nếu không có va chạm xảy ra, di chuyển xe ra xa người chơi
        MoveTowardsPlayer();
    }

    

    void MoveTowardsPlayer()
    {
        // Tính toán hướng di chuyển
        Vector3 direction = (playerTransform.position - transform.position).normalized;

        // Tính toán góc quay để hướng xe cảnh sát hướng về người chơi
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);

        // Sử dụng lerp để trơn tru hóa quá trình quay của xe cảnh sát
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * moveSpeed);

        // Tính toán lực tăng tốc độ
        float currentSpeed = rb.velocity.magnitude;
        if (currentSpeed < maxSpeed)
        {
            // Áp dụng lực tăng tốc độ
            rb.AddForce(transform.up * acceleration);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Áp dụng lực để hất văng xe của bạn khi va chạm với xe cảnh sát
            Vector2 direction = (transform.position - collision.transform.position).normalized;
            rb.AddForce(direction * 10f, ForceMode2D.Impulse);

            // Kiểm tra nếu moveSpeed chưa đạt tới 20f, thì mới giảm
            if (moveSpeed > 20f)
            {
                moveSpeed -= 5f; // Giảm tốc độ điều chỉnh theo nhu cầu của bạn
            }

            // Giảm lực hất văng của xe cảnh sát
            rb.AddForce(rb.velocity.normalized * -10f, ForceMode2D.Impulse);

            // Bắt đầu coroutine để tăng tốc độ lên lại sau một khoảng thời gian
            if (speedIncreaseCoroutine != null)
                StopCoroutine(speedIncreaseCoroutine);
            speedIncreaseCoroutine = StartCoroutine(IncreaseSpeedAfterDelay(1f));
        }
    }
    IEnumerator IncreaseSpeedAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        while (!isColliding && moveSpeed <= maxSpeed)
        {
            moveSpeed += 1f; // Tăng tốc độ điều chỉnh theo nhu cầu của bạn
            yield return new WaitForSeconds(0.1f); // Khoảng thời gian tăng tốc độ mỗi lần
        }
    }


}

using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float rotateSpeed;
    [SerializeField] float maxSpeed;
    [SerializeField] public Sprite carSmoke;
    Rigidbody2D rb;
    Coroutine speedIncreaseCoroutine;
    [SerializeField] RandomPackage randomPackageScript;

    public TMP_Text carCountText;
    public Image fillImage;
    public int carCount = 100;

    bool hasChangedSprite = false;


    bool isColliding = false;

    // Khai báo các biến accelerationInput và velocityVsUp ở đây
    float accelerationInput;
    float velocityVsUp;
    public bool isTurning = false;

    [SerializeField] int collisionCountThreshold = 3; // Ngưỡng số lần va chạm để thực hiện hành động
    int collisionCount = 0;
    int isCarDestroy = 5;
    bool destroy = false;

    public TMP_Text scoreText; // Thành phần TMP_Text để hiển thị điểm
    public int score = 0; // Điểm hiện tại
    private float elapsedTime = 0f; // Thời gian đã trôi qua
    private float scoreTimer = 0f; // Thời gian đã trôi qua từ lần cộng điểm trước
    private bool carExploded = false;
    private float explosionDelay = 3f;


    TopDownController controller;

    void Awake()
    {
        controller= GetComponent<TopDownController>();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

    }

    void Update()
    {
        Vector2 inputVector = Vector2.zero;
        // Lấy input từ bàn phím
        inputVector.x = Input.GetAxis("Horizontal");
        inputVector.y = Input.GetAxis("Vertical");
        controller.SetInputVector(inputVector);
        
        
        isTurning = Mathf.Abs(inputVector.x) > 0f;

        if (!carExploded)
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Money"))
        {
            score += 10;
            scoreText.text = "Score: " + score;
            Debug.Log("Package picked up");

            // Tăng số lượng món hàng lên 1
            // Hủy đối tượng package khi nó được chạm vào
            Destroy(other.gameObject);

            if (randomPackageScript != null)
            {
                // Gọi hàm SpawnPackage từ script RandomPackage
                StartCoroutine(SpawnSecondPackageAfterDelay(1f));
            }
            else
            {
                Debug.LogWarning("RandomPacage script is missing or has been destroyed.");
            }

        }
    }

    IEnumerator SpawnSecondPackageAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // Đợi 5 giây

        // Gọi hàm SpawnNewPackage từ script RandomPackage
        randomPackageScript.SpawnNewPackage();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PoliceCar"))
        {
            // Tăng biến đếm số lần va chạm
            collisionCount++;
            if (carCount > 0)
            {
                carCount -= 2;
                fillImage.fillAmount -= 0.02f;
                fillImage.fillAmount = Mathf.Max(fillImage.fillAmount, 0f);
            }

            carCountText.text = carCount.ToString() + "/100";
            // Nếu số lần va chạm vượt quá ngưỡng và chưa thay đổi sprite
            if (carCount <= 50)
            {
                // Thực hiện hành động thay đổi sprite
                GetComponent<SpriteRenderer>().sprite = carSmoke;
                collisionCountThreshold++;
                hasChangedSprite = true;
            }

            CarSmoke wheelSmoke = GetComponentInChildren<CarSmoke>();
            if (wheelSmoke != null && hasChangedSprite) // Kiểm tra đã thay đổi Sprite
            {
                wheelSmoke.ActivateSmoke();
            }
            CarDestroy carDestroy = GetComponentInChildren<CarDestroy>();
            // Nếu đã thay đổi sprite và vượt quá số lần va chạm để phá hủy
            if (carCount == 0)
            {
                carExploded = true;
                // Thực hiện hành động phá hủy
                PlayerPrefs.SetInt("Score", score);
                PlayerPrefs.Save();

                // Chuyển scene sang scene mới với tên "NewScene"
                
                carDestroy.ActivateSmokeAndExplode();
                
                // Dừng hàm OnCollisionEnter2D ở đây
                
            }
        }


    }

    

    IEnumerator WaitAndLoadScene(int sceneName, float delay)
    {
        // Đợi một khoảng thời gian
        yield return new WaitForSeconds(delay);

        // Chuyển Scene sang Scene mới với tên được chỉ định
        
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


    

    float GetLateralVelocity()
    {
        return Vector2.Dot(transform.right, rb.velocity);
    }

    public bool IsTireScreeching(out float lateralVelocity, out bool isBraking)
    {
        lateralVelocity = GetLateralVelocity();

        isBraking = false;

        // Sử dụng accelerationInput và velocityVsUp ở đây
        if (accelerationInput < 0 && velocityVsUp > 0)
        {
            isBraking = true;
            return true;
        }

        if (Mathf.Abs(GetLateralVelocity()) > 4.0f)
        {
            return true;
        }
        return false;
    }

    public float GetVelocity()
    {
        return rb.velocity.magnitude;
    }
    public float GetMaxSpeed()
    {
        return maxSpeed;
    }
}

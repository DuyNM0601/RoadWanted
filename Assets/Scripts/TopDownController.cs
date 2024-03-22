using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownController : MonoBehaviour
{
    public float accelerationFactor = 30f;
    public float turnFactor = 3.5f;
    public float driftFactor = 0;
    [SerializeField] RandomPackage randomPackageScript;
    float accelerationInput = 0;
    float steeringInput = 0;

    float rotationAngle = 0;

    bool isSpeed = false;
    private float initialAccelerationFactor;

    Rigidbody2D carRigidbody;

    void Awake()
    {
        carRigidbody = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        initialAccelerationFactor = accelerationFactor;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        ApplyEngineForce();
        KillVelocity();
        ApplySteering();
    }

    void ApplyEngineForce()
    {
        Vector2 engineForceVector = transform.up * accelerationInput * accelerationFactor;

        carRigidbody.AddForce(engineForceVector, ForceMode2D.Force);
    }

    void ApplySteering()
    {
        rotationAngle -= steeringInput * turnFactor;

        carRigidbody.MoveRotation(rotationAngle);
    }

    void KillVelocity()
    {
        Vector2 forwardVelocity = transform.up * Vector2.Dot(carRigidbody.velocity, transform.up);
        Vector2 rightVelocity = transform.right * Vector2.Dot(carRigidbody.velocity, transform.right);

        carRigidbody.velocity = forwardVelocity + rightVelocity * driftFactor;
    }

    public void SetInputVector(Vector2 inputVector)
    {
        steeringInput = inputVector.x;
        accelerationInput = inputVector.y;
    }


    void SpeedUp()
    {
        if (!isSpeed)
        {
            isSpeed = true;
            accelerationFactor += 20; // Tăng tốc độ
            StartCoroutine(ResetSpeedAfterDelay(5f)); // Gọi coroutine để đặt lại tốc độ sau 5 giây
        }
    }

    void Slow()
    {
        if (!isSpeed)
        {
            isSpeed = true;
            accelerationFactor -= 50; // Tăng tốc độ
            StartCoroutine(ResetSpeedAfterDelay(2f)); // Gọi coroutine để đặt lại tốc độ sau 5 giây
        }
    }

    IEnumerator ResetSpeedAfterDelay(float delay)
    {
        // Đợi một khoảng thời gian
        yield return new WaitForSeconds(delay);

        // Đặt lại tốc độ về giá trị ban đầu
        accelerationFactor = initialAccelerationFactor;

        // Đặt biến isSpeed về false để có thể kích hoạt tăng tốc độ lần tiếp theo
        isSpeed = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("SpeedUp"))
        {
            SpeedUp();
            Debug.Log("Package picked up");

            // Tăng số lượng món hàng lên 1
            // Hủy đối tượng package khi nó được chạm vào
            Destroy(other.gameObject);

            // Kiểm tra xem randomPackageScript có tồn tại không trước khi gọi hàm SpawnPackage
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

        if (other.CompareTag("Slow"))
        {
            Slow();
            Debug.Log("Package picked up");

            
        }


    }

    



    IEnumerator SpawnSecondPackageAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // Đợi 5 giây

        // Gọi hàm SpawnNewPackage từ script RandomPackage
        randomPackageScript.SpawnNewPackage();
    }

}

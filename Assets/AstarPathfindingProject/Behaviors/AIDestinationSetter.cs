using UnityEngine;
using System.Collections;

namespace Pathfinding {
	/// <summary>
	/// Sets the destination of an AI to the position of a specified object.
	/// This component should be attached to a GameObject together with a movement script such as AIPath, RichAI or AILerp.
	/// This component will then make the AI move towards the <see cref="target"/> set on this component.
	///
	/// See: <see cref="Pathfinding.IAstarAI.destination"/>
	///
	/// [Open online documentation to see images]
	/// </summary>
	[UniqueComponent(tag = "ai.destination")]
	[HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_a_i_destination_setter.php")]
	public class AIDestinationSetter : VersionedMonoBehaviour {
		/// <summary>The object that the AI should move to</summary>
		public Transform target;
        IAstarAI ai;
        [SerializeField] float moveSpeed;
        [SerializeField] float maxSpeed; 
        [SerializeField] float acceleration; 

        Coroutine speedIncreaseCoroutine;

        private Rigidbody2D rb;
        private bool isColliding;

        void OnEnable () {
            ai = GetComponent<IAstarAI>();
            if (ai != null) ai.onSearchPath += Update;
        }

        void OnDisable () {
            if (ai != null) ai.onSearchPath -= Update;
        }

        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        void Update () {
            if (target != null && ai != null) ai.destination = target.position;
            MoveTowardsDestination();
        }

        void MoveTowardsDestination()
        {
            // Kiểm tra nếu đang đến gần điểm đích, không thực hiện gì cả
            if (ai.reachedDestination)
                return;

            // Tính toán hướng di chuyển dựa trên hướng của velocity của AI
            Vector3 direction = ai.velocity.normalized;

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
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PollicSpawner : MonoBehaviour
{
    public GameObject firstPoliceCar; // Game object của chiếc xe cảnh sát thứ nhất
    public GameObject secondPoliceCar; // Game object của chiếc xe cảnh sát thứ hai
    public GameObject thirdPoliceCar;
    public float firstSpawnInterval = 5f; // Khoảng thời gian giữa lần spawn đầu tiên
    public float secondSpawnInterval = 5f; // Khoảng thời gian giữa các lần spawn tiếp theo cho xe thứ hai
    public int firstPoliceCountThreshold = 3; // Số lượng xe cảnh sát thứ nhất cần đạt tới trước khi spawn xe thứ hai
    public int secondPoliceCountThreshold = 3;
    private int spawnedFirstPoliceCount = 0;
    private int spawnedSecondPoliceCount = 0;
    private GameObject policeCarsContainer; // Container chứa cả hai chiếc xe cảnh sát

    void Start()
    {
        // Tìm container "PoliceCarsContainer" trong hierarchy
        policeCarsContainer = GameObject.Find("PoliceCarsContainer");

        // Ẩn container khi bắt đầu game
        policeCarsContainer.SetActive(false);

        // Bắt đầu coroutine để sinh ra các xe cảnh sát
        StartCoroutine(SpawnPoliceCars());
    }

    IEnumerator SpawnPoliceCars()
    {
        // Đợi một khoảng thời gian trước khi spawn xe thứ nhất
        yield return new WaitForSeconds(firstSpawnInterval);

        // Spawn các xe cảnh sát thứ nhất
        while (true)
        {
            if (spawnedFirstPoliceCount < firstPoliceCountThreshold)
            {
                Instantiate(firstPoliceCar, transform.position, Quaternion.identity);
                spawnedFirstPoliceCount++;
            }
            else if (spawnedSecondPoliceCount < secondPoliceCountThreshold)
            {
                Instantiate(secondPoliceCar, transform.position, Quaternion.identity);
                spawnedSecondPoliceCount++;
            }
            else
            {
                Instantiate(thirdPoliceCar, transform.position, Quaternion.identity);
            }

            // Đợi một khoảng thời gian trước khi spawn xe tiếp theo
            yield return new WaitForSeconds(secondSpawnInterval);
        }
    }
}

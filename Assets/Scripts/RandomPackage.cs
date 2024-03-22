using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPackage : MonoBehaviour
{
    public GameObject package;
    public List<Vector3> spawnPoints; // Danh sách các điểm xuất hiện có thể của package

    public void SpawnNewPackage()
    {
        // Kiểm tra xem có ít nhất một điểm xuất hiện trong danh sách không
        if (spawnPoints.Count > 0)
        {
            // Chọn một điểm xuất hiện ngẫu nhiên từ danh sách
            Vector3 randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];

            // Tạo package tại điểm xuất hiện ngẫu nhiên được chọn
            Instantiate(package, randomSpawnPoint, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("No spawn points available for the package.");
        }
    }
}

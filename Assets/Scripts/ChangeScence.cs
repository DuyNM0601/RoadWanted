using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScence : MonoBehaviour
{
    private bool shouldChangeScene = false; // Biến đánh dấu xem có cần chuyển scene hay không

    // Hàm này được gọi khi cần thay đổi scene
    public void ActivateSceneChange()
    {
        shouldChangeScene = true;
         // Chờ 3 giây trước khi thực hiện chuyển scene
    }

    public void Waiting()
    {
        StartCoroutine(WaitAndChangeScene(5f));
    }

    IEnumerator WaitAndChangeScene(float delay)
    {
        // Đợi một khoảng thời gian
        yield return new WaitForSeconds(delay);

        // Thực hiện thay đổi scene
        SceneManager.LoadScene(2);
    }

    void Update()
    {
        // Kiểm tra xem có cần thay đổi scene không
        if (shouldChangeScene)
        {
            Waiting(); // Gọi hàm ActivateSceneChange khi shouldChangeScene được đặt thành true
            shouldChangeScene = false; // Đặt lại shouldChangeScene về false sau khi gọi hàm
        }
    }

}

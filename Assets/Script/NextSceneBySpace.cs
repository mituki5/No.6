using UnityEngine;
using UnityEngine.SceneManagement;

public class NextSceneBySpace : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene("TitleScene");
        }
    }
}

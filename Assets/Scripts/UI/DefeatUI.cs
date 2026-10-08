using UnityEngine;
using UnityEngine.SceneManagement;

public class DefeatUI : MonoBehaviour
{
    [SerializeField] private GameObject defeatPanel;

    public void ShowDefeat()
    {
        defeatPanel.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;

        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
}

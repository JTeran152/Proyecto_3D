using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private string gameSceneName = "SampleScene";

    public void SelectEasy()
    {
        DifficultyManager.Instance.SetEasy();
        LoadGame();
    }

    public void SelectNormal()
    {
        DifficultyManager.Instance.SetNormal();
        LoadGame();
    }

    public void SelectHard()
    {
        DifficultyManager.Instance.SetHard();
        LoadGame();
    }

    private void LoadGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }
}
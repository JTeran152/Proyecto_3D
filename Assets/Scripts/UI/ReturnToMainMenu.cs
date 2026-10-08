
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToMainMenu : MonoBehaviour
{
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    public void GoToMainMenu()
    {
        // Reactivar el tiempo por si la partida terminó pausada.
        Time.timeScale = 1f;

        // Liberar el cursor para la interfaz del menú.
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Cargar la escena del menú principal.
        SceneManager.LoadScene(mainMenuSceneName);
    }
}

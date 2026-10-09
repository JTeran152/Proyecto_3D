
using UnityEngine;

public class ExitGame : MonoBehaviour
{
    public void QuitGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        Debug.Log("Salir del juego: en una compilación se cerrará la aplicación.");
#endif
    }
}

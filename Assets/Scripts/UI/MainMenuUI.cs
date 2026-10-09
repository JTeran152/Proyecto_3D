
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    [Header("Escena del juego")]
    [SerializeField] private string gameSceneName = "SampleScene";

    [Header("Sonidos de los botones")]
    [SerializeField] private AudioSource uiAudioSource;
    [SerializeField] private AudioClip easyClickSound;
    [SerializeField] private AudioClip normalClickSound;
    [SerializeField] private AudioClip hardClickSound;
    [SerializeField] private AudioClip exitClickSound;

    [Header("Transicion")]
    [SerializeField] private CanvasGroup fadeCanvasGroup;
    [SerializeField] private float transitionDuration = 0.5f;
    [SerializeField] private float actionDelay = 0.8f;

    private bool isLoading;

    public void SelectEasy()
    {
        if (isLoading) return;

        isLoading = true;
        DifficultyManager.Instance.SetEasy();
        StartCoroutine(LoadGameAfterSound(easyClickSound));
    }

    public void SelectNormal()
    {
        if (isLoading) return;

        isLoading = true;
        DifficultyManager.Instance.SetNormal();
        StartCoroutine(LoadGameAfterSound(normalClickSound));
    }

    public void SelectHard()
    {
        if (isLoading) return;

        isLoading = true;
        DifficultyManager.Instance.SetHard();
        StartCoroutine(LoadGameAfterSound(hardClickSound));
    }

    public void ExitGame()
    {
        if (isLoading) return;

        isLoading = true;
        StartCoroutine(ExitAfterSound());
    }

    private IEnumerator LoadGameAfterSound(AudioClip clickSound)
    {
        PlayClickSound(clickSound);

        yield return StartCoroutine(FadeToBlack());

        float remainingDelay =
            Mathf.Max(0f, actionDelay - transitionDuration);

        if (remainingDelay > 0f)
            yield return new WaitForSecondsRealtime(remainingDelay);

        SceneManager.LoadScene(gameSceneName);
    }

    private IEnumerator ExitAfterSound()
    {
        PlayClickSound(exitClickSound);

        yield return StartCoroutine(FadeToBlack());

        float remainingDelay =
            Mathf.Max(0f, actionDelay - transitionDuration);

        if (remainingDelay > 0f)
            yield return new WaitForSecondsRealtime(remainingDelay);

        Application.Quit();

#if UNITY_EDITOR
        Debug.Log("Salir del juego: prueba el cierre en una compilacion.");
#endif
    }

    private IEnumerator FadeToBlack()
    {
        if (fadeCanvasGroup == null)
        {
            yield return new WaitForSecondsRealtime(transitionDuration);
            yield break;
        }

        fadeCanvasGroup.blocksRaycasts = true;
        fadeCanvasGroup.interactable = false;
        fadeCanvasGroup.alpha = 0f;

        float elapsed = 0f;

        while (elapsed < transitionDuration)
        {
            elapsed += Time.unscaledDeltaTime;

            fadeCanvasGroup.alpha = Mathf.Clamp01(
                elapsed / transitionDuration
            );

            yield return null;
        }

        fadeCanvasGroup.alpha = 1f;
    }

    private void PlayClickSound(AudioClip clickSound)
    {
        if (uiAudioSource != null && clickSound != null)
        {
            uiAudioSource.PlayOneShot(clickSound);
        }
    }
}
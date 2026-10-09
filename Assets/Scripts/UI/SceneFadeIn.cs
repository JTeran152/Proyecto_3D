
using System.Collections;
using UnityEngine;

public class SceneFadeIn : MonoBehaviour
{
    [SerializeField] private CanvasGroup fadeCanvasGroup;
    [SerializeField] private float fadeDuration = 1f;

    private IEnumerator Start()
    {
        if (fadeCanvasGroup == null)
        {
            Debug.LogWarning("SceneFadeIn: falta asignar el CanvasGroup.");
            yield break;
        }

        // La escena comienza completamente a oscuras.
        fadeCanvasGroup.alpha = 1f;
        fadeCanvasGroup.blocksRaycasts = true;

        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.unscaledDeltaTime;

            fadeCanvasGroup.alpha =
                1f - Mathf.Clamp01(elapsed / fadeDuration);

            yield return null;
        }

        fadeCanvasGroup.alpha = 0f;
        fadeCanvasGroup.blocksRaycasts = false;
    }
}
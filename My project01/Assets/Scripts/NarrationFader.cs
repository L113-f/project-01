using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NarrationFader : MonoBehaviour
{
    public TextMeshProUGUI text;
    public CanvasGroup canvasGroup;

    public float fadeInDuration = .5f;
    public float holdDuration = 2f;
    public float fadeOutDuration = .5f;

    private Coroutine currentRoutine;

    public void ShowLine(string line)
    {
        if (currentRoutine != null)
        {
            StopCoroutine(currentRoutine);
        }
        currentRoutine = StartCoroutine(DoShowLine(line));
    }

    private IEnumerator DoShowLine(string line)
    {
        if (text == null || canvasGroup == null)
        {
            yield break;
        }

        text.text = line;
        canvasGroup.alpha = 0f;

        float time = 0f;
        while (time < fadeInDuration)
        {
            time += Time.deltaTime;
            float k = Mathf.Clamp01(time / fadeInDuration);
            canvasGroup.alpha = k;
            yield return null;
        }
        canvasGroup.alpha = 1f;

 
        yield return new WaitForSeconds(holdDuration);

        time = 0f;
        while (time < fadeOutDuration)
        {
            time += Time.deltaTime;
            float k = Mathf.Clamp01(time / fadeOutDuration);
            canvasGroup.alpha = 1f - k;
            yield return null;
        }
        canvasGroup.alpha = 0f;
    }
}

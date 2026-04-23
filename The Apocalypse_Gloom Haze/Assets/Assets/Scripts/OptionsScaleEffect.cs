using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OptionsScaleEffect : MonoBehaviour
{
    public Button targetButton;        // Kéo thả nút Options vào đây
    public float buttonScaleEffect = 1.2f; // Tỉ lệ phóng to
    public float effectDuration = 0.25f;    // Thời gian hiệu ứng

    private Vector3 originalScale;

    void Start()
    {
        if (targetButton != null)
            originalScale = targetButton.transform.localScale;
    }

    // Gọi hàm này từ OnClick của Button
    public void PlayScaleEffect()
    {
        if (targetButton != null)
            StartCoroutine(ScaleEffect());
    }

    IEnumerator ScaleEffect()
    {
        Transform buttonTransform = targetButton.transform;
        float halfDuration = effectDuration / 2f;
        float timer = 0f;

        // Scale up
        while (timer < halfDuration)
        {
            timer += Time.deltaTime;
            float scale = Mathf.Lerp(1f, buttonScaleEffect, timer / halfDuration);
            buttonTransform.localScale = originalScale * scale;
            yield return null;
        }

        timer = 0f;

        // Scale back
        while (timer < halfDuration)
        {
            timer += Time.deltaTime;
            float scale = Mathf.Lerp(buttonScaleEffect, 1f, timer / halfDuration);
            buttonTransform.localScale = originalScale * scale;
            yield return null;
        }

        // Reset lại scale gốc
        buttonTransform.localScale = originalScale;
    }
}

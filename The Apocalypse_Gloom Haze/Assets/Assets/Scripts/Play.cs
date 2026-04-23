using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Play : MonoBehaviour
{
    [Header("Scene Transition")]
    public Image fadeImage;          // Kéo thả Image đen full màn hình (alpha = 0 ban đầu)
    public float fadeDuration = 1f;  // Thời gian fade

    [Header("Button")]
    public Button playButton;        // Kéo thả Button Play
    public float buttonScaleEffect = 1.2f;
    public float effectDuration = 0.3f;

    private Vector3 originalButtonScale;

    void Start()
    {
        if (playButton != null)
            originalButtonScale = playButton.transform.localScale;
    }

    // Hàm gọi từ OnClick
    public void LoadSceneByIndex(int sceneIndex)
    {
        StartCoroutine(ButtonEffectSequence(sceneIndex));
    }

    IEnumerator ButtonEffectSequence(int sceneIndex)
    {
        // 1. Hiệu ứng nút phóng to/thu nhỏ
        if (playButton != null)
            yield return StartCoroutine(ButtonScaleEffect());

        // 2. Bắt đầu fade đen và load scene
        yield return StartCoroutine(LoadSceneWithFade(sceneIndex));
    }

    IEnumerator ButtonScaleEffect()
    {
        Transform buttonTransform = playButton.transform;
        float halfDuration = effectDuration / 2f;
        float timer = 0f;

        // Scale up
        while (timer < halfDuration)
        {
            timer += Time.deltaTime;
            float scale = Mathf.Lerp(1f, buttonScaleEffect, timer / halfDuration);
            buttonTransform.localScale = originalButtonScale * scale;
            yield return null;
        }

        timer = 0f;

        // Scale back
        while (timer < halfDuration)
        {
            timer += Time.deltaTime;
            float scale = Mathf.Lerp(buttonScaleEffect, 1f, timer / halfDuration);
            buttonTransform.localScale = originalButtonScale * scale;
            yield return null;
        }

        buttonTransform.localScale = originalButtonScale;
    }

    IEnumerator LoadSceneWithFade(int sceneIndex)
    {
        if (fadeImage == null)
        {
            // Nếu chưa gán fadeImage thì load scene luôn
            SceneManager.LoadScene(sceneIndex);
            yield break;
        }

        float timer = 0f;
        Color color = fadeImage.color;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            color.a = Mathf.Lerp(0f, 1f, timer / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }

        // Đảm bảo alpha = 1
        color.a = 1f;
        fadeImage.color = color;

        SceneManager.LoadScene(sceneIndex);
    }
}

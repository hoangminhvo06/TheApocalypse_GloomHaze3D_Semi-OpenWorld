using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{

    // Khai báo 1 Game Object là Pause_Panel
    public GameObject pausePanel;

    // Hiện Option_Panel
    public GameObject optionPanel;

    // Khai báo CanvasGroup để fade mượt mà
    public CanvasGroup fadePanel;

    public float fadeSpeed = 0.2f;

    private bool isPaused = false;


    public void PauseGame()
    {
        isPaused = true;
        pausePanel.SetActive(true);
        optionPanel.SetActive(false);
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        isPaused = false;
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
    }


    public void ClosePanel()
    {
        isPaused = false;
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void OpenOption()// Mở Option_Panel
    {
        pausePanel.SetActive(false); // Ẩn pausePanel đi (false)
        optionPanel.SetActive(true); // Hiện optionPanel (true)
    }

    public void CloseOption() // Đóng Option_Panel
    {
        optionPanel.SetActive(false); // Ẩn optionPanel(false)
        pausePanel.SetActive(true); // Hiện pausePanel(true)
    }

    IEnumerator FadeIn() // Logic: Alpha từ 0 -> 1
    {
        // Đảm bảo alpha bắt đầu từ 0
        fadePanel.alpha = 0f;
        fadePanel.gameObject.SetActive(true);

        while (fadePanel.alpha < 1f)
        {
            fadePanel.alpha += Time.unscaledDeltaTime * fadeSpeed;
            yield return null;
        }

        fadePanel.alpha = 1f;
    }

    IEnumerator FadeOut() // Logic: Alpha từ 1 -> 0
    {
        // Đảm bảo alpha bắt đầu từ 1
        fadePanel.alpha = 1f;

        while (fadePanel.alpha > 0f)
        {
            fadePanel.alpha -= Time.unscaledDeltaTime * fadeSpeed;
            yield return null;
        }

        fadePanel.alpha = 0f;
        fadePanel.gameObject.SetActive(false);
    }

    public void FadeResume()
    {
        StartCoroutine(FadeResumeRoutine());
    }

    IEnumerator FadeResumeRoutine()
    {
        yield return StartCoroutine(FadeIn());
        ResumeGame();
        yield return StartCoroutine(FadeOut());
    }

    public void FadeOption()
    {
        StartCoroutine(FadeOptionRoutine());
    }

    IEnumerator FadeOptionRoutine()
    {
        yield return StartCoroutine(FadeIn());
        OpenOption();
        yield return StartCoroutine(FadeOut());
    }

    public void FadeHome()
    {
        StartCoroutine(FadeHomeRoutine());
    }

    IEnumerator FadeHomeRoutine()
    {
        yield return StartCoroutine(FadeIn());

        Time.timeScale = 1f; // Đảm bảo không bị pause khi chuyển scene
        SceneManager.LoadScene(0);

        yield return StartCoroutine(FadeOut());
    }

    public void TogglePause()
    {
        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }
}
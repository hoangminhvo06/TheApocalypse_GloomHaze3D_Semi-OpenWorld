using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenuManager : MonoBehaviour
{
    [Header("Main Menu Elements")]
    public GameObject playButton;
    public GameObject optionsButton;
    public GameObject exitButton;
    public GameObject devs_infoButton;
    
    [Header("Panels")]
    public GameObject optionsPanel;
    public GameObject devsInfoPanel;
    
    [Header("Texts")]
    public TMP_Text theapocalypse_gloomhazeTMP;
    public TMP_Text theapocalypse_gloomhazeShadowTMP;
    public TMP_Text versiontextTMP;
    public TMP_Text clocktextTMP;
    
    [Header("Effects")]
    public OptionsScaleEffect optionsEffect;

    private void Start()
    {
        // Đảm bảo tất cả các panel đều bị ẩn khi bắt đầu
        if (optionsPanel != null) optionsPanel.SetActive(false);
        if (devsInfoPanel != null) devsInfoPanel.SetActive(false);
        
        // Hiển thị các phần tử của main menu
        SetMainMenuElementsActive(true);
        
        // Kiểm tra các tham chiếu quan trọng
        CheckReferences();
    }
    
    private void CheckReferences()
    {
        if (optionsPanel == null) Debug.LogWarning("optionsPanel chưa được gán trong Inspector!");
        if (devsInfoPanel == null) Debug.LogWarning("devsInfoPanel chưa được gán trong Inspector!");
        if (playButton == null) Debug.LogWarning("playButton chưa được gán trong Inspector!");
        if (optionsButton == null) Debug.LogWarning("optionsButton chưa được gán trong Inspector!");
        if (exitButton == null) Debug.LogWarning("exitButton chưa được gán trong Inspector!");
        if (devs_infoButton == null) Debug.LogWarning("devs_infoButton chưa được gán trong Inspector!");
    }
    
    // Phương thức để hiển thị/ẩn các phần tử của main menu
    private void SetMainMenuElementsActive(bool active)
    {
        if (playButton != null) playButton.SetActive(active);
        if (optionsButton != null) optionsButton.SetActive(active);
        if (exitButton != null) exitButton.SetActive(active);
        if (devs_infoButton != null) devs_infoButton.SetActive(active);
        if (theapocalypse_gloomhazeTMP != null) theapocalypse_gloomhazeTMP.gameObject.SetActive(active);
        if (theapocalypse_gloomhazeShadowTMP != null) theapocalypse_gloomhazeShadowTMP.gameObject.SetActive(active);
        if (versiontextTMP != null) versiontextTMP.gameObject.SetActive(active);
        if (clocktextTMP != null) clocktextTMP.gameObject.SetActive(active);
    }
    
    #region Options Panel Methods
    
    // Phương thức được gọi khi bấm nút Options
    public void ShowOptionsPanel()
    {
        if (optionsPanel != null)
        {
            optionsPanel.SetActive(true);
            
            // Lấy thời gian từ OptionsScaleEffect, nếu chưa gán thì mặc định 0.3s
            float delay = (optionsEffect != null) ? optionsEffect.effectDuration : 0.3f;
            
            // Ẩn các phần tử của main menu sau một khoảng thời gian
            StartCoroutine(HideMainMenuElementsAfterDelay(delay));
        }
    }
    
    private IEnumerator HideMainMenuElementsAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SetMainMenuElementsActive(false);
    }
    
    #endregion
    
    #region Devs Info Panel Methods
    
    // Phương thức được gọi khi bấm nút Devs Info
    public void ShowDevsInfoPanel()
    {
        if (devsInfoPanel != null)
        {
            devsInfoPanel.SetActive(true);
            // Ẩn các phần tử của main menu ngay lập tức
            SetMainMenuElementsActive(false);
        }
    }
    
    #endregion
    
    #region Common Methods
    
    // Phương thức được gọi khi bấm nút Close trên bất kỳ panel nào
    public void BackToMainMenu()
    {
        // Ẩn tất cả các panel
        if (optionsPanel != null) optionsPanel.SetActive(false);
        if (devsInfoPanel != null) devsInfoPanel.SetActive(false);
        
        // Hiển thị lại các phần tử của main menu
        SetMainMenuElementsActive(true);
    }
    
    // Phương thức được gọi khi bấm nút Exit
    public void ExitGame()
    {
        Debug.Log("Thoát game");
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
    
    // Phương thức được gọi khi bấm nút Play
    public void StartGame()
    {
        Debug.Log("Bắt đầu game");
        // Thêm code để bắt đầu game ở đây
        // Ví dụ: SceneManager.LoadScene("GameScene");
    }
    
    #endregion
}

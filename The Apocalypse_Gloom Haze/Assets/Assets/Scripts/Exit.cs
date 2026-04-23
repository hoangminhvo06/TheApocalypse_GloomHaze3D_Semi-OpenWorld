using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviour
{
    public void Exitgame()
    {
        #if UNITY_WEBGL
        // WebGL không thể quit app, giải pháp là load lại MainMenu
        SceneManagement.LoadScene(0);
        #elif UNITY_EDITOR
        // Khi đang test trong Editor thì thoát PlayMode
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        // Android, Windows, Mac, Linux build đều exit được
         Application.Quit();
         #endif
    }
   
}

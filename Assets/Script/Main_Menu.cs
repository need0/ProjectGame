using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main_Menu : MonoBehaviour
{
    private void Start() 
    {
        Time.timeScale = 1.0f;
    }

    public void Button_Start()
    {
        SceneManager.LoadScene("Map_LV1");
    }
    public void Button_Credit() 
    {
        SceneManager.LoadScene("Credit");
    }
    public void Button_Home()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Button_Quit() 
    {
#if UnityEditor
        unityEditor.EditorApplication.isPlaying = false;
#endif
            Application.Quit();
    }


}

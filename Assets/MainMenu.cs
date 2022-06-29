using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    
    void Start()
    {
        
    }

    private void Awake()
    {
        
    }

    private void OnEnable()
    {
        
    }

    // Update is called once per frame
    // https://www.youtube.com/watch?v=thqNYDOOLF8 help with changing hud values
    void Update()
    {

    }

    public void StartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Tutorial", LoadSceneMode.Single);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

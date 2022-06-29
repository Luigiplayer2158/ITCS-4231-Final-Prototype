using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuHandler : MonoBehaviour
{
    // https://www.youtube.com/watch?v=JivuXdrIHK0 pause menu assistance
    // https://www.youtube.com/watch?v=TBcfhJoCVQo first minute of this video helped with a bug I had

    public static bool isPaused = false;
    private PlayerInputActions playerInputActions;
    private InputAction pauseAction;
    public GameObject pauseMenu;
    public GameObject gameHUD;
    public PlayerMovement player;

    public GameObject smallCollectableText;
    public GameObject largeCollectableText;
    public int collectableNum = 0;
    public int largeCollectableNum = 0;

    public Texture health3;
    public Texture health2;
    public Texture health1;
    public GameObject currImg;
    public RawImage rawImg;

    // Start is called before the first frame update
    // https://answers.unity.com/questions/42843/referencing-non-static-variables-from-another-scri.html getting player health assistance
    // https://docs.unity3d.com/2018.1/Documentation/ScriptReference/UI.RawImage-texture.html changing texture
    void Start()
    {
        rawImg = currImg.GetComponent<RawImage>();
    }

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        pauseAction = playerInputActions.Player.Pause;
        playerInputActions.Player.Pause.Enable();
    }

    // Update is called once per frame
    // https://www.youtube.com/watch?v=thqNYDOOLF8 help with changing hud values
    void Update()
    {
        if (pauseAction.triggered)
        {
            if (isPaused)
            {
                Resume();
            } else
            {
                Pause();
            }
        }
        
       

        if (player.health == 3)
        {
            rawImg.texture = health3;
        } else if (player.health == 2)
        {
            rawImg.texture = health2;
        } else if (player.health <= 1)
        {
            rawImg.texture = health1;
        }

        if (GameObject.Find("SmallCollectable") == null)
        {
            smallCollectableText.GetComponent<Text>().fontSize = 15;
            smallCollectableText.GetComponent<Text>().text = "No More Small Collectables To Be Found!";
        }
        if (GameObject.Find("SmallCollectable") != null)
        {
            smallCollectableText.GetComponent<Text>().text = "x " + collectableNum.ToString();
        }

        if (GameObject.Find("LargeCollectable") == null)
        {
            largeCollectableText.GetComponent<Text>().fontSize = 15;
            largeCollectableText.GetComponent<Text>().text = "No More Large Collectables To Be Found!";
        }
        if (GameObject.Find("LargeCollectable") != null)
        {
            largeCollectableText.GetComponent<Text>().text = "x " + largeCollectableNum.ToString();
        }

    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        gameHUD.SetActive(true);
        Time.timeScale = 1f;
        isPaused = false;
    }

    void Pause()
    {
        pauseMenu.SetActive(true);
        gameHUD.SetActive(false);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void GoHome()
    {
        pauseMenu.SetActive(false);
        gameHUD.SetActive(true);
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main_HUB", LoadSceneMode.Single);
        isPaused = false;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

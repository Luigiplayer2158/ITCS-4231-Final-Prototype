using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoad : MonoBehaviour
{
    // Start is called before the first frame update
    //https://docs.unity3d.com/ScriptReference/Collision-gameObject.html collision
    //https://docs.unity3d.com/ScriptReference/SceneManagement.SceneManager.LoadScene.html scene manager
    public string LevelName;

    public BoxCollider collision;
    void Start()
    {
 
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Touch2!");
            SceneManager.LoadScene(LevelName, LoadSceneMode.Single);
        }
    }
}

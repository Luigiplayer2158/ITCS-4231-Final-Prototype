using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{

    //https://www.youtube.com/watch?v=rO19dA2jksk moving platform help
    public GameObject player;
    public GameObject emptyObject;
    // Start is called before the first frame update
    void Start()
    {
        //https://answers.unity.com/questions/147816/how-to-avoid-scaling-heritage-when-parenting.html avoiding scale messing up player when using parent
        emptyObject = new GameObject();
        emptyObject.transform.parent = this.transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            player.transform.parent = emptyObject.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            player.transform.parent = null;
        }
    }
}

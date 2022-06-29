using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCode : MonoBehaviour
{
    public Transform target;
    bool playerSeen = false;


    // https://www.youtube.com/watch?v=6ZkvUu4HFKI basic AI enemy code from this video, I added the aggressive radius on my own.
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (playerSeen)
        {
            transform.LookAt(target);
            transform.Translate(Vector3.forward * 5 * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerSeen = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerSeen = false;
        }
    }

}

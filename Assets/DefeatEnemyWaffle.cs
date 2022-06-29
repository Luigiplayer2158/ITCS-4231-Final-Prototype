using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefeatEnemyWaffle : MonoBehaviour
{
    public MeshRenderer mR;
    public Collider boxCollider;
    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.Find("Enemy") == null)
        {
            mR.enabled = true;
            boxCollider.enabled = true;
        }
        if (GameObject.Find("Enemy") != null)
        {
            mR.enabled = false;
            boxCollider.enabled = false;
        }
    }
}

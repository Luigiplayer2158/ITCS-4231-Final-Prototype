using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LargeCollectableBase : MonoBehaviour
{
    public GameObject thisObject;
    public MenuHandler menuHUD;
    public BoxCollider collision;
    public int collectableValue = 1;
    public Rigidbody rb;
    Vector3 velocity;
    public float rotateSpeed = 100;

    private bool isTouched = false;

    //https://docs.unity3d.com/ScriptReference/Rigidbody.MoveRotation.html rotate the object

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        velocity = new Vector3(rotateSpeed, rotateSpeed, rotateSpeed);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        Quaternion rotation = Quaternion.Euler(velocity * Time.fixedDeltaTime);
        rb.MoveRotation(rb.rotation * rotation);
    }
    //https://docs.unity3d.com/ScriptReference/Object.Destroy.html destroy object when collected
    //https://answers.unity.com/questions/738991/ontriggerenter-being-called-multiple-times-in-succ.html help with object being called twice
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" & !isTouched)
        {
            Destroy(thisObject);
            Destroy(collision);
            Destroy(this);
            Destroy(rb);


            menuHUD.largeCollectableNum += 1;
            Debug.Log(menuHUD.largeCollectableNum);
            isTouched = true;
        }
    }
}

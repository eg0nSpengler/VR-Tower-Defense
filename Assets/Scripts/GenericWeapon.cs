using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericWeapon : MonoBehaviour
{
    public float VelocityNeededToCare = 5f;
    private new Rigidbody rigidbody;
    private GM gm;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        if (gm == null)
            gm = GameObject.FindWithTag("GameController").GetComponent<GM>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.transform.root.gameObject.tag == "Joe")
        {
            Debug.Log("Thwacked " + other.gameObject.transform.root.gameObject.name + " with a " + gameObject.name + " with a magnitude of " + rigidbody.velocity.magnitude * rigidbody.mass + "! Owned!");
            gm.RemoveEnemy(other, rigidbody);
            //else
            //{
            //    other.gameObject.SendMessage("UpdateKinematic", true);
            //}   
        }
    }
}

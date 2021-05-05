using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    public string tagFilter;
    public int damage;
    private bool useTag;
    private GM gm;
    //-------------------------------------------------
    void Start()
    {
        if (gm == null)
            gm = GameObject.FindWithTag("GameController").GetComponent<GM>(); // get our gm
        if (!string.IsNullOrEmpty(tagFilter))
        {
            Debug.Log("Found tag, only killing tagged objects...."); // if we have a tag, we only kill stuff with that tag. goodbye weapons being cut up
            useTag = true;
        }
        if (gm==null) // wait what we don't have a gm? panic!
        {
            Debug.LogError("Trap couldn't find GM!!! Is it tagged correctly?");
        }

    }


    //-------------------------------------------------
    void OnTriggerEnter(Collider collider)
    {
        Debug.Log("Object entered trap! Tagged as: " + collider.gameObject.transform.root.gameObject.tag);
        Debug.Log(collider.gameObject.transform.root.gameObject.transform.position);
        if (!useTag || (useTag && collider.gameObject.transform.root.gameObject.tag == tagFilter))
        {
            Debug.Log("Object has been killed, energy rewarded.");
            // tell our gm to clean it up
            gm.RemoveEnemy(collider, null, damage);
        }
    }
}

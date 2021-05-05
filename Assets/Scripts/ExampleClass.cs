using UnityEngine;
using System.Collections;

public class ExampleClass : MonoBehaviour
{
    void OnCollisionStay(Collision collisionInfo)
    {
        // Debug-draw all contact points and normals

        foreach (ContactPoint contact in collisionInfo.contacts)
        {
            Debug.DrawRay(contact.point, contact.normal, Color.cyan);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VendingObject : MonoBehaviour
{
    [System.Serializable]
    public class VendableObject
    {
        public string name;
        public GameObject vendedObject;
        public int stock;
        public int energyCost;
    }

    public VendableObject[] vendableObjects;
}

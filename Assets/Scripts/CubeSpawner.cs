using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class CubeSpawner : MonoBehaviour
{
    public Hand leftHand;
    public Hand rightHand;
    private GameObject prim;
    public PrimitiveType primitiveType = PrimitiveType.Cube;
    bool spewDebug = false;
    bool debug = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (SteamVR_Actions._default.CreateCube.GetStateDown(SteamVR_Input_Sources.LeftHand) || SteamVR_Actions._default.CreateCube.GetStateDown(SteamVR_Input_Sources.RightHand))
        {
            CreatePrimitive(primitiveType);
            //cube.AddComponent<FreezeMovement>();
        }
    }
    private static Transform tHelper = null;
    private void CreateObject(Transform @object, Transform leftHand, Transform rightHand)
    {
        if (!tHelper)
        {
            GameObject oHelper = new GameObject
            {
                name = "CreateObject_helper"
            };
            tHelper = oHelper.transform;
        }

        // helper
        tHelper.position = (leftHand.position + rightHand.position) * 0.5f;
        tHelper.LookAt(rightHand.position, Vector3.up);

        @object.SetPositionAndRotation(tHelper.position, tHelper.rotation);

        // get controller pos
        Vector3 vConL = tHelper.InverseTransformPoint(leftHand.position);
        Vector3 vConR = tHelper.InverseTransformPoint(rightHand.position);
        if (spewDebug)
        {
            Debug.Log("Left Controller Inverse Transform Point: " + vConL);
            Debug.Log("Right Controller Inverse Transform Point: " + vConR);
        }
        float xScale = (leftHand.position.x - rightHand.position.x);
        float yScale = (leftHand.position.y - rightHand.position.y);
        float zScale = (leftHand.position.z - rightHand.position.z);
        if (debug)
        {
            Debug.Log("Creating object with scale : " + xScale + ", " + yScale + ", " + zScale);
        }
        @object.localScale = new Vector3(System.Math.Abs(xScale), System.Math.Abs(yScale), System.Math.Abs(zScale));
    }
    private void CreatePrimitive(PrimitiveType primitive)
    {
        prim = GameObject.CreatePrimitive(primitive);
        CreateObject(prim.transform, leftHand.transform, rightHand.transform);
        if (spewDebug)
        {
            Debug.Log("Left Hand Position: " + leftHand.transform.position.ToString());
            Debug.Log("Right Hand Position: " + rightHand.transform.position.ToString());
        }
        prim.AddComponent<Interactable>();
        prim.AddComponent<NavMeshObstacle>();
        prim.AddComponent<Rigidbody>();
        prim.AddComponent<Throwable>();
        prim.GetComponent<NavMeshObstacle>().carveOnlyStationary = false;
        prim.GetComponent<NavMeshObstacle>().carving = true;
    }
}

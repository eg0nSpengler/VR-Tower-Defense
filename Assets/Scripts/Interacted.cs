using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Valve.VR.InteractionSystem;

public class Interacted : MonoBehaviour 
{
    private Interactable interactable;
    private NavMeshAgent agent;
    public Collider mainCollider;
    public new Rigidbody rigidbody;
    public bool isGrabbed;
    public bool forceTouchingGround = false;
    private LayerMask mask = default;
    public float fallTime = 1.0f;
    public float fallenTimer = 1f;
    private float TimeLastGrabbed = 0;
    public bool touchingGround = true;
    private float distToGround;
    public bool stuck = false;
    public byte stuckCount = 0;
    // Start is called before the first frame update
    void Start()
    {
        // Get all our components.
        interactable = GetComponent<Interactable>();
        agent = GetComponent<NavMeshAgent>();
        rigidbody = GetComponent<Rigidbody>();
        distToGround = GetComponent<Collider>().bounds.extents.y;
        mask = LayerMask.NameToLayer("Joe");
        TimeLastGrabbed = Time.realtimeSinceStartup;
        agent.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        // Have they gotten up yet?
        if (fallenTimer >= 0)
        {
            fallenTimer -= Time.deltaTime; // Remove from the timer.
        }
        else if (forceTouchingGround || stuck)
            touchingGround = true;
        UpdateKinematic();
        //rigidbody.WakeUp();
        isGrabbed = interactable.attachedToHand;            
    }
    void OnCollisionStay(Collision collisionInfo)
    {
        //Debug.Log("In continuous contact with " + collisionInfo.transform.name);
        touchingGround = true;
        rigidbody.isKinematic = true;
        stuckCount = 0;

        stuck = false;
        //rigidbody.velocity = Vector3.zero;
        //rigidbody.angularVelocity = Vector3.zero;
    }
    private void OnCollisionEnter(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            Debug.DrawRay(contact.point, contact.normal, Color.green);
        }
        //Debug.Log("In contact with " + collision.transform.name);
        touchingGround = true;
        stuckCount++;
        CheckStuck();
        rigidbody.isKinematic = true;
    }
    private void OnCollisionExit(Collision collision)
    {
        //Debug.Log("No longer in contact with " + collision.transform.name);
        touchingGround = false;
        stuckCount++;
        CheckStuck();
        rigidbody.isKinematic = false;
    }
    void CheckStuck()
    {

        if (stuckCount >= 5)
        {
            Debug.Log("I'm stuck! Forcing myself to touch the ground...");
            stuck = true;
        }
    }
    public void UpdateKinematic(bool beingHit = false)
    {
        if (fallenTimer <= 0)
        {
            if (isGrabbed || !touchingGround || beingHit)
            {
                agent.enabled = false;
                rigidbody.isKinematic = false;
                fallenTimer = fallTime;
                Ragdoll(true);
            }
            else if (touchingGround)
            {
                agent.enabled = true;
                rigidbody.isKinematic = true;
            }
        }
        else
        {
            agent.enabled = false;
            rigidbody.isKinematic = false;
            Ragdoll(true);
        }
        
    }
    public void Ragdoll(bool isRagdoll)
    {
        Rigidbody[] bodies = GetComponentsInChildren<Rigidbody>();
        Collider[] allColliders = GetComponentsInChildren<Collider>();
        //Debug.Log(allColliders.ToString());
        foreach (Collider col in allColliders)
        {
            col.enabled = isRagdoll;
        }
        mainCollider.enabled = !isRagdoll;
        GetComponent<Rigidbody>().useGravity = isRagdoll;
        GetComponent<Animator>().enabled = !isRagdoll;
    }
}
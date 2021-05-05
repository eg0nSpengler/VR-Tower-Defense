using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveTo : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform goal;
    private NavMeshAgent agent;
    private GM gm;
    private Interacted isHeld;
    private EnemyInfo enemyInfo;
    private LineRenderer lineRenderer;
    void Start()
    {
        if (gm == null)
            gm = GameObject.FindWithTag("GameController").GetComponent<GM>();
        agent = GetComponent<NavMeshAgent>();
        agent.enabled = false;
        isHeld = GetComponent<Interacted>();
        lineRenderer = GetComponent<LineRenderer>();
        enemyInfo = GetComponent<EnemyInfo>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Agent Status: Enabled ="
        //    + agent.enabled.ToString()
        //    + " Touching ground: "
        //    + isHeld.touchingGround.ToString()
        //    + " IsGrabbed: "
        //    + isHeld.isGrabbed
        //    + "\n"
        //    + "Velocity, position: "
        //    + isHeld.rigidbody.velocity.ToString()
        //    + ", "
        //    + isHeld.transform.position.ToString()
        //    );
        //if (agent.hasPath)
        //{
        //    lineRenderer.positionCount = agent.path.corners.Length;
        //    lineRenderer.SetPositions(agent.path.corners);
        //    lineRenderer.enabled = true;
        //}
        if (agent.enabled && isHeld.touchingGround && !isHeld.isGrabbed)
        {
            NavMeshPath path = new NavMeshPath();
            agent.CalculatePath(goal.position, path);
            if (path.status == NavMeshPathStatus.PathInvalid || path.status == NavMeshPathStatus.PathPartial)
            {
                // Target is unreachable, get angery
            }
            if (path.status == NavMeshPathStatus.PathComplete)
            {
                // can get to goal
                
            }
            //Debug.Log(transform.position.ToString() + goal.position.ToString());
            agent.destination = goal.position;
            if (Vector3.Distance(transform.position, goal.position) <= 0.5 )
            {
                Debug.Log("Enemy reached goal!");
                gm.EnergyController.TakeDamage(enemyInfo.CurrentHealth, enemyInfo.isBoss);
                Destroy(transform.root.gameObject);
                gm.CurrentEnemies--;
            }
            
        }
    }
}

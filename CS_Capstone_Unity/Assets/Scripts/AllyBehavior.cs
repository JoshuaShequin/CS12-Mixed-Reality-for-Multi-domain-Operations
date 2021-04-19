using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;



public class AllyBehavior : MonoBehaviour
{

    NavMeshAgent agent;     // Baked pathing agent

    // Unit Params
    public GameObject bullet;
    public int health = 100;
    STATE state;            // General behavior state
    FIRE_ORDERS fireOrders;

    // Movement Params
    public int wanderRadius = 50;
    public int wanderTime = 100;
    private int wanderTix = 0;                                  // Gives delay to movement commands to prevent stutter steps
    private List<Vector3> patrolPoints = new List<Vector3>();   // Holds patrol point positions
    private int destPoint = 0;                                  // Index for current patrol point target

    // Attack Params
    private GameObject currTarget;  // Will hold a reference to where the target is
    public int clipSize = 30;       // How many shots before reload
    public int reloadTime = 10;     // How long to reload
    public float rateOfFire = 0.5f;    // Time between shots. Values < 1 are better
    private bool canShoot = true;      // Can shots be fired

    // Vision Params
    public float visionRadius = 20;
    public float visionDistance = 50;
    private RaycastHit hitInfo;

    // Behavior States
    private enum STATE
    {
        SCANNING, MOVING, ATTACKING, PATROLLING, DEAD
    }

    // Determines if units will ask for permission to engage
    public enum FIRE_ORDERS
    {
        HOLD_FIRE, FREE_FIRE
    }

    // Initialize and start
    void Start()
    {
        // Get unit specific Nav Agent
        agent = GetComponent<NavMeshAgent>();

        // Set starting states
        state = STATE.SCANNING;
        fireOrders = FIRE_ORDERS.FREE_FIRE;
    }

    void Update()
    {
        // Will have to get different input key for VR input
        if (Input.GetMouseButtonDown(1))
        {
            MoveToDest();
        }

        // Currently uses shift-Click to set a patrol point
        if (  ( Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift) ) && Input.GetMouseButtonDown(0) ) {
            AddPatrolPoint();
        }

        // Clear all set patrol points on ctrl-lmb
        if (( (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl) ) && Input.GetMouseButtonDown(0)) ) {
            RemovePatrolPoints();
        }

        BehaviorStateMachine();
    }

    // Add a patrol point for the unit
    void AddPatrolPoint()
    {
        Debug.Log("==Patrol point added");
        RaycastHit hit;

        // This cast input key will have to be matched to VR input
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity) && hit.transform != null)
        {
            Debug.Log(hit.point);
            patrolPoints.Add(hit.point);
        }

        // If this is the first patrol point, start moving towards it
        if (patrolPoints.Count == 1)
        {
            agent.SetDestination(patrolPoints[0]);
        }
        state = STATE.PATROLLING;

    }


    // Remove all assigned patrol points
    void RemovePatrolPoints()
    {
        Debug.Log("==Patrol points cleared");
        patrolPoints.Clear();
        state = STATE.SCANNING;
    }


    // Will move between patrol points
    void Patrol()
    {
        // Do we need to update the patrol point yet?
        if ((!agent.pathPending && agent.remainingDistance <= 0.5f) && patrolPoints.Count > 0)
        {
            agent.SetDestination(patrolPoints[destPoint]);
            destPoint = (destPoint + 1) % patrolPoints.Count;
        }
    }


    // Can be used to raycast a destination to units
    void MoveToDest()
    {
        RaycastHit hit;

        // This cast input key will have to be matched to VR input
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100))
        {
            agent.destination = hit.point;
        }
    }


    // Will check for hostile vision collision and return a bool
    public bool CheckLineOfSight()
    {
        LayerMask mask = LayerMask.GetMask("Enemy");
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, visionRadius, mask);
        if (hitColliders.Length >= 1)
        {
            Debug.Log("Enemy spotted");
            // Set the current target
            currTarget = hitColliders[0].gameObject;

            // Activate attack state
            if (fireOrders == FIRE_ORDERS.FREE_FIRE)
            {
                state = STATE.ATTACKING;
                return true;
            }
            else if (fireOrders == FIRE_ORDERS.HOLD_FIRE)
            {
                // Log to player that an enemy has been spotted
                // Ask player for confirmation to engage
                Debug.Log("Ally is holding fire due to Hold Fire state");
                return true;
            }
            return true;
        }
        else
        {
            return false;   // No vision detection
        }
    }


    // Remove health from unit
    public void LoseHealth(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            // Turn off
            state = STATE.DEAD;
        }
        else if (state != STATE.ATTACKING)  // We are taking damage, attack back
            state = STATE.ATTACKING;
    }


    // Face model towards targets current position
    public void FaceTarget()
    {
        this.transform.LookAt(currTarget.transform);
    }


    // Coroutine that works to delay fire rate.
    IEnumerator ShootDelay()
    {
        yield return new WaitForSeconds(rateOfFire);
        canShoot = true;

    }

    // Check for hostiles and fire
    public void Attack()
    {
        if (CheckLineOfSight())
        {
            if (canShoot)
            {
                // FaceTarget is called to ensure correct target position
                FaceTarget();

                Debug.Log("Attacking");
                GameObject instantiatedBullet = (GameObject)Instantiate(bullet, transform.position + (Vector3.up * 2) + (transform.forward * 2)
                                                , transform.rotation);
                canShoot = false;
                StartCoroutine(ShootDelay());   // Enforce units rate of fire 
            }
        }
        else
        {
            // Search for possibly more hostiles
            state = STATE.SCANNING;
        }
    }


    public void Scanning()
    {
        // Check for hostiles. Move if none are found
        if (CheckLineOfSight())
        {
            state = STATE.ATTACKING;
        }
        // Back to patrol
        else if (patrolPoints.Count > 0)
        {
            state = STATE.PATROLLING;
        }
        // Start wandering
        else
        {
            state = STATE.MOVING;
           
        }
    }

    // Gets a random Vector3 relative to origin and wanderRadius.
    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randomDirection = Random.insideUnitSphere * dist;

        randomDirection += origin;
        NavMeshHit hitInfo;
        NavMesh.SamplePosition(randomDirection, out hitInfo, dist, layermask);

        return hitInfo.position;
    }

    // Unit will wander in a direction no farther then the wanderRadius
    public void Moving()
    {
        if (--wanderTix > 0) { state = STATE.SCANNING; }
        else
        {
            Vector3 newPosition = RandomNavSphere(transform.position, wanderRadius, -1);
            agent.SetDestination(newPosition);
            state = STATE.SCANNING;
            wanderTix = wanderTime;
        }
    }


    // Destroy gameobject when health is depleted
    private void DeleteUnit()
    {
        Destroy(this.gameObject);
    }


    private void BehaviorStateMachine()
    {

      switch (state)
      {
            case STATE.ATTACKING:
                    Attack();
                    break;
            case STATE.SCANNING:
                    Scanning();
                    break;
            case STATE.MOVING:
                    Moving();
                    break;
            case STATE.PATROLLING:
                    Patrol();
                    break;
            case STATE.DEAD:
                    DeleteUnit();
                    break;
        }
    }
}

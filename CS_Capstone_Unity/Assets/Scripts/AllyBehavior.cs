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
    public STATE state;            // General behavior state
    FIRE_ORDERS fireOrders;
    public float alertRadius = 15.0f;
    public static int alertCooldown = 4;
    private int alertTix = 0;

    // Movement Params
    public int wanderRadius = 50;
    public int wanderPause = 5;                                 // How long a unit will pause at a destination before moving again
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

    // Variables for the Trail
    public TrailRenderer trail;
    public bool trail_active = false;
    private int enable_time = 60; // the trail renderer needs to exist for a few frames before we hide it


    // VR variables
    private GameObject playerObject;
    private Transform anchor;
    private GameObject fogofwar;
    private bool isSelected = false;
    public GameObject selectedSprite;
    

    // Behavior States
    public enum STATE
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


        // Initiating player transform for controls

        selectedSprite.SetActive(false);
        playerObject = GameObject.FindGameObjectWithTag("Player_Tracking_Space");
        anchor = playerObject.transform.Find("TrackingSpace").transform.Find("RightHandAnchor");
        


        // Set starting states
        state = STATE.SCANNING;
        fireOrders = FIRE_ORDERS.FREE_FIRE;

    }

    void Update()
    {
        
        // Will have to get different input key for VR input


        if(OVRInput.GetDown(OVRInput.Button.Two)) {
            if(isSelected) {
                MoveToDest();
            }
        }
        




        // Currently uses shift-Click to set a patrol point
        if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && Input.GetMouseButtonDown(0))
        {
            // Toss the mouse click position
            RaycastHit hit;

            // This cast input key will have to be matched to VR input
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity) && hit.transform != null)
            {
                AddPatrolPoint(hit.point);
            }
        }

        // Clear all set patrol points on ctrl-lmb
        if (((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) && Input.GetMouseButtonDown(0)))
        {
            RemovePatrolPoints();
        }


        BehaviorStateMachine();

        // the trail renderer needs to exist for a few movement frames before we hide it
        if (!trail_active)
        {
            if (enable_time > 0)
            {
                enable_time -= 1;
            }
            else if (enable_time == 0)
            {
                trail_active = true;
                ToggleTrail();
                enable_time = -1;
            }
        }
    }

    // Alerts units within a radius from the center to help in battle at destination.
    void AlertTeam(Vector3 center, float radius, Vector3 destination)
    {
        Debug.Log("==A unit is asking nearby units for help.");
        // Mask for only the Ally unit layer 8
        int layerMask = 1 << 8;
        Collider[] hitColliders = Physics.OverlapSphere(center, radius, layerMask);

        // For each unit found in the sphere, ask them to come to you
        foreach (var hitCollider in hitColliders)
        {
            // End patrol and send to destination. If backtracking is really wanted, old points will have to be stored
            hitCollider.gameObject.GetComponent<AllyBehavior>().RemovePatrolPoints();
            hitCollider.gameObject.GetComponent<AllyBehavior>().MoveToVector(destination);
            hitCollider.gameObject.GetComponent<AllyBehavior>().state = STATE.ATTACKING;
        }
    }

    // Add a patrol point for the unit
    public void AddPatrolPoint(Vector3 position)
    {
        Debug.Log("==Patrol point added");
        patrolPoints.Add(position);

        // If this is the first patrol point, start moving towards it
        if (patrolPoints.Count == 1)
        {
            agent.destination = patrolPoints[0];
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


    // Will move between patrol points. Called in PATROLLING state
    void Patrol()
    {
        
        // Do we need to update the patrol point yet?
        if (!agent.pathPending)
        {
            if (agent.remainingDistance <= 2.5f && patrolPoints.Count > 0)
            {
                destPoint = (destPoint + 1) % patrolPoints.Count;
            }
            agent.SetDestination(patrolPoints[destPoint]);
        }

    }


    // Can be used to raycast a destination to units
    public void MoveToDest()
    {
        
        RaycastHit hit;
    
        if(Physics.Raycast(anchor.position, anchor.TransformDirection(Vector3.forward), out hit, 100)) {
            agent.SetDestination(hit.point);
            Debug.Log("Moving to " + hit.point);
        }   
    }


    public void MoveToVector(Vector3 target)
    {
        agent.SetDestination(target);
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
                // Alert nearby units
                if (alertTix == 0)
                {
                    AlertTeam(transform.position, alertRadius, currTarget.transform.position);
                    alertTix = alertCooldown;
                }
                else
                    alertTix--;

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
        // This will ensure that destination orders are followed. 
        // MoveTo* functions can circumvent this block allowing for priority movement.
        if (agent.remainingDistance <= 2.5f)
        {

            // WanderTix gives pause at stop points
            if (--wanderTix > 0) { state = STATE.SCANNING; }
            else
            {
                Vector3 newPosition = RandomNavSphere(transform.position, wanderRadius, -1);
                agent.SetDestination(newPosition);
                state = STATE.SCANNING;
                wanderTix = wanderPause;
            }
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

    public void ToggleTrail()
    {
        trail_active = !trail_active;
        trail.enabled = trail_active;
    }

    public void SelectedUnit() {
        if(isSelected) {
            selectedSprite.SetActive(false);
            isSelected = false;
        } else {
            selectedSprite.SetActive(true);
            isSelected = true;
        }
    }
}
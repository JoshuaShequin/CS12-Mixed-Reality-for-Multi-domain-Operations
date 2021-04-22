using UnityEngine;
using UnityEngine.AI;
using System.Collections;


public class EnemyBehavior : MonoBehaviour
{

    NavMeshAgent agent;     // Baked pathing agent

    // Unit Params
    public GameObject bullet;
    public int health = 100;
    public STATE state;            // General behavior state
    FIRE_ORDERS fireOrders;

    // Movement Params
    public int wanderRadius = 50;
    public int wanderTime = 100;
    private int wanderTix = 0;

    // Attack Params
    private GameObject currTarget;     // Will hold a reference to where the target is
    public int clipSize = 30;          // How many shots before reload
    public int reloadTime = 10;        // How long to reload
    public float rateOfFire = 0.5f;    // Time between shots. Values < 1 are better
    private bool canShoot = true;      // Can shots be fired

    // Vision Params
    public float visionRadius = 20;
    public float visionDistance = 50;
    private RaycastHit hitInfo;

    // Behavior States
    public enum STATE
    {
        SCANNING, MOVING, ATTACKING, DEAD
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
        if (Input.GetMouseButtonDown(0))
        {
            MoveToDest();
        }

        BehaviorStateMachine();
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


    // Will check for hostile vision collision and return a bool only for forward line of site
    public bool CheckLineOfSight()
    {
        LayerMask mask = LayerMask.GetMask("Ally");
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, visionRadius, mask);
        if (hitColliders.Length >= 1)
        {
            //Debug.Log("Ally unit spotted");
            // Set the current target
            currTarget = hitColliders[0].gameObject;

            // Activate attack state
            if (fireOrders == FIRE_ORDERS.FREE_FIRE)
            {
                state = STATE.ATTACKING;
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
        // Eventually will rotate unit to move vision cone and check a wider area for hostiles
        if (CheckLineOfSight())
        {
            state = STATE.ATTACKING;
        }
        else
        {

            // Check near FoV
            if (CheckLineOfSight())
            {
                state = STATE.ATTACKING;
            }
            else
            {
                state = STATE.MOVING;
            }
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
        if (--wanderTix > 0) { return; }
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
            case STATE.DEAD:
                DeleteUnit();
                break;
        }
    }
}

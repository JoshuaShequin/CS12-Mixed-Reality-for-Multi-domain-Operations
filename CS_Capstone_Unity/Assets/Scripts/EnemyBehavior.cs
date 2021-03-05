using UnityEngine;
using UnityEngine.AI;

public class EnemyBehavior : MonoBehaviour
{

    public int health = 100;

    NavMeshAgent agent;     // Baked pathing agent

    // Vision params
    public float visionRadius;
    public float visionDistance;
    private RaycastHit hitInfo;

    // Start is called to grab the NavMesh Agent
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        // Will have to get different input key for VR input
        if (Input.GetMouseButtonDown(0))
        {
            MoveToDest();
        } 

        // Vision call
        CheckLineOfSight();

    }

    // Will be called on mouse click to set a move destination
    void MoveToDest()
    {
        RaycastHit hit;

        // This cast input key will have to be matched to VR input
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100))
        {
            agent.destination = hit.point;
        }
    }

    public void CheckLineOfSight()
    {
        LayerMask mask = LayerMask.GetMask("Ally");
        if (Physics.SphereCast(transform.position, visionRadius, transform.forward, out hitInfo, visionDistance, mask))
        {   
            // Activate state, i.e. Cover/Attack     
        }
    }

    public void LoseHealth(int damage)
    {
        Debug.Log("Enemy has lost health");
        Debug.Log("Health is: " + health);
        health -= damage;
        if (health <= 0)
        {
            Destroy(this.gameObject);
        }
    }

  
}
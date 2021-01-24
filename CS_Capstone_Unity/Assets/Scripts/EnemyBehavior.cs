using UnityEngine;
using UnityEngine.AI;

public class EnemyBehavior : MonoBehaviour
{

    NavMeshAgent agent;     // Baked pathing agent

    // Start is called to grab the NavMesh Agent
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Fixed Update is called more often and recommend for physics operations
    // This code can be transferred to Update() if performance is an issue
    void FixedUpdate()
    {

        // Check for hostile forces
        // !! Currently doesnt collide on other layers !! //
        LayerMask mask = LayerMask.GetMask("Allied Forces");
        RaycastHit hit;

        // Check if there is a collision
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 1000, mask))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.red);
            Debug.Log("==Enemy has spotted an allied force.");
        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.green);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Will have to get different input key for VR input
        if (Input.GetMouseButtonDown(1))
        {
            MoveToDest();
        }
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
}
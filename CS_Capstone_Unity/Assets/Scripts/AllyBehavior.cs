using UnityEngine;
using UnityEngine.AI;


public class AllyBehavior : MonoBehaviour
{

    NavMeshAgent agent;     // Baked pathing agent

    void Start()
    {
        // Get unit specific Nav Agent
        agent = GetComponent<NavMeshAgent>();    
    }

    // Update is called once per frame
    void Update()
    {
        // Will have to get different input key for VR input
        if (Input.GetMouseButtonDown(0)) {
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

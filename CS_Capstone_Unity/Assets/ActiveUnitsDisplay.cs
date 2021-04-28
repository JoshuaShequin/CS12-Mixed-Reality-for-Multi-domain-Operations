using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActiveUnitsDisplay : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject[] gameObjects;
        gameObjects = GameObject.FindGameObjectsWithTag("Ally_NPC");
   

        if(gameObjects.Length == 0) {
            Debug.Log("No objects tagged with 'Ally_NPC'");
        } else {
            Debug.Log("Number of objects found with the tag 'Ally_NPC': " + gameObjects.Length);
        }

        foreach (GameObject Ally in gameObjects) {
            AllyBehavior ab = Ally.GetComponent<AllyBehavior>();
            Debug.Log("Ally Health: " + ab.health);
        }

    

        for(int i = 0; i < gameObjects.Length; i++) {
            AllyBehavior ab = gameObjects[i].GetComponent<AllyBehavior>();
            Debug.Log("Ally Health: " + ab.health);
            
        }


    }

    // // Update is called once per frame
    // void Update()
    // {
        
    // }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsMenuScript : MonoBehaviour
{
    public GameObject SettingsMenuCanvas;
    public GameObject mainMenu;
    public GameObject fogOfWar;
    GameObject[] gameObjects;
    
    // Start is called before the first frame update
    void Start(){
        SettingsMenuCanvas.SetActive(false);
    }

    public void togglePath() {
        gameObjects = GameObject.FindGameObjectsWithTag("Ally_NPC");
        foreach (GameObject Ally in gameObjects) {
            AllyBehavior ab = Ally.GetComponent<AllyBehavior>();
            ab.ToggleTrail();
        }
    }

    public void toggleFog() {
        if(fogOfWar.activeSelf) {
            fogOfWar.SetActive(false);
        } else {
            fogOfWar.SetActive(true);
        }
    }

    public void exitSettings() {
        SettingsMenuCanvas.SetActive(false);
        mainMenu.SetActive(true);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitStatusUpdater : MonoBehaviour
{

    public GameObject ally_NPC;
    public Text text;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        AllyBehavior ab = ally_NPC.GetComponent<AllyBehavior>();
        text.GetComponent<UnityEngine.UI.Text>().text = "Unit Health: " + ab.health.ToString() + "\nStatus: " + ab.state;
    }
}

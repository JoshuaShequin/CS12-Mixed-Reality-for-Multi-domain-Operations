using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitLogUpdateScript : MonoBehaviour
{

    public GameObject ally;
    public Text text;
    private string oldState;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        AllyBehavior ab = ally.GetComponent<AllyBehavior>();
        text.GetComponent<UnityEngine.UI.Text>().text = "Unit Status: " + ab.state + "..." + "\n";
    }
}

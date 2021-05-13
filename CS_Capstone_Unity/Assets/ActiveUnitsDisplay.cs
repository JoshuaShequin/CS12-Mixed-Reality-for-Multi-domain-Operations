using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActiveUnitsDisplay : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject UnitLogView;
    public GameObject ActiveUnitView;

    public GameObject text_prefab;

    private GameObject[] allies;

    private bool units_selected = false;
    void Start()
    {


    }

    // // Update is called once per frame
    void Update()
    {
        if (!units_selected)
        {
            allies = GameObject.FindGameObjectsWithTag("Ally_NPC");
            if (allies.Length > 0)
            {
                units_selected = true;
                foreach(GameObject ally in allies)
                {
                    var item = Instantiate<GameObject>(text_prefab, ActiveUnitView.transform.Find("Viewport").transform.Find("Content"));
                    item.GetComponent<UnitStatusUpdater>().ally_NPC = ally;
                }
            }

        }
    }

}

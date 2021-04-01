using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    public GameObject ally;
    public Image foregroundImage;
    AllyBehavior ab;

    int hp_amount;
    int max_hp;

    // Start is called before the first frame update
    void Start()
    {
        ab = ally.GetComponent<AllyBehavior>();
        hp_amount = ab.health;
        max_hp = hp_amount;
    }

    // Update is called once per frame
    void Update()
    {
        hp_amount = ab.health;
        if (hp_amount == 0)
        {
            foregroundImage.fillAmount = 0.0f;
        }
        else
        {
            foregroundImage.fillAmount = (float)hp_amount / (float)max_hp;

        }
        
        
    }
}

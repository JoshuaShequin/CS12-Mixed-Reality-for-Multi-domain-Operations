using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCTracker : MonoBehaviour
{

    float base_ally_x = -40.0f;
    float base_ally_y = -18.0f;

    float base_enemy_x = 318.0f;
    float base_enemy_y = -18.0f;

    public GameObject Drawing_base;

    int max_NPCs = 5;
    int fontSize = 16;
    float textHeight = 25f;
    float textWidth = 160f;

    int ally_normal_NPC_count = 2;
    int enemy_normal_NPC_count = 2;

    GameObject normalAllyNPCCountObj;
    Text normalAllyNPCCountText;
    RectTransform normalAllyNPCCountRT;

    GameObject normalEnemyNPCCountObj;
    Text normalEnemyNPCCountText;
    RectTransform normalEnemyNPCCountRT;

    // Start is called before the first frame update
    void Start()
    {
        // CREATE THE TEXT FOR THE ALLY NORMAL NPCs
        // create_text(normalAllyNPCCountObj, normalAllyNPCCountText, normalAllyNPCCountRT, base_ally_x, base_ally_y);
        normalAllyNPCCountObj = new GameObject();
        normalAllyNPCCountObj.transform.parent = Drawing_base.transform;
        normalAllyNPCCountObj.name = "Normal_Ally_NPC_Count_Obj";

        normalAllyNPCCountText = normalAllyNPCCountObj.AddComponent<Text>();
        normalAllyNPCCountText.text = ally_normal_NPC_count.ToString();
        normalAllyNPCCountText.font = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
        normalAllyNPCCountText.fontSize = fontSize;
        normalAllyNPCCountText.color = Color.black;

        normalAllyNPCCountRT = normalAllyNPCCountText.GetComponent<RectTransform>();
        normalAllyNPCCountRT.localPosition = new Vector3(base_ally_x, base_ally_y, 0);
        normalAllyNPCCountRT.sizeDelta = new Vector2(textWidth, textHeight);

        // CREATE THE TEXT FOR THE ENEMY NORMAL NPCs
        // create_text(normalEnemyNPCCountObj, normalEnemyNPCCountText, normalEnemyNPCCountRT, base_enemy_x, base_enemy_y);
        normalEnemyNPCCountObj = new GameObject();
        normalEnemyNPCCountObj.transform.parent = Drawing_base.transform;
        normalEnemyNPCCountObj.name = "Normal_Enemy_NPC_Count_Obj";

        normalEnemyNPCCountText = normalEnemyNPCCountObj.AddComponent<Text>();
        normalEnemyNPCCountText.text = enemy_normal_NPC_count.ToString();
        normalEnemyNPCCountText.font = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
        normalEnemyNPCCountText.fontSize = fontSize;
        normalEnemyNPCCountText.color = Color.black;

        normalEnemyNPCCountRT = normalEnemyNPCCountText.GetComponent<RectTransform>();
        normalEnemyNPCCountRT.localPosition = new Vector3(base_enemy_x, base_enemy_y, 0);
        normalEnemyNPCCountRT.sizeDelta = new Vector2(textWidth, textHeight);

    }

    private void create_text(GameObject obj, Text text, RectTransform RT, float x, float y)
    {
        obj = new GameObject();
        obj.transform.parent = Drawing_base.transform;
        obj.name = "Normal_Ally_NPC_Count_Obj";

        text = obj.AddComponent<Text>();
        text.text = ally_normal_NPC_count.ToString();
        text.font = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
        text.fontSize = fontSize;
        text.color = Color.black;

        RT = text.GetComponent<RectTransform>();
        RT.localPosition = new Vector3(x, y, 0);
        RT.sizeDelta = new Vector2(textWidth, textHeight);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void addAllyNPC(string t)
    {
        // t is string type for behavior
        // f is string type for faction, a for ally, n for neutral, e for enemy
        if (ally_normal_NPC_count < max_NPCs)
        {
            ally_normal_NPC_count++;
            Debug.Log(ally_normal_NPC_count);
            normalAllyNPCCountText.text = ally_normal_NPC_count.ToString();
        }

    }

    public void addEnemyNPC(string t)
    {
        // t is string type for behavior
        // f is string type for faction, a for ally, n for neutral, e for enemy
        if (enemy_normal_NPC_count < max_NPCs+1)
        {
            enemy_normal_NPC_count++;
            normalEnemyNPCCountText.text = enemy_normal_NPC_count.ToString();
        }

    }

    public void remAllyNPC(string t)
    {
        // t is string type for behavior
        // f is string type for faction, a for ally, n for neutral, e for enemy
        if (ally_normal_NPC_count > 0)
        {
            ally_normal_NPC_count--;
            normalAllyNPCCountText.text = ally_normal_NPC_count.ToString();
        }

    }

    public void remEnemyNPC(string t)
    {
        // t is string type for behavior
        // f is string type for faction, a for ally, n for neutral, e for enemy
        if (enemy_normal_NPC_count > 0)
        {
            enemy_normal_NPC_count--;
            normalEnemyNPCCountText.text = enemy_normal_NPC_count.ToString();
        }

    }
}

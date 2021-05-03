using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NPCTracker : MonoBehaviour
{

    float base_ally_x = -112.0f;
    float base_ally_y = -18.0f;

    float base_enemy_x = 242.0f;
    float base_enemy_y = -18.0f;

    public GameObject Drawing_base;

    int max_NPCs = 5;
    int fontSize = 10;
    float textHeight = 25f;
    float textWidth = 15f;

    public int ally_normal_NPC_count = 2;
    public int enemy_normal_NPC_count = 2;

    private bool created = false;
    private bool spawned_units = false;

    GameObject normalAllyNPCCountObj;
    Text normalAllyNPCCountText;
    RectTransform normalAllyNPCCountRT;

    GameObject normalEnemyNPCCountObj;
    Text normalEnemyNPCCountText;
    RectTransform normalEnemyNPCCountRT;

    public GameObject normalAllyUnit;
    public GameObject normalEnemyUnit;

    // Start is called before the first frame update
    void Start()
    {
        if (!created)
        {
            DontDestroyOnLoad(this.gameObject);
            created = true;
        }

        if (SceneManager.GetActiveScene().name == "main_menu_scene")
        {

            // CREATE THE TEXT FOR THE ALLY NORMAL NPCs
            //create_text(this.normalAllyNPCCountObj, this.normalAllyNPCCountText, this.normalAllyNPCCountRT, this.base_ally_x, this.base_ally_y);
            this.normalAllyNPCCountObj = new GameObject();
            this.normalAllyNPCCountObj.transform.parent = this.Drawing_base.transform;
            this.normalAllyNPCCountObj.name = "Normal_Ally_NPC_Count_Obj";

            this.normalAllyNPCCountText = this.normalAllyNPCCountObj.AddComponent(typeof(Text)) as Text;
            this.normalAllyNPCCountText.text = this.ally_normal_NPC_count.ToString();
            this.normalAllyNPCCountText.font = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
            this.normalAllyNPCCountText.fontSize = fontSize;
            this.normalAllyNPCCountText.color = Color.black;

            this.normalAllyNPCCountRT = this.normalAllyNPCCountText.GetComponent<RectTransform>();
            this.normalAllyNPCCountRT.localPosition = new Vector3(this.base_ally_x, this.base_ally_y, 0);
            this.normalAllyNPCCountRT.sizeDelta = new Vector2(this.textWidth, this.textHeight);

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
        else
        {
            Debug.Log("HELLO");
            Debug.Log(ally_normal_NPC_count.ToString());
            Debug.Log(enemy_normal_NPC_count.ToString());
        }
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
        // Debug.Log(this.normalAllyNPCCountObj);
        if (!spawned_units && SceneManager.GetActiveScene().name == "town")
        {
            for (int i = 0; i < ally_normal_NPC_count; i++)
            {
                Instantiate(normalAllyUnit, new Vector3(58.1f + i, 249.6f, 417.8f), Quaternion.identity);
            }
            for (int i = 0; i < enemy_normal_NPC_count; i++)
            {
                Instantiate(normalEnemyUnit, new Vector3(-277.1f + i, 249.6f, 637.7f), Quaternion.identity);
            }

            spawned_units = true;
        }
    }

    public void addAllyNPC(string t)
    {
        // Debug.Log(this.normalAllyNPCCountObj);
        // t is string type for behavior
        // f is string type for faction, a for ally, n for neutral, e for enemy
        if (this.ally_normal_NPC_count < this.max_NPCs)
        {
            this.ally_normal_NPC_count++;
            this.normalAllyNPCCountText.text = this.ally_normal_NPC_count.ToString();
        }

    }

    public void addEnemyNPC(string t)
    {
        // t is string type for behavior
        // f is string type for faction, a for ally, n for neutral, e for enemy
        if (this.enemy_normal_NPC_count < this.max_NPCs)
        {
            this.enemy_normal_NPC_count++;
            this.normalEnemyNPCCountText.text = this.enemy_normal_NPC_count.ToString();
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
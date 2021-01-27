using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCTracker : MonoBehaviour
{
    List<string> EnemyNPCs = new List<string>();
    List<string> AllyNPCs = new List<string>();

    Dictionary<string, string> behaviorTypes = new Dictionary<string, string>();

    float base_ally_x = -165.7f;
    float base_ally_y = 19.5f;

    float base_enemy_x = 180.0f;
    float base_enemy_y = 19.5f;

    public GameObject Drawing_base;

    int max_NPCs = 5;
    float height_buffer = 5.0f;
    int fontSize = 16;
    float textHeight = 25f;
    float textWidth = 160f;

    // Start is called before the first frame update
    void Start()
    {
        behaviorTypes.Add("n", "Normal");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void addAllyNPC(string t)
    {
        // t is string type for behavior
        // f is string type for faction, a for ally, n for neutral, e for enemy
        if (AllyNPCs.Count < max_NPCs)
        {
            AllyNPCs.Add(t);
            GameObject myText = new GameObject();
            myText.transform.parent = Drawing_base.transform;
            myText.name = t;

            Text text = myText.AddComponent<Text>();
            text.text = behaviorTypes["n"];
            text.font = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
            text.fontSize = fontSize;
            text.color = Color.black;

            RectTransform rectTransform = text.GetComponent<RectTransform>();
            rectTransform.localPosition = new Vector3(base_ally_x, base_ally_y - height_buffer - (textHeight + height_buffer) * AllyNPCs.Count, 0);
            rectTransform.sizeDelta = new Vector2(textWidth, textHeight);
        }

    }

    public void addEnemyNPC(string t)
    {
        // t is string type for behavior
        // f is string type for faction, a for ally, n for neutral, e for enemy
        if (EnemyNPCs.Count < max_NPCs)
        {
            EnemyNPCs.Add(t);
            GameObject myText = new GameObject();
            myText.transform.parent = Drawing_base.transform;
            myText.name = t;

            Text text = myText.AddComponent<Text>();
            text.text = behaviorTypes["n"];
            text.font = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
            text.fontSize = fontSize;
            text.color = Color.black;

            RectTransform rectTransform = text.GetComponent<RectTransform>();
            rectTransform.localPosition = new Vector3(base_enemy_x, base_enemy_y - height_buffer - (textHeight + height_buffer) * EnemyNPCs.Count, 0);
            rectTransform.sizeDelta = new Vector2(textWidth, textHeight);
        }

    }

    public void remAllyNPC(string t)
    {
        // t is string type for behavior
        // f is string type for faction, a for ally, n for neutral, e for enemy
        if (AllyNPCs.Contains(t))
        {
            AllyNPCs.Remove(t);
        }

    }
}

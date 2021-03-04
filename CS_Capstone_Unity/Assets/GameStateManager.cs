using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{
    int recorded_normal_enemy_count = 0;
    int recorded_normal_ally_count = 0;

    int total_enemy_forces = 0;
    int total_ally_forces = 0;

    public int ally_points = 0;
    public int enemy_points = 0;

    string game_state = "STARTING";

    GameObject mainCamera;

    public GameObject endgameUI;

    public GameObject normalAllyUnit;
    public GameObject normalEnemyUnit;

    public Text endStateText;



    // Start is called before the first frame update
    void Start()
    {
        this.mainCamera = GameObject.FindGameObjectsWithTag("MainCamera")[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (game_state == "GOING")
        {
            total_enemy_forces = GameObject.FindGameObjectsWithTag("Enemy_NPC").Length;
            total_ally_forces = GameObject.FindGameObjectsWithTag("Ally_NPC").Length;
            if (total_ally_forces == 0 || total_enemy_forces == 0)
            {
                this.complete_game();
            }
        }
        if (game_state == "STARTING")
        {
            recorded_normal_enemy_count = GameObject.FindGameObjectsWithTag("Enemy_NPC").Length;
            recorded_normal_ally_count = GameObject.FindGameObjectsWithTag("Ally_NPC").Length;

            if (recorded_normal_ally_count > 0 && recorded_normal_enemy_count > 0)
            {
                total_ally_forces = recorded_normal_ally_count;
                total_enemy_forces = recorded_normal_enemy_count;
                this.game_state = "GOING";
            }
        }
    }

    public void restart()
    {
        for (int i = 0; i < recorded_normal_ally_count; i++)
        {
            Instantiate(normalAllyUnit, new Vector3(58.1f + i, 249.6f, 417.8f), Quaternion.identity);
        }
        for (int i = 0; i < recorded_normal_enemy_count; i++)
        {
            Instantiate(normalEnemyUnit, new Vector3(-277.1f + i, 249.6f, 637.7f), Quaternion.identity);
        }
        this.endgameUI.SetActive(false);
        this.game_state = "GOING";

    }

    private void complete_game()
    {
        if (total_ally_forces == 0)
        {
            endStateText.text = "DEFEAT";
        }
        else
        {
            endStateText.text = "VICTORY";
        }
        
        this.game_state = "ENDING";
        this.endgameUI.SetActive(true);
    }

    public void back_to_main_menu(string sceneName)
    {
        SceneManager.LoadScene(sceneName);

    }
}

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

    public GameObject endgameUI;

    public GameObject normalAllyUnit;
    public GameObject normalEnemyUnit;

    public Text endStateText;

    private Vector3[] spawn_points = new[] { new Vector3(-277f, 249.6f, 637.5f), new Vector3(-101.1f, 249.6f, 636.5f), new Vector3(-127f, 249.6f, 588.4f),
                                             new Vector3(-180f, 249.6f, 570.8f), new Vector3(-200f, 249.6f, 534.9f), new Vector3(-154f, 249.6f, 509.8f),
                                             new Vector3(-85f, 249.6f, 471.5f), new Vector3(-112f, 249.6f, 422f), new Vector3(-247f, 249.6f, 497.5f),
                                             new Vector3(-250f, 251f, 456.5f), new Vector3(-140.9f, 251f, 497.5f), new Vector3(-198f, 251f, 574.5f),
                                             new Vector3(-200f, 251f, 622.5f), new Vector3(-35.3f, 251f, 555.5f), new Vector3(-34.6f, 251f, 580.5f),
                                             new Vector3(-5.6f, 251f, 564.5f), new Vector3(-25f, 251f, 529.5f), new Vector3(-38.6f, 251f, 504.5f),
                                             new Vector3(-26.1f, 251f, 480.1f)};

    public float cluster_chance = 0.0f;
    public int cluster_size = 0;



    // Start is called before the first frame update
    void Start()
    {
        
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
        GameObject[] alive_normal_enemies = GameObject.FindGameObjectsWithTag("Enemy_NPC");
        GameObject[] alive_normal_allys = GameObject.FindGameObjectsWithTag("Ally_NPC");

        for(int i = 0; i < alive_normal_enemies.Length; i++)
        {
            Destroy(alive_normal_enemies[i]);
        }

        for (int i = 0; i < alive_normal_allys.Length; i++)
        {
            Destroy(alive_normal_allys[i]);
        }


        for (int i = 0; i < recorded_normal_ally_count; i++)
        {
            Instantiate(normalAllyUnit, new Vector3(58.1f, 249.6f, 417.8f + (i * 5)), Quaternion.identity);
        }
        bool cluster_next = (Random.value < cluster_chance);
        int current_cluster_size = 0;
        int rnd_idx = Random.Range(0, spawn_points.Length);
        for (int i = 0; i < recorded_normal_enemy_count; i++)
        {
            if (!cluster_next)
            {
                rnd_idx = Random.Range(0, spawn_points.Length);
                cluster_next = (Random.value < cluster_chance);

            }
            else if (current_cluster_size < cluster_size)
            {
                current_cluster_size = current_cluster_size + 1;
                if (current_cluster_size == cluster_size)
                {
                    cluster_next = (Random.value < cluster_chance);
                    current_cluster_size = 0;
                    Instantiate(normalEnemyUnit, spawn_points[rnd_idx], Quaternion.identity);
                    rnd_idx = Random.Range(0, spawn_points.Length);
                    continue;
                }

            }

            Instantiate(normalEnemyUnit, spawn_points[rnd_idx], Quaternion.identity);

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

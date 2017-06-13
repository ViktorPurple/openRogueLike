using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public float levelStartDelay = 2f;
    public float turnDelay = .1f;
    public static GameManager instance = null;

    //public int playerFoodPoints = 100;
    //public int playerMaxFoodPoints = 100;
    //public int playerHealthPoints = 100;
    //public int playerMaxHealthPoints = 100;

    [HideInInspector]
    public bool playersTurn = true;

    private List<Enemy> enemies;

    private bool enemiesMoving;

    // Use this for initialization
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            if (instance != this)
                Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        enemies = new List<Enemy>();

    }



    // Update is called once per frame
    void Update()
    {
        if (playersTurn || enemiesMoving)
            return;
        StartCoroutine(MoveEnemies());

    }


    //add enemies
    public void AddEnemyToList(Enemy script)
    {
        enemies.Add(script);
    }

    //move Enemies
    IEnumerator MoveEnemies()
    {

        {
            //While enemiesMoving is true player is unable to move.
            enemiesMoving = true;

            //yield return new WaitForSeconds(turnDelay);

            //If there are no enemies spawned (IE in first level):
            if (enemies.Count == 0)
            {
                yield return new WaitForSeconds(turnDelay);
            }

            //Loop through List of Enemy objects.
            for (int i = 1; i < enemies.Count; i++)
            {
                //Call the MoveEnemy function of Enemy at index i in the enemies List.
                enemies[i].Move();

                Debug.Log("+" + i);
                //Wait for Enemy's moveTime before moving next Enemy, 
                yield return null;
            }
           
            enemiesMoving = false;
            playersTurn = true;
        }
    }
}

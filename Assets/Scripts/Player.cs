using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    [SerializeField]
    private Stat healthBar;
    [SerializeField]
    private Stat foodBar;

    public int hp = 30;
    public int food = 50;

    Player() { }

    public void Awake()
    {
        healthBar.Initialize();
        foodBar.Initialize();
    }

    public void Update()
    {
        healthBar.CurrentVal = hp;
        foodBar.CurrentVal = food;
    }

    public void movingDeduct()
    {
        if (food > 0) { food--; }
        else { hp--; }

        healthBar.CurrentVal = hp;
        foodBar.CurrentVal = food;
        gameOver();
    }

    public void gameOver()
    {
        if (hp <= 0)
        {
            Debug.Log("GAMOVER!");
        }
    }

    public void gotHit(int damage)
    {
        hp -= damage;
        healthBar.CurrentVal = hp;
    }

    public void gainHp(int heal)
    {
        hp += heal;
        healthBar.CurrentVal = hp;
    }

    public void gainFood(int foodAmount)
    {
        food += foodAmount;
        foodBar.CurrentVal = food;
    }
}

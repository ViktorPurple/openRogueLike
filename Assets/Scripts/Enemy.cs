using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [SerializeField]
    private float viewAbility;
    [SerializeField]
    private LayerMask blockingLayer;
    [SerializeField]
    private LayerMask playerLayer;
    [SerializeField]
    private LayerMask enemyLayer;
    [SerializeField]
    private int baseDamage;

    // public AudioClip[] enemyAttack;
    private Rigidbody2D enemy;
    private BoxCollider2D boxCollider;

    private Animator animator;
    private Transform target;
    private bool skipMove;
    //moving speed

    public float moveTime = 0.1f;
    private float inverseMoveTime;

    // Use this for initialization
    void Start()
    {
        GameManager.instance.AddEnemyToList(this);
        target = GameObject.FindGameObjectWithTag("Player").transform;
        boxCollider = GetComponent<BoxCollider2D>();
        enemy = GetComponent<Rigidbody2D>();
        inverseMoveTime = 1f / moveTime;
    }


    // Update is called once per frame
    public void Move()
    {

        float x = 0;
        x = target.position.x - transform.position.x < 0 ? -1 : 1;
        x = target.position.x - transform.position.x == 0 ? 0 : x;

        float y = 0;
        y = target.position.y - transform.position.y < 0 ? -1 : 1;
        y = target.position.y - transform.position.y == 0 ? 0 : y;


        //Debug.Log("enemy going: " + x + ", " + y);
        Vector3 end = new Vector3(transform.position.x + x, transform.position.y + y, 0);


        //Debug.Log(cantMove(end) == false ? "nope" : "yep");

        StartCoroutine(SmoothMovement(end));

        return;
    }


    protected IEnumerator SmoothMovement(Vector3 end)
    {
        if (!cantMove(end))
        {
            if (!attackHit(end))
            {
                float remaningDistance = (transform.position - end).sqrMagnitude;

                while (remaningDistance > float.Epsilon)
                {
                    Vector3 newPosition = Vector3.MoveTowards(enemy.position, end, inverseMoveTime * Time.deltaTime);
                    enemy.MovePosition(newPosition);
                    remaningDistance = (transform.position - end).sqrMagnitude;
                    //yield return new WaitForSeconds(moveTime);
                    yield return null;
                }
            }
            else
            {
                Debug.Log("hitted for " + baseDamage);
            }
        }
    }

    private bool cantMove(Vector3 end)
    {
        RaycastHit2D hit;

        boxCollider.enabled = false;
        hit = Physics2D.Linecast(new Vector2(enemy.position.x, enemy.position.y), new Vector2(end.x, end.y), blockingLayer);
        if (!hit)
        {
            hit = Physics2D.Linecast(new Vector2(enemy.position.x, enemy.position.y), new Vector2(end.x, end.y), enemyLayer);
        }
        boxCollider.enabled = true;

        return hit;
    }

    private bool attackHit(Vector3 end)
    {
        RaycastHit2D hit;

        boxCollider.enabled = false;
        hit = Physics2D.Linecast(new Vector2(enemy.position.x, enemy.position.y), new Vector2(end.x, end.y), playerLayer);
        boxCollider.enabled = true;

        return hit;
    }


}

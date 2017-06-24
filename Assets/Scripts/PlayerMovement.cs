using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    //player 
    private Rigidbody2D player;
    //Touch pozition in world
    private Vector3 touchPoz;
    //Touch pozition in points Rounded 
    private Vector2 touchFinalRound;
    //camera
    [SerializeField]
    private Camera cam;
    //time to move
    [SerializeField]
    public float turnDelay = .1f;
    //Player Moving
    private bool playerMoving = false;
    private bool playerStop = false;

    //bars
    [SerializeField]
    private Stat healthBar;
    [SerializeField]
    private Stat foodBar;

    //player stats
    [SerializeField]
    private int food;
    [SerializeField]
    private int hp;
    //battlestats
    [SerializeField]
    private int number;
    [SerializeField]
    private int dice;
    [SerializeField]
    private int crit;

    //moving speed
    public float moveTime = 0.1f;
    private float inverseMoveTime;

    //collision 
    [SerializeField]
    private LayerMask blockingLayer;
    [SerializeField]
    private LayerMask enemyLayer;

    private BoxCollider2D boxCollider;
    //list of walls
    List<Node> blockNodes = new List<Node>();


    //test vars
    [SerializeField]
    private GameObject touchMark;

    private PathFinder path;
    private List<Vector3> pathToFollow;
    // Use this for initialization

    void Start()
    {
        blockNodes = gameObject.AddComponent<BlockingLayerLoading>().getWalls();
        boxCollider = GetComponent<BoxCollider2D>();
        player = GetComponent<Rigidbody2D>();
        inverseMoveTime = 1f / moveTime;
        path = gameObject.AddComponent<PathFinder>();
        //initialization of bars
        healthBar.Initialize();
        foodBar.Initialize();
    }


    // Update is called once per frame
    void Update()
    {
        healthBar.CurrentVal = hp;
        foodBar.CurrentVal = food;

        if (GameManager.instance.playersTurn)
        {
            if (pathToFollow == null || pathToFollow.Count < 1)
            {
                if (Input.touchCount > 0 && !GameManager.instance.enemiesMoving)
                {
                    Touch whereToGo = Input.touches[0];
                    if (whereToGo.phase == TouchPhase.Began)
                    {

                        //get tap position in points
                        touchPoz = cam.ScreenToWorldPoint(new Vector3(whereToGo.position.x, whereToGo.position.y, 0f));

                        //Vector3 -> Vector 2
                        touchFinalRound = new Vector2(Mathf.Round(touchPoz.x), Mathf.Round(touchPoz.y));

                        if (transform.position.x == touchFinalRound.x && transform.position.y == touchFinalRound.y) { return; }

                        //go = Instantiate(touchMark, touchFinalRound, Quaternion.identity);

                        path = gameObject.AddComponent<PathFinder>();
                        //path = gameObject.AddComponent<PathFinder>();
                        if (path.PathFinderMain(player.position, youTouchWall(touchFinalRound)))
                        {
                            playerStop = false;
                            pathToFollow = path.path;
                            Move1();


                            if (playerStop)
                            {
                                stopCleaner();
                                return;
                            }

                            pathToFollow.RemoveAt(pathToFollow.Count - 1);

                            if (pathToFollow.Count == 0)
                            {
                                playerMoving = false;
                                GameManager.instance.playersTurn = false;
                            }
                        }
                    }
                }
            }
            else
            {
                Move1();
                //Debug.Log(pathToFollow[pathToFollow.Count - 1]);
                if (playerStop)
                {
                    stopCleaner();
                    return;
                }

                
                pathToFollow.RemoveAt(pathToFollow.Count - 1);

                if (pathToFollow.Count == 0)
                {
                    playerMoving = false;
                    GameManager.instance.playersTurn = false;
                    stopCleaner();
                }
            }
        }
    }

    //private void Move()
    //{
    //    if (path.PathFinderMain(player.position, youTouchWall(touchFinalRound)))
    //    {

    //        StartCoroutine(MovePlayerByPath(path.path));
    //    }
    //    else
    //    {
    //        Destroy(go);
    //    }


    //    //StartCoroutine(SmoothMovePlayer(touchFinalRound));
    //    Destroy(GetComponent<PathFinder>());
    //    Destroy(GetComponent<BlockingLayerLoading>());
    //    Destroy(GetComponent<BlockingLayerLoading>());
    //}

    private void Move1()
    {
        //path = gameObject.AddComponent<PathFinder>();
        StartCoroutine(SmoothMovePlayer(pathToFollow[pathToFollow.Count - 1]));
        movingDeduct();
        
        //StartCoroutine(SmoothMovePlayer(touchFinalRound));
        //Destroy(GetComponent<PathFinder>());
        //Destroy(GetComponent<BlockingLayerLoading>());
        //Destroy(GetComponent<BlockingLayerLoading>());
    }

    private IEnumerator SmoothMovePlayer(Vector3 end)
    {
        while (player.position.x - end.x != 0 || player.position.y - end.y != 0)
        {
            Vector3 nextStep = end;
            // Debug.Log(cantMove(nextStep) == false ? "nope" : "yep");
            if (!cantMove(nextStep))
            {
                float remaningDistance = (transform.position - nextStep).sqrMagnitude;

                while (remaningDistance > float.Epsilon)
                {
                    playerMoving = true;
                    Vector3 newPosition = Vector3.MoveTowards(player.position, nextStep, inverseMoveTime * Time.deltaTime);
                    player.MovePosition(newPosition);
                    remaningDistance = (transform.position - nextStep).sqrMagnitude;

                    GameManager.instance.playersTurn = false;
                    playerMoving = false;
                    yield return null;
                }
            }
            else
            {
                playerStop = true;
                if (hitEnemy(nextStep))
                {
                    GameManager.instance.hitEnemyOnPosition(damageCalc(number, dice), nextStep, critHappens(crit));
                }
                yield return null;
                break;
            }
        }

        // playerMoving = false;

    }

    private Vector3 calculateNextCell(Vector3 end)
    {
        float x = 0;
        x = player.position.x - end.x > 0 ? -1 : 1;
        x = player.position.x - end.x == 0 ? 0 : x;

        float y = 0;
        y = player.position.y - end.y > 0 ? -1 : 1;
        y = player.position.y - end.y == 0 ? 0 : y;

        return new Vector3(player.position.x + x, player.position.y + y, 0); ;
    }

    private bool cantMove(Vector3 end)
    {
        RaycastHit2D hit;

        boxCollider.enabled = false;
        hit = Physics2D.Linecast(new Vector2(player.position.x, player.position.y), new Vector2(end.x, end.y), blockingLayer);
        boxCollider.enabled = true;

        return hit;
    }


    private bool hitEnemy(Vector3 end)
    {
        RaycastHit2D hit;

        boxCollider.enabled = false;
        hit = Physics2D.Linecast(new Vector2(player.position.x, player.position.y), new Vector2(end.x, end.y), enemyLayer);
        boxCollider.enabled = true;

        return hit;
    }

    //private IEnumerator MovePlayerByPath(List<Vector3> path)
    //{

    //    //yield return new WaitForSeconds(turnDelay);
    //    //while (player.position.x - end.x != 0 || player.position.y - end.y != 0)
    //    for (int i = path.Count - 1; i >= 0; i--)
    //    {
    //        Vector3 nextStep = path[i];
    //        if (GameManager.instance.playersTurn == true)
    //        {
    //            Debug.Log("Player turn");
    //            if (!cantMove(nextStep))
    //            {
    //                float remaningDistance = (transform.position - nextStep).sqrMagnitude;

    //                while (remaningDistance > float.Epsilon)
    //                {
    //                    playerMoving = true;
    //                    Vector3 newPosition = Vector3.MoveTowards(player.position, nextStep, inverseMoveTime * Time.deltaTime);
    //                    player.MovePosition(newPosition);
    //                    remaningDistance = (transform.position - nextStep).sqrMagnitude;
    //                    yield return null;
    //                    //yield return new WaitForSeconds(turnDelay);
    //                }
    //            }
    //            else
    //            {
    //                yield return null;
    //                break;
    //            }
    //        }

    //        yield return new WaitForSeconds(turnDelay);
    //        GameManager.instance.playersTurn = false;
    //        //yield return null;

    //    }

    //    playerMoving = false;
    //    Destroy(go);
    //}

    //check if tap on wall
    private Vector2 youTouchWall(Vector2 end)
    {
        foreach (Node node in blockNodes)
        {
            if (end.x == node.x && end.y == node.y)
            {
                float x = 0;
                x = player.position.x - end.x > 0 ? -1 : 1;
                x = player.position.x - end.x == 0 ? 0 : x;

                float y = 0;
                y = player.position.y - end.y > 0 ? -1 : 1;
                y = player.position.y - end.y == 0 ? 0 : y;

                return new Vector3(end.x - x, end.y - y, 0);
            }
        }
        return end;
    }

    private void stopCleaner()
    {
        pathToFollow = null;
        playerMoving = false;
        GameManager.instance.playersTurn = false;
        Destroy(GetComponent<PathFinder>());
        Destroy(GetComponent<BlockingLayerLoading>());
        Destroy(GetComponent<BlockingLayerLoading>());
    }


    public void movingDeduct()
    {
        if (food > 0) { food--; }
        else { hp--; }

        gameOver();
    }

    public void gameOver()
    {
        if (hp <= 0)
        {
            Debug.Log("GAMOVER!");
        }
    }

    public void gotHit(int damage, bool crit)
    {
        if (crit)
        {
            FloatingTextController.CreateFloatingText((2 * damage).ToString(), transform, Color.red);
            hp -= 2 * damage;
        }
        else
        {
            FloatingTextController.CreateFloatingText(damage.ToString(), transform, Color.yellow);
            hp -= damage;
        }
       
        gameOver();
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


    //damage section
    private int damageCalc(int number, int dice)
    {

        int damage = 0;
        for (int i = 0; i < number; i++)
        {
            damage += Random.Range(1, dice);
        }

        return damage;
    }

    private bool critHappens(int crit)
    {
        return (Random.Range(0, 100) <= crit);
    }
}

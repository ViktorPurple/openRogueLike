using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

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
    bool playerMoving = false;

    //public variables (for now)
    //moving speed
    public float moveTime = 0.1f;
    private float inverseMoveTime;

    //collision 
    [SerializeField]
    private LayerMask blockingLayer;
    private BoxCollider2D boxCollider;
    //list of walls
    List<Node> blockNodes = new List<Node>();

    //test vars
    [SerializeField]
    private GameObject touchMark;
    [SerializeField]
    private GameObject go;

    // Use this for initialization
    void Start() {
        blockNodes = gameObject.AddComponent<BlockingLayerLoading>().getWalls();
        boxCollider = GetComponent<BoxCollider2D>();
        player = GetComponent<Rigidbody2D>();
        inverseMoveTime = 1f / moveTime;
    }

    // Update is called once per frame
    void Update() {
        if (GameManager.instance.playersTurn)
        {
            if (Input.touchCount > 0 && !playerMoving)
            {
                Touch whereToGo = Input.touches[0];
                if (whereToGo.phase == TouchPhase.Began)
                {

                    //get tap position in points
                    touchPoz = cam.ScreenToWorldPoint(new Vector3(whereToGo.position.x, whereToGo.position.y, 0f));

                    //Vector3 -> Vector 2
                    touchFinalRound = new Vector2(Mathf.Round(touchPoz.x), Mathf.Round(touchPoz.y));


                    go = Instantiate(touchMark, touchFinalRound, Quaternion.identity);


                    Move();
                    
                }
            }
        }
    }

    private void Move()
    {
        PathFinder path = gameObject.AddComponent<PathFinder>();
        if (path.PathFinderMain(player.position, youTouchWall(touchFinalRound)))
        {
            StartCoroutine(MovePlayerByPath(path.path));            
        }
        else
        {
            Destroy(go);
        }


        //StartCoroutine(SmoothMovePlayer(touchFinalRound));
        Destroy(GetComponent<PathFinder>());
        Destroy(GetComponent<BlockingLayerLoading>());
        Destroy(GetComponent<BlockingLayerLoading>());
    }

    //private IEnumerator SmoothMovePlayer(Vector3 end)
    //{
        

    //    while (player.position.x - end.x != 0 || player.position.y - end.y != 0)
    //    {
            

    //        Vector3 nextStep = calculateNextCell(end);
         
    //       // Debug.Log(cantMove(nextStep) == false ? "nope" : "yep");
    //        if (!cantMove(nextStep))
    //        {
    //            float remaningDistance = (transform.position - nextStep).sqrMagnitude;

    //            while (remaningDistance > float.Epsilon)
    //            {
    //                playerMoving = true;
    //                Vector3 newPosition = Vector3.MoveTowards(player.position, nextStep, inverseMoveTime * Time.deltaTime);
    //                player.MovePosition(newPosition);
    //                remaningDistance = (transform.position - nextStep).sqrMagnitude;
    //                yield return null;
    //            }
    //        } else
    //        {
    //            yield return null;
    //            break;
    //        }
    //    }

    //    playerMoving = false;
    //    Destroy(go);
    //}

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

    private IEnumerator MovePlayerByPath(List<Vector3> path)
    {

        //yield return new WaitForSeconds(turnDelay);
        //while (player.position.x - end.x != 0 || player.position.y - end.y != 0)
        for (int i = path.Count - 1; i >= 0; i--)
        {
            Vector3 nextStep = path[i];

            Debug.Log("Player turn");
            if (!cantMove(nextStep))
            {
                float remaningDistance = (transform.position - nextStep).sqrMagnitude;

                while (remaningDistance > float.Epsilon)
                {
                    playerMoving = true;
                    Vector3 newPosition = Vector3.MoveTowards(player.position, nextStep, inverseMoveTime * Time.deltaTime);
                    player.MovePosition(newPosition);
                    remaningDistance = (transform.position - nextStep).sqrMagnitude;
                    yield return null;
                    //yield return new WaitForSeconds(turnDelay);
                }
            }
            else
            {
                yield return null;
                break;
            }

            GameManager.instance.playersTurn = false;
            //yield return null;
            yield return new WaitForSeconds(turnDelay);
        }

        playerMoving = false;
        Destroy(go);
    }

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
}

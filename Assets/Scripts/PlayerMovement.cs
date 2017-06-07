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

    //test vars
    [SerializeField]
    private GameObject touchMark;
    [SerializeField]
    private GameObject pathShit;
    private GameObject go;

    // Use this for initialization
    void Start() {
        boxCollider = GetComponent<BoxCollider2D>();
        player = GetComponent<Rigidbody2D>();
        inverseMoveTime = 1f / moveTime;
    }

    // Update is called once per frame
    void Update() {
        int horizontal = 0;
        int vertical = 0;

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

    private void Move()
    {

        
        StartCoroutine(SmoothMovePlayer(touchFinalRound));
        


    }

    private IEnumerator SmoothMovePlayer(Vector3 end)
    {
        gameObject.AddComponent<PathFinder>().PathFinderMain(player.position, end, pathShit);

        while (player.position.x - end.x != 0 || player.position.y - end.y != 0)
        {
            

            Vector3 nextStep = calculateNextCell(end);
         
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
                    yield return null;
                }
            } else
            {
                yield return null;
                break;
            }
        }

        playerMoving = false;
        Destroy(go);
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


}

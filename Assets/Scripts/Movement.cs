using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public int currentSpeed;
    public int tires = 40;
    public bool moveAllowed; 
    public int movementPoints;
    public int lap = 1;
    

    private Vector3 transformPosition;
    private Coroutine moveCoroutine;

    private bool isMoving;
    private int whosTurn;

    private static UIScript uiScript;
    private int chosenSpeed;
    private int track;

    private Dictionary<int, int> moves = new Dictionary<int, int>()
    {
        { 0, 0 },
        { 40, 1 },
        { 80, 2 },
        { 120, 3 },
        { 160, 4 },
        { 200, 5 },
        { 240, 6 },
        { 280, 7 },
        { 320, 8 }
    };
    
    private void Start()
    {
        transformPosition = gameObject.transform.position; 
        uiScript = FindObjectOfType<UIScript>();
    }
    
    private void Update()
    {
        if (gameObject.name == "Player1") { whosTurn = 1; }
        else { whosTurn = -1; }

        if (!isMoving) {
            if (moveAllowed && moveCoroutine == null)
            {
                if (uiScript.HasChosenSpeed)
                {
                    chosenSpeed = uiScript.ChosenSpeed;
                    moveCoroutine = StartCoroutine(Move());
                }
            } 
        }
    }

    IEnumerator Move() 
    {
        if (isMoving) { yield break; }
        isMoving = true;

        int[] playerParams = SpeedChanging.ChangeParams(chosenSpeed, currentSpeed, tires);

        currentSpeed = playerParams[0];
        tires = playerParams[1];

        movementPoints = moves[currentSpeed];

        // todo 2: który tor mozliwy
        // todo 3: zakręty - obrót i zmiana pozycji

        while (movementPoints > 0)
        {
            // left track
            if (Input.GetAxis("Horizontal") < 0 || Input.GetKeyDown(KeyCode.A))
            {
                if (Quaternion.Equals(transform.rotation.z, 180f))
                {
                    transformPosition.x += 1;
                    transformPosition.y -= 1;
                }
                else if (Quaternion.Equals(transform.rotation.z, 270f))
                {
                    transformPosition.x += 1;
                    transformPosition.y += 1;
                }
                else if (Quaternion.Equals(transform.rotation.z, 0f))
                {
                    transformPosition.x -= 1;
                    transformPosition.y += 1;
                }
                else if (Quaternion.Equals(transform.rotation.z, 90f))
                {
                    transformPosition.x -= 1;
                    transformPosition.y -= 1;
                }

                movementPoints--;
            }
            // right track
            else if (Input.GetAxis("Horizontal") > 0 || Input.GetKeyDown(KeyCode.D))
            {
                if (Quaternion.Equals(transform.rotation.z, 180f))
                {
                    transformPosition.x -= 1;
                    transformPosition.y -= 1;
                }
                else if (Quaternion.Equals(transform.rotation.z, 270f))
                {
                    transformPosition.x += 1;
                    transformPosition.y -= 1;
                }
                else if (Quaternion.Equals(transform.rotation.z, 0f))
                {
                    transformPosition.x += 1;
                    transformPosition.y += 1;
                }
                else if (Quaternion.Equals(transform.rotation.z, 90f))
                {
                    transformPosition.x -= 1;
                    transformPosition.y += 1;
                }

                movementPoints--;
            }
            // current track
            else if (Input.GetAxis("Vertical") > 0 || Input.GetKeyDown(KeyCode.W))
            {
                if (Quaternion.Equals(transform.rotation.z, 180f))
                {
                    transformPosition.y -= 2;
                }
                else if (Quaternion.Equals(transform.rotation.z, 270f))
                {
                    transformPosition.x += 2;
                }
                else if (Quaternion.Equals(transform.rotation.z, 0f))
                {
                    transformPosition.y += 2;
                }
                else if (Quaternion.Equals(transform.rotation.z, 90f))
                {
                    transformPosition.x -= 2;
                }

                movementPoints--;
            }

            while (NextField(transformPosition)) { yield return null; }

            yield return new WaitForSeconds(0.1f);
        }

        whosTurn = -whosTurn;
        isMoving = false;
        
        if (whosTurn == 1) { GameMaster.MovePlayer(1); }
        else if (whosTurn == -1) { GameMaster.MovePlayer(2); }

        moveCoroutine = null;
    }
    
    bool NextField(Vector3 dest) {
        return dest != (transform.position = Vector3.MoveTowards(transform.position, 
            dest, 4*Time.deltaTime)); }
    
    
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.name);
        if (other.name == "Trigger1_turn2")
        { 
            transform.Rotate(0f, 0f, 90f);
            Debug.Log("trigger");
        }
    }
}

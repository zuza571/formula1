using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public int currentSpeed;
    public int tires;
    public bool moveAllowed; 
    public int movementPoints;
    public int lap;
    

    private Vector3 transformPosition;
    private Coroutine moveCoroutine;

    private bool isMoving;
    private int whosTurn;
    private bool handledDiceRoll;

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
        lap = 0;
        tires = GameParams.tires;
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

    // TODO: nie można wjeżdzać w innego gracza
    IEnumerator Move() 
    {
        if (isMoving) { yield break; }
        isMoving = true;

        int[] playerParams = SpeedChanging.ChangeParams(chosenSpeed, currentSpeed, tires);

        currentSpeed = playerParams[0];
        tires = playerParams[1];

        movementPoints = moves[currentSpeed];
        PanelUIMainMenuScript.CurrentTires = tires;

        // todo: który tor możliwy - podświetlane pola
        // todo: wypadanie poza mapę
        
        while (movementPoints > 0)
        {
            transformPosition = gameObject.transform.position; 
            float rotationZ = gameObject.transform.eulerAngles.z;

            // left track
            if (Input.GetAxis("Horizontal") < 0 || Input.GetKeyDown(KeyCode.A))
            {
                if (Quaternion.Equals(rotationZ, 180f))
                {
                    transformPosition.x -= 1;
                    transformPosition.y += 1;
                }
                else if (Quaternion.Equals(rotationZ, 270f))
                {
                    transformPosition.x -= 1;
                    transformPosition.y -= 1;
                }
                else if (Quaternion.Equals(rotationZ, 0f))
                {
                    transformPosition.x += 1;
                    transformPosition.y -= 1;
                }
                else if (Quaternion.Equals(rotationZ, 90f))
                {
                    transformPosition.x += 1;
                    transformPosition.y += 1;
                }

                if (transformPosition != gameObject.transform.position)
                {
                    movementPoints--; 
                }
            }
            // right track
            else if (Input.GetAxis("Horizontal") > 0 || Input.GetKeyDown(KeyCode.D))
            {
                if (Quaternion.Equals(rotationZ, 180f))
                {
                    transformPosition.x += 1;
                    transformPosition.y += 1;
                }
                else if (Quaternion.Equals(rotationZ, 270f))
                {
                    transformPosition.x -= 1;
                    transformPosition.y += 1;
                }
                else if (Quaternion.Equals(rotationZ, 0f))
                {
                    transformPosition.x -= 1;
                    transformPosition.y -= 1;
                }
                else if (Quaternion.Equals(rotationZ, 90f))
                {
                    transformPosition.x += 1;
                    transformPosition.y -= 1;
                }

                if (transformPosition != gameObject.transform.position)
                {
                    movementPoints--; 
                }
            }
            // current track
            else if (Input.GetAxis("Vertical") > 0 || Input.GetKeyDown(KeyCode.W))
            {
                if (Quaternion.Equals(rotationZ, 180f))
                {
                    transformPosition.y += 2;
                }
                else if (Quaternion.Equals(rotationZ, 270f))
                {
                    transformPosition.x -= 2;
                }
                else if (Quaternion.Equals(rotationZ, 0f))
                {
                    transformPosition.y -= 2;
                }
                else if (Quaternion.Equals(rotationZ, 90f))
                {
                    transformPosition.x += 2;
                }

                if (transformPosition != gameObject.transform.position)
                {
                    movementPoints--; 
                }
            }
            
            // turning 
            if (Math.Abs(transformPosition.y - 11f) < 0.05 && Math.Abs(transformPosition.x + 27.5f) < 0.05)
            {
                // verify speed
                if (currentSpeed > 120)
                {
                    int diceRollCount = (currentSpeed - 120) / 40;
                    RollTheDice(diceRollCount);
                }
                
                transformPosition.y = 11.5f;
                transformPosition.x = -27f;
                transform.Rotate(0,0,-90);
            }
            if (Math.Abs(transformPosition.y - 12f) < 0.05 && Math.Abs(transformPosition.x + 28.5f) < 0.05)
            {
                // verify speed
                if (currentSpeed > 160)
                {
                    int diceRollCount = (currentSpeed - 160) / 40;
                    RollTheDice(diceRollCount);
                }
                
                transformPosition.y = 12.5f;
                transformPosition.x = -28f;
                transform.Rotate(0,0,-90);
            }
            if (Math.Abs(transformPosition.y - 13f) < 0.05 && Math.Abs(transformPosition.x + 29.5f) < 0.05)
            {
                // verify speed
                if (currentSpeed > 200)
                {
                    int diceRollCount = (currentSpeed - 200) / 40;
                    RollTheDice(diceRollCount);
                }
                
                transformPosition.y = 13.5f;
                transformPosition.x = -29f;
                transform.Rotate(0,0,-90);
            }
            
            if (Math.Abs(transformPosition.y - 11.5f) < 0.05 && Math.Abs(transformPosition.x + 17f) < 0.05)
            {
                // verify speed
                if (currentSpeed > 120)
                {
                    int diceRollCount = (currentSpeed - 120) / 40;
                    RollTheDice(diceRollCount);
                }
                
                transformPosition.y = 11f;
                transformPosition.x = -16.5f;
                transform.Rotate(0,0,-90);
            }
            if (Math.Abs(transformPosition.y - 12.5f) < 0.05 && Math.Abs(transformPosition.x + 16f) < 0.05)
            {
                // verify speed
                if (currentSpeed > 160)
                {
                    int diceRollCount = (currentSpeed - 120) / 40;
                    RollTheDice(diceRollCount);
                }
                
                transformPosition.y = 12f;
                transformPosition.x = -15.5f;
                transform.Rotate(0,0,-90);
            }
            if (Math.Abs(transformPosition.y - 13.5f) < 0.05 && Math.Abs(transformPosition.x + 15f) < 0.05)
            {
                // verify speed
                if (currentSpeed > 200)
                {
                    int diceRollCount = (currentSpeed - 120) / 40;
                    RollTheDice(diceRollCount);
                }
                
                transformPosition.y = 13f;
                transformPosition.x = -14.5f;
                transform.Rotate(0,0,-90);
            }
            
            if (Math.Abs(transformPosition.y - 5f) < 0.05 && Math.Abs(transformPosition.x + 16.5f) < 0.05)
            {
                // verify speed
                if (currentSpeed > 120)
                {
                    int diceRollCount = (currentSpeed - 120) / 40;
                    RollTheDice(diceRollCount);
                }
                
                transformPosition.y = 4.5f;
                transformPosition.x = -16f;
                transform.Rotate(0,0,90);
            }
            if (Math.Abs(transformPosition.y - 6f) < 0.05 && Math.Abs(transformPosition.x + 15.5f) < 0.05)
            {
                // verify speed
                if (currentSpeed > 160)
                {
                    int diceRollCount = (currentSpeed - 120) / 40;
                    RollTheDice(diceRollCount);
                }
                
                transformPosition.y = 5.5f;
                transformPosition.x = -15f;
                transform.Rotate(0,0,90);
            }
            if (Math.Abs(transformPosition.y - 7f) < 0.05 && Math.Abs(transformPosition.x + 14.5f) < 0.05)
            {
                // verify speed
                if (currentSpeed > 200)
                {
                    int diceRollCount = (currentSpeed - 120) / 40;
                    RollTheDice(diceRollCount);
                }
                
                transformPosition.y = 6.5f;
                transformPosition.x = -14f;
                transform.Rotate(0,0,90);
            }
            
            if (Math.Abs(transformPosition.y - 4.5f) < 0.05 && Math.Abs(transformPosition.x + 4f) < 0.05)
            {
                // verify speed
                if (currentSpeed > 120)
                {
                    int diceRollCount = (currentSpeed - 120) / 40;
                    RollTheDice(diceRollCount);
                }
                transformPosition.y = 4f;
                transformPosition.x = -3.5f;
                transform.Rotate(0,0,-90);
            }
            if (Math.Abs(transformPosition.y - 5.5f) < 0.05 && Math.Abs(transformPosition.x + 3f) < 0.05)
            {
                // verify speed
                if (currentSpeed > 160)
                {
                    int diceRollCount = (currentSpeed - 120) / 40;
                    RollTheDice(diceRollCount);
                }
                transformPosition.y = 5f;
                transformPosition.x = -2.5f;
                transform.Rotate(0,0,-90);
            }
            if (Math.Abs(transformPosition.y - 6.5f) < 0.05 && Math.Abs(transformPosition.x + 2f) < 0.05)
            {
                // verify speed
                if (currentSpeed > 200)
                {
                    int diceRollCount = (currentSpeed - 120) / 40;
                    RollTheDice(diceRollCount);
                }
                
                transformPosition.y = 6f;
                transformPosition.x = -1.5f;
                transform.Rotate(0,0,-90);
            }
            
            if (Math.Abs(transformPosition.y + 22f) < 0.05 && Math.Abs(transformPosition.x + 3.5f) < 0.05)
            {
                // verify speed
                if (currentSpeed > 120)
                {
                    int diceRollCount = (currentSpeed - 120) / 40;
                    RollTheDice(diceRollCount);
                }
                
                transformPosition.y = -22.5f;
                transformPosition.x = -4f;
                transform.Rotate(0,0,-90);
            }
            if (Math.Abs(transformPosition.y + 23f) < 0.05 && Math.Abs(transformPosition.x + 2.5f) < 0.05)
            {
                // verify speed
                if (currentSpeed > 160)
                {
                    int diceRollCount = (currentSpeed - 120) / 40;
                    RollTheDice(diceRollCount);
                }
                
                transformPosition.y = -23.5f;
                transformPosition.x = -3f;
                transform.Rotate(0,0,-90);
            }
            if (Math.Abs(transformPosition.y + 24f) < 0.05 && Math.Abs(transformPosition.x + 1.5f) < 0.05)
            {
                // verify speed
                if (currentSpeed > 200)
                {
                    int diceRollCount = (currentSpeed - 120) / 40;
                    RollTheDice(diceRollCount);
                }
                
                transformPosition.y = -24.5f;
                transformPosition.x = -2f;
                transform.Rotate(0,0,-90);
            }
            
            if (Math.Abs(transformPosition.y + 22.5f) < 0.05 && Math.Abs(transformPosition.x + 16f) < 0.05)
            {
                // verify speed
                if (currentSpeed > 120)
                {
                    int diceRollCount = (currentSpeed - 120) / 40;
                    RollTheDice(diceRollCount);
                }
                
                transformPosition.y = -22f;
                transformPosition.x = -16.5f;
                transform.Rotate(0,0,-90);
            }
            if (Math.Abs(transformPosition.y + 23.5f) < 0.05 && Math.Abs(transformPosition.x + 17f) < 0.05)
            {
                // verify speed
                if (currentSpeed > 160)
                {
                    int diceRollCount = (currentSpeed - 120) / 40;
                    RollTheDice(diceRollCount);
                }
                
                transformPosition.y = -23f;
                transformPosition.x = -17.5f;
                transform.Rotate(0,0,-90);
            }
            if (Math.Abs(transformPosition.y + 24.5f) < 0.05 && Math.Abs(transformPosition.x + 18f) < 0.05)
            {
                // verify speed
                if (currentSpeed > 200)
                {
                    int diceRollCount = (currentSpeed - 120) / 40;
                    RollTheDice(diceRollCount);
                }
                
                transformPosition.y = -24f;
                transformPosition.x = -18.5f;
                transform.Rotate(0,0,-90);
            }
            
            if (Math.Abs(transformPosition.y + 14f) < 0.05 && Math.Abs(transformPosition.x + 16.5f) < 0.05)
            {
                // verify speed
                if (currentSpeed > 120)
                {
                    int diceRollCount = (currentSpeed - 120) / 40;
                    RollTheDice(diceRollCount);
                }
                
                transformPosition.y = -13.5f;
                transformPosition.x = -17f;
                transform.Rotate(0,0,90);
            }
            if (Math.Abs(transformPosition.y + 15f) < 0.05 && Math.Abs(transformPosition.x + 17.5f) < 0.05)
            {
                // verify speed
                if (currentSpeed > 160)
                {
                    int diceRollCount = (currentSpeed - 120) / 40;
                    RollTheDice(diceRollCount);
                }
                
                transformPosition.y = -14.5f;
                transformPosition.x = -18f;
                transform.Rotate(0,0,90);
            }
            if (Math.Abs(transformPosition.y + 16f) < 0.05 && Math.Abs(transformPosition.x + 18.5f) < 0.05)
            {
                // verify speed
                if (currentSpeed > 200)
                {
                    int diceRollCount = (currentSpeed - 120) / 40;
                    RollTheDice(diceRollCount);
                }
                
                transformPosition.y = -15.5f;
                transformPosition.x = -19f;
                transform.Rotate(0,0,90);
            }
            
            if (Math.Abs(transformPosition.y + 13.5f) < 0.05 && Math.Abs(transformPosition.x + 27f) < 0.05)
            {
                // verify speed
                if (currentSpeed > 120)
                {
                    int diceRollCount = (currentSpeed - 120) / 40;
                    RollTheDice(diceRollCount);
                }
                
                transformPosition.y = -13f;
                transformPosition.x = -27.5f;
                transform.Rotate(0,0,-90);
            }
            if (Math.Abs(transformPosition.y + 14.5f) < 0.05 && Math.Abs(transformPosition.x + 28f) < 0.05)
            {
                // verify speed
                if (currentSpeed > 160)
                {
                    int diceRollCount = (currentSpeed - 120) / 40;
                    RollTheDice(diceRollCount);
                }
                
                transformPosition.y = -14f;
                transformPosition.x = -28.5f;
                transform.Rotate(0,0,-90);
            }
            if (Math.Abs(transformPosition.y + 15.5f) < 0.05 && Math.Abs(transformPosition.x + 29f) < 0.05)
            {
                // verify speed
                if (currentSpeed > 200)
                {
                    int diceRollCount = (currentSpeed - 120) / 40;
                    RollTheDice(diceRollCount);
                }
                
                transformPosition.y = -15f;
                transformPosition.x = -29.5f;
                transform.Rotate(0,0,-90);
            }

            GameMaster.UpdateLaps(transformPosition);
            
            // map edges
            if (!((transformPosition.y > -15.9 && transformPosition.y < 13.9 && transformPosition.x > -30.4 && transformPosition.x < -26.6) 
                || (transformPosition.y > 10.6 && transformPosition.y < 14.4 && transformPosition.x > -30 && transformPosition.x < -14)
                || (transformPosition.y > 4.1 && transformPosition.y < 13.9 && transformPosition.x > -17.4 && transformPosition.x < -13.6)
                || (transformPosition.y > 3.9 && transformPosition.y < 7.4 && transformPosition.x > -17.4 && transformPosition.x < -0.6)
                || (transformPosition.y > -24.9 && transformPosition.y < 6.9 && transformPosition.x > -4.4 && transformPosition.x < -0.6)
                || (transformPosition.y > -24.9 && transformPosition.y < -22.1 && transformPosition.x > -19.9 && transformPosition.x < -0.6)
                || (transformPosition.y > -24.9 && transformPosition.y < -13.1 && transformPosition.x > -19.4 && transformPosition.x < -15.6)
                || (transformPosition.y > -16.4 && transformPosition.y < -13.4 && transformPosition.x > -30.4 && transformPosition.x < -15.6)))
            {
                Debug.Log("chujowo jedziesz");
                transformPosition = transform.position;
                movementPoints++;
            }

            // get other player's position
            Vector3 otherPlayerPosition = GameMaster.OtherPlayerPosition(whosTurn);
            float distance = Vector3.Distance(transformPosition, otherPlayerPosition);

            if (Mathf.Abs(distance) < 0.05f)
            {
                Debug.Log("inny gracz tu jest");
                Debug.Log(GameMaster.OtherPlayerPosition(whosTurn));
                Debug.Log(gameObject.transform.position);
                transformPosition = transform.position;
                movementPoints++;
            }

            while (NextField(transformPosition)) { yield return null; }
            
            yield return new WaitForSeconds(0.1f);
        }

        whosTurn = -whosTurn;
        isMoving = false;

        if (whosTurn == 1)
        {
            GameMaster.MovePlayer(1);
            GameMaster.CurrentUIGameMaster();
        }
        else if (whosTurn == -1)
        {
            GameMaster.MovePlayer(2);
            GameMaster.CurrentUIGameMaster();
        }

        moveCoroutine = null;
    }
    
    bool NextField(Vector3 dest) {
        return dest != (transform.position = Vector3.MoveTowards(transform.position, 
            dest, 4*Time.deltaTime)); 
    }

    
    // todo: podczas rzutu kostką nie można się ruszać
    void RollTheDice(int rollCount) { StartCoroutine(RollDiceWithTimeout(rollCount)); }
    private IEnumerator RollDiceWithTimeout(int rollCount)
    {
        // disable movement
        gameObject.transform.Rotate(0,0,0.01f);
        while (rollCount > 0)
        {
            Dice diceScript = FindObjectOfType<Dice>();
            if (diceScript != null)
            {
                StartCoroutine(diceScript.RollTheDice());
                yield return new WaitUntil(() => diceScript.RandomDiceSlide > 0);
                int result = diceScript.RandomDiceSlide;
                Debug.Log("Dice result: " + result);

                int[] playerParams;
                switch (result)
                {
                    case 1:
                        tires -= 1;
                        if (tires < 0)
                        {
                            playerParams = SpeedChanging.ChangeParams(0, currentSpeed, tires);
                            currentSpeed = playerParams[0];
                            // reset movement points to 0
                            movementPoints = 0;
                            // stop rolling the dice
                            rollCount = 0;
                            tires = 0;
                        }
                        PanelUIMainMenuScript.CurrentTires = tires;
                        break;
                    case 2:
                        tires -= 2;
                        if (tires < 0)
                        {
                            playerParams = SpeedChanging.ChangeParams(0, currentSpeed, tires);
                            currentSpeed = playerParams[0];
                            // reset movement points to 0
                            movementPoints = 0;
                            // stop rolling the dice
                            rollCount = 0;
                            tires = 0;
                        }
                        PanelUIMainMenuScript.CurrentTires = tires;
                        break;
                    case 3:
                        tires -= 3;
                        if (tires < 0)
                        {
                            playerParams = SpeedChanging.ChangeParams(0, currentSpeed, tires);
                            currentSpeed = playerParams[0];
                            // reset movement points to 0
                            movementPoints = 0;
                            // stop rolling the dice
                            rollCount = 0;
                            tires = 0;
                        }
                        PanelUIMainMenuScript.CurrentTires = tires;
                        break;
                    case 4:
                        break;
                    case 5:
                        break;
                    case 6:
                        playerParams = SpeedChanging.ChangeParams(0, currentSpeed, tires);
                        currentSpeed = playerParams[0];
                        tires = playerParams[1];
                        // reset movement points to 0
                        movementPoints = 0;
                        // stop rolling the dice
                        rollCount = 0;
                        if (tires < 0)
                        {
                            tires = 0;
                        }
                        PanelUIMainMenuScript.CurrentTires = tires;
                        break;
                }
            }

            yield return new WaitForSeconds(2f);

            rollCount--;

            Debug.Log("roll count " + rollCount);
            Debug.Log("tires " + tires);
            Debug.Log("movement " + movementPoints);
            Debug.Log("--------------------------");
        }
        
        // enable movement after all dice rolls
        gameObject.transform.Rotate(0,0,-0.01f);
    }
}

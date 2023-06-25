using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

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
    private bool handledDiceRoll;

    private static UIScript uiScript;
    private int chosenSpeed;
    private int track;

    private List<int> playerIndices;
    private int currentPlayer;
    private int eachPlayerHasMoved = 0;
    public int playerMoved = 0;

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
        // then changed to zero, it is to get know when all gamers made 1st move
        lap = -1;
        tires = GameParams.tires;
        uiScript = FindObjectOfType<UIScript>();
        currentPlayer = GameMaster.currentPlayer;
        playerIndices = new List<int>();
        for (int i = 0; i < GameMaster.players.Count; i++)
        {
            playerIndices.Add(i);
        }
    }
    
    private void Update()
    {
        if (gameObject.name == "Player1") {
            currentPlayer = 1;
        } else if (gameObject.name == "Player2") {
            currentPlayer = 2;
        } else if (gameObject.name == "Player3") {
            currentPlayer = 3;
        } else if (gameObject.name == "Player4") {
            currentPlayer = 4;
        } else if (gameObject.name == "Player5") {
            currentPlayer = 5;
        } else if (gameObject.name == "Player6") {
            currentPlayer = 6;
        }
        
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
        PanelUIMainGameScript.CurrentTires = tires;
        PanelUIMainGameScript.CurrentMovementPoints = movementPoints;
        
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
                    PanelUIMainGameScript.CurrentMovementPoints = movementPoints;
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
                    PanelUIMainGameScript.CurrentMovementPoints = movementPoints;
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
                    PanelUIMainGameScript.CurrentMovementPoints = movementPoints;
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
                Debug.Log("Wyjeżdzasz za mapę!");
                transformPosition = transform.position;
                movementPoints++;
            }

            // get other player's position
            List<Vector3> otherPlayerPositions = GameMaster.OtherPlayerPosition(currentPlayer);
            foreach (Vector3 position in otherPlayerPositions)
            {
                float distance = Vector3.Distance(transformPosition, position);

                if (Mathf.Abs(distance) < 0.05f)
                {
                    Debug.Log("Inny gracz jest tutaj");
                    transformPosition = transform.position;
                    movementPoints++;
                    break;
                }
            }

            PanelUIMainGameScript.CurrentMovementPoints = movementPoints;

            while (NextField(transformPosition)) { yield return null; }
            GameMaster.UpdateLaps(transformPosition);
            
            yield return new WaitForSeconds(0.1f);
        }

        // 1st turn
        foreach (GameObject player in GameMaster.players)
        {
            int playerLapCount = player.GetComponent<Movement>().lap;
            if (playerLapCount != -1) 
            {
                player.GetComponent<Movement>().playerMoved = 1;
                eachPlayerHasMoved = 1;
            }
            else
            { 
                player.GetComponent<Movement>().playerMoved = 0;
                eachPlayerHasMoved = 0;
                break;
            }
        }

        if (eachPlayerHasMoved == 0)
        {
            currentPlayer = playerIndices[currentPlayer - 1];
            currentPlayer++;
            if (currentPlayer - 1 >= playerIndices.Count)
            {
                currentPlayer = 1;
            }
            // currentPlayer++;
            // if (currentPlayer > GameParams.players)
            // {
            //     currentPlayer = 1;
            // }
        }
        // other turns
        else
        {
            eachPlayerHasMoved = -eachPlayerHasMoved;
            bool allPlayersMoved = true;
            foreach (GameObject player in GameMaster.players)
            {
                player.GetComponent<Movement>().playerMoved = eachPlayerHasMoved;
                int moved = player.GetComponent<Movement>().playerMoved;
                if (moved != eachPlayerHasMoved)
                {
                    allPlayersMoved = false;
                    break;
                }
            }
            
            if (allPlayersMoved)
            {
                eachPlayerHasMoved = -eachPlayerHasMoved;
                playerMoved = eachPlayerHasMoved;

                Debug.Log("KONIEC TURY");
                // todo: kto zaczyna
                currentPlayer = CheckPlayerPositions();
                
                int closestPlayerIndex = playerIndices[0];
                playerIndices.RemoveAt(0);
                playerIndices.Insert(0, closestPlayerIndex);
            }
            else
            {
                currentPlayer = playerIndices[currentPlayer - 1];
                currentPlayer++;
                if (currentPlayer - 1 >= playerIndices.Count)
                {
                    currentPlayer = 1;
                }
            }
        }

        isMoving = false;
        GameMaster.MovePlayer(currentPlayer);
        GameMaster.CurrentUIGameMaster();

        moveCoroutine = null;
    }
    
    bool NextField(Vector3 dest) {
        return dest != (transform.position = Vector3.MoveTowards(transform.position, 
            dest, 4*Time.deltaTime)); 
    }
    
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
                        PanelUIMainGameScript.CurrentTires = tires;
                        PanelUIMainGameScript.CurrentMovementPoints = movementPoints;
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
                        PanelUIMainGameScript.CurrentTires = tires;
                        PanelUIMainGameScript.CurrentMovementPoints = movementPoints;
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
                        PanelUIMainGameScript.CurrentTires = tires;
                        PanelUIMainGameScript.CurrentMovementPoints = movementPoints;
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
                        PanelUIMainGameScript.CurrentTires = tires;
                        PanelUIMainGameScript.CurrentMovementPoints = movementPoints;
                        break;
                }
            }

            yield return new WaitForSeconds(2f);

            rollCount--;
        }
        
        // enable movement after all dice rolls
        gameObject.transform.Rotate(0,0,-0.01f);
    }

    int CheckPlayerPositions()
    {
        int highestLapCount = -1;
        List<GameObject> playersWithHighestLapCount = new List<GameObject>();

       
        foreach (GameObject player in GameMaster.players)
        {
            int playerLapCount = player.GetComponent<Movement>().lap;

            if (playerLapCount > highestLapCount)
            {
                highestLapCount = playerLapCount;
                playersWithHighestLapCount.Clear();
                playersWithHighestLapCount.Add(player);
            }
            else if (playerLapCount == highestLapCount)
            {
                playersWithHighestLapCount.Add(player);
            }
        }

        int highestAreaID = -1;
        List<GameObject> playersInHighestArea = new List<GameObject>();
        List<GameObject> playersClosestToNextArea = new List<GameObject>();
        GameObject playerClosestToNextArea = null;
        float closestDistanceToNextArea = float.MaxValue;

        foreach (GameObject player in playersWithHighestLapCount)
        {
            Vector3 playerPosition = player.transform.position;
            int playerArea = AssignArea(playerPosition);

            if (playerArea > highestAreaID)
            {
                highestAreaID = playerArea;
                playersInHighestArea.Clear();
                playersInHighestArea.Add(player);
            }
            else if (playerArea == highestAreaID)
            {
                playersInHighestArea.Add(player);
            }
        }

        int nextAreaID = highestAreaID + 1;
        int nextAreaIndex = nextAreaID % 9;

        // distance to next area
        Vector3 nextAreaCenter = GetAreaCenter(nextAreaIndex);
        foreach (GameObject player_ in playersInHighestArea)
        {
            // todo: zmienic liczenie dystansu do nastepnego obszaru ?
            Vector3 playerPosition_ = player_.transform.position;
            float distanceToNextArea = Vector3.Distance(playerPosition_, nextAreaCenter);

            if (distanceToNextArea < closestDistanceToNextArea)
            {
                closestDistanceToNextArea = distanceToNextArea;
                playersClosestToNextArea.Clear();
                playersClosestToNextArea.Add(player_);
            }
            else if (Mathf.Abs(distanceToNextArea - closestDistanceToNextArea) < 0.5f)
            {
                playersClosestToNextArea.Add(player_);
            }
        }

        int playerCount = playersClosestToNextArea.Count;
        Debug.Log(playerCount);
        if (playerCount > 1)
        {
            int randomIndex = Random.Range(0, playerCount);
            playerClosestToNextArea = playersClosestToNextArea[randomIndex];
        }
        else if (playerCount == 1)
        {
            playerClosestToNextArea = playersClosestToNextArea[0];
        }

        Debug.Log("Gracze z najwyższą liczbą okrążeń: " + highestLapCount);
        foreach (GameObject player in playersWithHighestLapCount)
        {
            Debug.Log("Gracz: " + player.name);
        }

        Debug.Log("Gracze w obszarze o najwyższym ID: " + highestAreaID);
        foreach (GameObject player in playersInHighestArea)
        {
            Debug.Log("Gracz: " + player.name);
        }

        Debug.Log("Gracz najbliżej następnego obszaru: " + playerClosestToNextArea.name);

        int playerToMove = int.Parse(playerClosestToNextArea.name.Substring(playerClosestToNextArea.name.Length - 1));

        return playerToMove;
    }
    
    private int AssignArea(Vector3 position)
    {
        int area = 0;
        if ((position.y > -8.6f && position.y < 13.9f && position.x > -30.4f && position.x < -26.6f) && 
            (((Math.Abs((GameMaster.triggerStart1.transform.position.y) - position.y) > 0.05 
               && Math.Abs(GameMaster.triggerStart1.transform.position.x - position.x) < 0.05))
             || ((Math.Abs((GameMaster.trigger2_final.transform.position.y) - position.y) > 0.05 
                  && Math.Abs(GameMaster.trigger2_final.transform.position.x - position.x) < 0.05))
             || ((Math.Abs((GameMaster.triggerStart2.transform.position.y) - position.y) > 0.05 
                  && Math.Abs(GameMaster.triggerStart2.transform.position.x - position.x) < 0.05))))
        {
            area = 1;
        }
        else if (position.y > 10.6f && position.y < 14.4f && position.x > -30f && position.x < -14f)
        {
            area = 2;
        }
        else if (position.y > 4.1f && position.y < 13.9f && position.x > -17.4f && position.x < -13.6f)
        {
            area = 3;
        } else if (position.y > 3.9f && position.y < 7.4f && position.x > -17.4f && position.x < -0.6f)
        {
            area = 4;
        }
        else if (position.y > -24.9f && position.y < 6.9f && position.x > -4.4f && position.x < -0.6f)
        {
            area = 5;
        }
        else if (position.y > -24.9f && position.y < -22.1f && position.x > -19.9f && position.x < -0.6f)
        {
            area = 6;
        }
        else if (position.y > -24.9f && position.y < -13.1f && position.x > -19.4f && position.x < -15.6f)
        {
            area = 7;
        }
        else if (position.y > -16.4f && position.y < -13.4f && position.x > -30.4f && position.x < -15.6f)
        {
            area = 8;
        }
        else if ((position.y > -15.9f && position.y < -6.6f && position.x > -30.4f && position.x < -26.6f) 
                 && GetComponent<Movement>().lap > 0)
        {
            area = 9;
        }

        return area;
    }
    
    Vector3 GetAreaCenter(int areaID)
    {
        Vector3 areaCenter = Vector3.zero;
        switch (areaID)
        {
            case 1:
                areaCenter = new Vector3(-28.5f, 3.65f, 2.7f);
                break;
            case 2:
                areaCenter = new Vector3(-22f, 12.5f, 2.7f);
                break;
            case 3:
                areaCenter = new Vector3(-15.5f, 8.95f, 2.7f);
                break;
            case 4:
                areaCenter = new Vector3(-9f, 5.65f, 2.7f);
                break;
            case 5:
                areaCenter = new Vector3(-2.5f, -9f, 2.7f);
                break;
            case 6:
                areaCenter = new Vector3(-9f, -23.5f, 2.7f);
                break;
            case 7:
                areaCenter = new Vector3(-22f, -19.5f, 2.7f);
                break;
            case 8:
                areaCenter = new Vector3(-22f, -14.9f, 2.7f);
                break;
            case 9:
                areaCenter = new Vector3(-28.5f, -11.25f, 2.7f);
                break;
            default:
                Debug.LogError("Nieprawidłowy identyfikator obszaru: " + areaID);
                break;
        }

        return areaCenter;
    }

}

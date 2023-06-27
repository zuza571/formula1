using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public int currentSpeed;
    public int tires;
    public bool moveAllowed; 
    public int movementPoints;
    public int lap;
    public SkipTurnButtonScript button;

    private Vector3 _transformPosition;
    private Coroutine _moveCoroutine;

    private bool _isMoving;
    private bool _handledDiceRoll;

    private static UIScript _uiScript;
    private int _chosenSpeed;
    private int _track;

    private int _currentPlayer;
    private bool _eachPlayerHasMoved;
    private bool _skipCurrentPlayer;
    public int _movesInGame = 0;

    private Dictionary<int, int> _moves = new Dictionary<int, int>()
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
        _uiScript = FindObjectOfType<UIScript>();
        _currentPlayer = GameMaster.CurrentPlayer;
    }
    
    private void Update()
    {
        if (gameObject.name == "Player1") {
            _currentPlayer = 1;
        } else if (gameObject.name == "Player2") {
            _currentPlayer = 2;
        } else if (gameObject.name == "Player3") {
            _currentPlayer = 3;
        } else if (gameObject.name == "Player4") {
            _currentPlayer = 4;
        } else if (gameObject.name == "Player5") {
            _currentPlayer = 5;
        } else if (gameObject.name == "Player6") {
            _currentPlayer = 6;
        }
        
        if (!_isMoving) {
            if (moveAllowed && _moveCoroutine == null)
            {
                if (_uiScript.HasChosenSpeed)
                {
                    PanelUIMainGameScript.ActivateHintPanel = false;
                    _chosenSpeed = _uiScript.ChosenSpeed;
                    _moveCoroutine = StartCoroutine(Move());
                }
                else
                {
                    PanelUIMainGameScript.ActivateHintPanel = false;
                    if (Input.GetKeyDown(KeyCode.A)
                        || Input.GetAxis("Horizontal") > 0 || Input.GetKeyDown(KeyCode.D) ||
                        Input.GetAxis("Vertical") > 0
                        || Input.GetKeyDown(KeyCode.W))
                    {
                        PanelUIMainGameScript.ActivateHintPanel = true;
                        PanelUIMainGameScript.HintPanelText = "Select speed before move!";
                    }
                }
            } 
        }
    }
    
    IEnumerator Move() 
    {
        if (_isMoving) { yield break; }
        _isMoving = true;

        int[] playerParams = SpeedChanging.ChangeParams(_chosenSpeed, currentSpeed, tires);
        if (playerParams[1] >= 0)
        {
            currentSpeed = playerParams[0];
            tires = playerParams[1];

            movementPoints = _moves[currentSpeed];
            PanelUIMainGameScript.CurrentTires = tires;
            PanelUIMainGameScript.CurrentMovementPoints = movementPoints;
            _movesInGame++;
        
            button.gameObject.SetActive(true);
        }
        else
        {
            PanelUIMainGameScript.ActivateHintPanel = true;
            PanelUIMainGameScript.HintPanelText = "Not enough tires!";
            yield return new WaitForSeconds(4f);
            PanelUIMainGameScript.ActivateHintPanel = false;
        }

        while (movementPoints > 0)
        {
            
            if (button.ShouldResetPoints())
            {
                playerParams = SpeedChanging.ChangeParams(0, currentSpeed, tires);
                currentSpeed = playerParams[0];
                tires = playerParams[1];
                // reset movement points to 0
                movementPoints = 0;
                if (tires < 0)
                {
                    tires = 0;
                }
                PanelUIMainGameScript.CurrentTires = tires;
                PanelUIMainGameScript.CurrentMovementPoints = movementPoints;
                break;
            }
            
            _transformPosition = gameObject.transform.position; 
            float rotationZ = gameObject.transform.eulerAngles.z;

            // left track
            if (Input.GetAxis("Horizontal") < 0 || Input.GetKeyDown(KeyCode.A))
            {
                if (Quaternion.Equals(rotationZ, 180f))
                {
                    _transformPosition.x -= 1;
                    _transformPosition.y += 1;
                }
                else if (Quaternion.Equals(rotationZ, 270f))
                {
                    _transformPosition.x -= 1;
                    _transformPosition.y -= 1;
                }
                else if (Quaternion.Equals(rotationZ, 0f))
                {
                    _transformPosition.x += 1;
                    _transformPosition.y -= 1;
                }
                else if (Quaternion.Equals(rotationZ, 90f))
                {
                    _transformPosition.x += 1;
                    _transformPosition.y += 1;
                }

                if (_transformPosition != gameObject.transform.position)
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
                    _transformPosition.x += 1;
                    _transformPosition.y += 1;
                }
                else if (Quaternion.Equals(rotationZ, 270f))
                {
                    _transformPosition.x -= 1;
                    _transformPosition.y += 1;
                }
                else if (Quaternion.Equals(rotationZ, 0f))
                {
                    _transformPosition.x -= 1;
                    _transformPosition.y -= 1;
                }
                else if (Quaternion.Equals(rotationZ, 90f))
                {
                    _transformPosition.x += 1;
                    _transformPosition.y -= 1;
                }

                if (_transformPosition != gameObject.transform.position)
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
                    _transformPosition.y += 2;
                }
                else if (Quaternion.Equals(rotationZ, 270f))
                {
                    _transformPosition.x -= 2;
                }
                else if (Quaternion.Equals(rotationZ, 0f))
                {
                    _transformPosition.y -= 2;
                }
                else if (Quaternion.Equals(rotationZ, 90f))
                {
                    _transformPosition.x += 2;
                }

                if (_transformPosition != gameObject.transform.position)
                {
                    movementPoints--; 
                    PanelUIMainGameScript.CurrentMovementPoints = movementPoints;
                }
            }

            // turning 
            if (Math.Abs(_transformPosition.y - 11f) < 0.05 && Math.Abs(_transformPosition.x + 27.5f) < 0.05)
            {
                // verify speed
                if (currentSpeed > 80)
                {
                    int diceRollCount = (currentSpeed - 80) / 40;
                    RollTheDice(diceRollCount);
                }
                
                _transformPosition.y = 11.5f;
                _transformPosition.x = -27f;
                transform.Rotate(0,0,-90);
            }
            if (Math.Abs(_transformPosition.y - 12f) < 0.05 && Math.Abs(_transformPosition.x + 28.5f) < 0.05)
            {
                // verify speed
                if (currentSpeed > 120)
                {
                    int diceRollCount = (currentSpeed - 120) / 40;
                    RollTheDice(diceRollCount);
                }
                
                _transformPosition.y = 12.5f;
                _transformPosition.x = -28f;
                transform.Rotate(0,0,-90);
            }
            if (Math.Abs(_transformPosition.y - 13f) < 0.05 && Math.Abs(_transformPosition.x + 29.5f) < 0.05)
            {
                // verify speed
                if (currentSpeed > 160)
                {
                    int diceRollCount = (currentSpeed - 160) / 40;
                    RollTheDice(diceRollCount);
                }
                
                _transformPosition.y = 13.5f;
                _transformPosition.x = -29f;
                transform.Rotate(0,0,-90);
            }
            
            if (Math.Abs(_transformPosition.y - 11.5f) < 0.05 && Math.Abs(_transformPosition.x + 17f) < 0.05)
            {
                // verify speed
                if (currentSpeed > 120)
                {
                    int diceRollCount = (currentSpeed - 120) / 40;
                    RollTheDice(diceRollCount);
                }
                
                _transformPosition.y = 11f;
                _transformPosition.x = -16.5f;
                transform.Rotate(0,0,-90);
            }
            if (Math.Abs(_transformPosition.y - 12.5f) < 0.05 && Math.Abs(_transformPosition.x + 16f) < 0.05)
            {
                // verify speed
                if (currentSpeed > 160)
                {
                    int diceRollCount = (currentSpeed - 160) / 40;
                    RollTheDice(diceRollCount);
                }
                
                _transformPosition.y = 12f;
                _transformPosition.x = -15.5f;
                transform.Rotate(0,0,-90);
            }
            if (Math.Abs(_transformPosition.y - 13.5f) < 0.05 && Math.Abs(_transformPosition.x + 15f) < 0.05)
            {
                // verify speed
                if (currentSpeed > 200)
                {
                    int diceRollCount = (currentSpeed - 200) / 40;
                    RollTheDice(diceRollCount);
                }
                
                _transformPosition.y = 13f;
                _transformPosition.x = -14.5f;
                transform.Rotate(0,0,-90);
            }
            
            if (Math.Abs(_transformPosition.y - 5f) < 0.05 && Math.Abs(_transformPosition.x + 16.5f) < 0.05)
            {
                // verify speed
                if (currentSpeed > 160)
                {
                    int diceRollCount = (currentSpeed - 160) / 40;
                    RollTheDice(diceRollCount);
                }
                
                _transformPosition.y = 4.5f;
                _transformPosition.x = -16f;
                transform.Rotate(0,0,90);
            }
            if (Math.Abs(_transformPosition.y - 6f) < 0.05 && Math.Abs(_transformPosition.x + 15.5f) < 0.05)
            {
                // verify speed
                if (currentSpeed > 120)
                {
                    int diceRollCount = (currentSpeed - 120) / 40;
                    RollTheDice(diceRollCount);
                }
                
                _transformPosition.y = 5.5f;
                _transformPosition.x = -15f;
                transform.Rotate(0,0,90);
            }
            if (Math.Abs(_transformPosition.y - 7f) < 0.05 && Math.Abs(_transformPosition.x + 14.5f) < 0.05)
            {
                // verify speed
                if (currentSpeed > 80)
                {
                    int diceRollCount = (currentSpeed - 80) / 40;
                    RollTheDice(diceRollCount);
                }
                
                _transformPosition.y = 6.5f;
                _transformPosition.x = -14f;
                transform.Rotate(0,0,90);
            }
            
            if (Math.Abs(_transformPosition.y - 4.5f) < 0.05 && Math.Abs(_transformPosition.x + 4f) < 0.05)
            {
                // verify speed
                if (currentSpeed > 160)
                {
                    int diceRollCount = (currentSpeed - 160) / 40;
                    RollTheDice(diceRollCount);
                }
                _transformPosition.y = 4f;
                _transformPosition.x = -3.5f;
                transform.Rotate(0,0,-90);
            }
            if (Math.Abs(_transformPosition.y - 5.5f) < 0.05 && Math.Abs(_transformPosition.x + 3f) < 0.05)
            {
                // verify speed
                if (currentSpeed > 200)
                {
                    int diceRollCount = (currentSpeed - 200) / 40;
                    RollTheDice(diceRollCount);
                }
                _transformPosition.y = 5f;
                _transformPosition.x = -2.5f;
                transform.Rotate(0,0,-90);
            }
            if (Math.Abs(_transformPosition.y - 6.5f) < 0.05 && Math.Abs(_transformPosition.x + 2f) < 0.05)
            {
                // verify speed
                if (currentSpeed > 240)
                {
                    int diceRollCount = (currentSpeed - 240) / 40;
                    RollTheDice(diceRollCount);
                }
                
                _transformPosition.y = 6f;
                _transformPosition.x = -1.5f;
                transform.Rotate(0,0,-90);
            }
            
            if (Math.Abs(_transformPosition.y + 22f) < 0.05 && Math.Abs(_transformPosition.x + 3.5f) < 0.05)
            {
                // verify speed
                if (currentSpeed > 80)
                {
                    int diceRollCount = (currentSpeed - 80) / 40;
                    RollTheDice(diceRollCount);
                }
                
                _transformPosition.y = -22.5f;
                _transformPosition.x = -4f;
                transform.Rotate(0,0,-90);
            }
            if (Math.Abs(_transformPosition.y + 23f) < 0.05 && Math.Abs(_transformPosition.x + 2.5f) < 0.05)
            {
                // verify speed
                if (currentSpeed > 120)
                {
                    int diceRollCount = (currentSpeed - 120) / 40;
                    RollTheDice(diceRollCount);
                }
                
                _transformPosition.y = -23.5f;
                _transformPosition.x = -3f;
                transform.Rotate(0,0,-90);
            }
            if (Math.Abs(_transformPosition.y + 24f) < 0.05 && Math.Abs(_transformPosition.x + 1.5f) < 0.05)
            {
                // verify speed
                if (currentSpeed > 160)
                {
                    int diceRollCount = (currentSpeed - 160) / 40;
                    RollTheDice(diceRollCount);
                }
                
                _transformPosition.y = -24.5f;
                _transformPosition.x = -2f;
                transform.Rotate(0,0,-90);
            }
            
            if (Math.Abs(_transformPosition.y + 22.5f) < 0.05 && Math.Abs(_transformPosition.x + 16f) < 0.05)
            {
                // verify speed
                if (currentSpeed > 200)
                {
                    int diceRollCount = (currentSpeed - 200) / 40;
                    RollTheDice(diceRollCount);
                }
                
                _transformPosition.y = -22f;
                _transformPosition.x = -16.5f;
                transform.Rotate(0,0,-90);
            }
            if (Math.Abs(_transformPosition.y + 23.5f) < 0.05 && Math.Abs(_transformPosition.x + 17f) < 0.05)
            {
                // verify speed
                if (currentSpeed > 240)
                {
                    int diceRollCount = (currentSpeed - 240) / 40;
                    RollTheDice(diceRollCount);
                }
                
                _transformPosition.y = -23f;
                _transformPosition.x = -17.5f;
                transform.Rotate(0,0,-90);
            }
            if (Math.Abs(_transformPosition.y + 24.5f) < 0.05 && Math.Abs(_transformPosition.x + 18f) < 0.05)
            {
                // verify speed
                if (currentSpeed > 280)
                {
                    int diceRollCount = (currentSpeed - 280) / 40;
                    RollTheDice(diceRollCount);
                }
                
                _transformPosition.y = -24f;
                _transformPosition.x = -18.5f;
                transform.Rotate(0,0,-90);
            }
            
            if (Math.Abs(_transformPosition.y + 14f) < 0.05 && Math.Abs(_transformPosition.x + 16.5f) < 0.05)
            {
                // verify speed
                if (currentSpeed > 240)
                {
                    int diceRollCount = (currentSpeed - 240) / 40;
                    RollTheDice(diceRollCount);
                }
                
                _transformPosition.y = -13.5f;
                _transformPosition.x = -17f;
                transform.Rotate(0,0,90);
            }
            if (Math.Abs(_transformPosition.y + 15f) < 0.05 && Math.Abs(_transformPosition.x + 17.5f) < 0.05)
            {
                // verify speed
                if (currentSpeed > 200)
                {
                    int diceRollCount = (currentSpeed - 200) / 40;
                    RollTheDice(diceRollCount);
                }
                
                _transformPosition.y = -14.5f;
                _transformPosition.x = -18f;
                transform.Rotate(0,0,90);
            }
            if (Math.Abs(_transformPosition.y + 16f) < 0.05 && Math.Abs(_transformPosition.x + 18.5f) < 0.05)
            {
                // verify speed
                if (currentSpeed > 160)
                {
                    int diceRollCount = (currentSpeed - 160) / 40;
                    RollTheDice(diceRollCount);
                }
                
                _transformPosition.y = -15.5f;
                _transformPosition.x = -19f;
                transform.Rotate(0,0,90);
            }
            
            if (Math.Abs(_transformPosition.y + 13.5f) < 0.05 && Math.Abs(_transformPosition.x + 27f) < 0.05)
            {
                // verify speed
                if (currentSpeed > 80)
                {
                    int diceRollCount = (currentSpeed - 80) / 40;
                    RollTheDice(diceRollCount);
                }
                
                _transformPosition.y = -13f;
                _transformPosition.x = -27.5f;
                transform.Rotate(0,0,-90);
            }
            if (Math.Abs(_transformPosition.y + 14.5f) < 0.05 && Math.Abs(_transformPosition.x + 28f) < 0.05)
            {
                // verify speed
                if (currentSpeed > 120)
                {
                    int diceRollCount = (currentSpeed - 120) / 40;
                    RollTheDice(diceRollCount);
                }
                
                _transformPosition.y = -14f;
                _transformPosition.x = -28.5f;
                transform.Rotate(0,0,-90);
            }
            if (Math.Abs(_transformPosition.y + 15.5f) < 0.05 && Math.Abs(_transformPosition.x + 29f) < 0.05)
            {
                // verify speed
                if (currentSpeed > 160)
                {
                    int diceRollCount = (currentSpeed - 160) / 40;
                    RollTheDice(diceRollCount);
                }
                
                _transformPosition.y = -15f;
                _transformPosition.x = -29.5f;
                transform.Rotate(0,0,-90);
            }
            
            // map edges
            if (!((_transformPosition.y > -15.9 && _transformPosition.y < 13.9 && _transformPosition.x > -30.4 && _transformPosition.x < -26.6) 
                || (_transformPosition.y > 10.6 && _transformPosition.y < 14.4 && _transformPosition.x > -30 && _transformPosition.x < -14)
                || (_transformPosition.y > 4.1 && _transformPosition.y < 13.9 && _transformPosition.x > -17.4 && _transformPosition.x < -13.6)
                || (_transformPosition.y > 3.9 && _transformPosition.y < 7.4 && _transformPosition.x > -17.4 && _transformPosition.x < -0.6)
                || (_transformPosition.y > -24.9 && _transformPosition.y < 6.9 && _transformPosition.x > -4.4 && _transformPosition.x < -0.6)
                || (_transformPosition.y > -24.9 && _transformPosition.y < -22.1 && _transformPosition.x > -19.9 && _transformPosition.x < -0.6)
                || (_transformPosition.y > -24.9 && _transformPosition.y < -13.1 && _transformPosition.x > -19.4 && _transformPosition.x < -15.6)
                || (_transformPosition.y > -16.4 && _transformPosition.y < -13.4 && _transformPosition.x > -30.4 && _transformPosition.x < -15.6)))
            {
                Debug.Log("Wyjeżdzasz za mapę!");
                _transformPosition = transform.position;
                movementPoints++;
            }

            // get other player's position
            List<Vector3> otherPlayerPositions = GameMaster.OtherPlayerPosition(_currentPlayer);
            foreach (Vector3 position in otherPlayerPositions)
            {
                float distance = Vector3.Distance(_transformPosition, position);

                if (Mathf.Abs(distance) < 0.05f)
                {
                    Debug.Log("Inny gracz jest tutaj");
                    _transformPosition = transform.position;
                    movementPoints++;
                    break;
                }
            }

            PanelUIMainGameScript.CurrentMovementPoints = movementPoints;
            GameMaster.UpdateLaps(_transformPosition);
            
            while (NextField(_transformPosition)) { yield return null; }

            yield return new WaitForSeconds(0.1f);
        }
        
        button.gameObject.SetActive(false);

        _currentPlayer = CheckPlayerPositions();
        _isMoving = false;
        GameMaster.MovePlayer(_currentPlayer);
        GameMaster.CurrentUIGameMaster();
        _moveCoroutine = null;
    }
    
    bool NextField(Vector3 dest) {
        return dest != (transform.position = Vector3.MoveTowards(transform.position, 
            dest, 4*Time.deltaTime)); 
    }
    
    void RollTheDice(int rollCount) { StartCoroutine(RollDiceWithTimeout(rollCount)); }
    
    public IEnumerator RollDiceWithTimeout(int rollCount)
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
                        playerParams = SpeedChanging.ChangeParams(0, currentSpeed, tires);
                        currentSpeed = playerParams[0];
                        tires = playerParams[1];

                        tires -= 1;
                        if (tires < 0)
                        {
                            tires = 0;
                            playerParams = SpeedChanging.ChangeParams(0, currentSpeed, tires);
                            currentSpeed = playerParams[0];
                            tires = playerParams[1];
                            movementPoints = 0;
                            rollCount = 0;
                        }
                        PanelUIMainGameScript.CurrentTires = tires;
                        PanelUIMainGameScript.CurrentMovementPoints = movementPoints;
                        break;
                    case 2:
                        playerParams = SpeedChanging.ChangeParams(0, currentSpeed, tires);
                        currentSpeed = playerParams[0];
                        tires = playerParams[1];
                        
                        tires -= 2;
                        if (tires < 0)
                        {
                            tires = 0;
                            playerParams = SpeedChanging.ChangeParams(0, currentSpeed, tires);
                            currentSpeed = playerParams[0];
                            tires = playerParams[1];
                            movementPoints = 0;
                            rollCount = 0;
                        }
                        PanelUIMainGameScript.CurrentTires = tires;
                        PanelUIMainGameScript.CurrentMovementPoints = movementPoints;
                        break;
                    case 3:
                        playerParams = SpeedChanging.ChangeParams(0, currentSpeed, tires);
                        currentSpeed = playerParams[0];
                        tires = playerParams[1];

                        tires -= 3;
                        if (tires < 0)
                        {
                            tires = 0;
                            playerParams = SpeedChanging.ChangeParams(0, currentSpeed, tires);
                            currentSpeed = playerParams[0];
                            tires = playerParams[1];
                            movementPoints = 0;
                            rollCount = 0;
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
                        movementPoints = 0;
                        rollCount = 0;
                        if (tires < 0)
                        {
                            tires = 0;
                            playerParams = SpeedChanging.ChangeParams(0, currentSpeed, tires);
                            currentSpeed = playerParams[0];
                            tires = playerParams[1];
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
        float threshold = 0.0001f;
        if (Mathf.Abs(gameObject.transform.eulerAngles.z) < threshold)
        {
            var eulerAngles = gameObject.transform.eulerAngles;
            eulerAngles = new Vector3(
                eulerAngles.x,
                eulerAngles.y,
                0f
            );
            gameObject.transform.eulerAngles = eulerAngles;
        }
    }

    int CheckPlayerPositions()
    {
        List<GameObject> playersWithLessMoves = new List<GameObject>();
        int playerMovesInGameMin = GameMaster.Players[0].GetComponent<Movement>()._movesInGame;
        foreach (GameObject player in GameMaster.Players)
        {
            int playerMovesInGame = player.GetComponent<Movement>()._movesInGame;
            if (playerMovesInGame < playerMovesInGameMin)
            {
                playerMovesInGameMin = playerMovesInGame;
            }
        }

        foreach (GameObject player in GameMaster.Players)
        {
            if (player.GetComponent<Movement>()._movesInGame == playerMovesInGameMin)
            {
                playersWithLessMoves.Add(player);
            }
        }

        int highestLapCount = -1;
        List<GameObject> playersWithHighestLapCount = new List<GameObject>();

        foreach (GameObject player in playersWithLessMoves)
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

        int areaIDMax = 0;
        GameObject playerClosestToNextArea = null;

        foreach (GameObject player in playersWithHighestLapCount)
        {
            if (player.GetComponent<Movement>()._movesInGame == 0)
            {
                if (playerClosestToNextArea != null &&
                    player.transform.position.y > playerClosestToNextArea.transform.position.y)
                {
                    playerClosestToNextArea = player;
                    continue;
                }
                if (playerClosestToNextArea != null 
                         && Math.Abs(player.transform.position.y - playerClosestToNextArea.transform.position.y) < 0.05
                         && player.transform.position.x < playerClosestToNextArea.transform.position.x) 
                {
                    playerClosestToNextArea = player;
                    continue;
                }
                if (playerClosestToNextArea != null 
                    && Math.Abs(player.transform.position.y - playerClosestToNextArea.transform.position.y) < 0.05
                    && player.transform.position.x > playerClosestToNextArea.transform.position.x) 
                {
                    continue;
                }
            }

            int currentAreaID = 0;

            if (((Math.Abs(player.transform.position.y + 11) < 0.05 &&
                  Math.Abs(player.transform.position.x + 29.5) < 0.05)
                 || (Math.Abs(player.transform.position.y + 13) < 0.05 &&
                     Math.Abs(player.transform.position.x + 29.5) < 0.05)
                 || (Math.Abs(player.transform.position.y + 15) < 0.05 &&
                     Math.Abs(player.transform.position.x + 29.5) < 0.05)
                 || (Math.Abs(player.transform.position.y + 14) < 0.05 &&
                     Math.Abs(player.transform.position.x + 28.5) < 0.05)
                 || (Math.Abs(player.transform.position.y + 12) < 0.05 &&
                     Math.Abs(player.transform.position.x + 28.5) < 0.05)
                 || (Math.Abs(player.transform.position.y + 10) < 0.05 &&
                     Math.Abs(player.transform.position.x + 28.5) < 0.05)
                 || (Math.Abs(player.transform.position.y + 13) < 0.05 &&
                     Math.Abs(player.transform.position.x + 27.5) < 0.05)
                 || (Math.Abs(player.transform.position.y + 15) < 0.05 &&
                     Math.Abs(player.transform.position.x + 27.5) < 0.05)
                 || (Math.Abs(player.transform.position.y + 17) < 0.05 &&
                     Math.Abs(player.transform.position.x + 27.5) < 0.05))
                && player.GetComponent<Movement>()._movesInGame > 5)
            {
                currentAreaID = 9;
                if (currentAreaID > areaIDMax)
                {
                    areaIDMax = currentAreaID;
                    playerClosestToNextArea = player;
                }
                else if (currentAreaID == areaIDMax)
                {
                    if (playerClosestToNextArea != null &&
                        player.transform.position.x < playerClosestToNextArea.transform.position.x)
                    {
                        playerClosestToNextArea = player;
                    }
                    else if (playerClosestToNextArea != null && Math.Abs(player.transform.position.x -
                                                                         playerClosestToNextArea.transform.position
                                                                             .x) < 0.05
                                                             && player.transform.position.y >
                                                             playerClosestToNextArea.transform.position.y)
                    {
                        playerClosestToNextArea = player;
                    }
                }
            }
            else if (player.transform.position.y > -16.4 && player.transform.position.y < -13.4 &&
                     player.transform.position.x > -30.4 &&
                     player.transform.position.x < -15.6 && Math.Abs(player.transform.eulerAngles.z - 270) < 0.05)
            {
                currentAreaID = 8;
                if (currentAreaID > areaIDMax)
                {
                    areaIDMax = currentAreaID;
                    playerClosestToNextArea = player;
                }
                else if (currentAreaID == areaIDMax)
                {
                    if (playerClosestToNextArea != null &&
                        player.transform.position.x < playerClosestToNextArea.transform.position.x)
                    {
                        playerClosestToNextArea = player;
                    }
                    else if (playerClosestToNextArea != null && Math.Abs(player.transform.position.x -
                                                                         playerClosestToNextArea.transform.position
                                                                             .x) < 0.05
                                                             && player.transform.position.y >
                                                             playerClosestToNextArea.transform.position.y)
                    {
                        playerClosestToNextArea = player;
                    }
                }
            }
            else if (player.transform.position.y > -24.9 && player.transform.position.y < -13.1 &&
                     player.transform.position.x > -19.4 &&
                     player.transform.position.x < -15.6 && Math.Abs(player.transform.eulerAngles.z - 180) < 0.05)
            {
                currentAreaID = 7;
                if (currentAreaID > areaIDMax)
                {
                    areaIDMax = currentAreaID;
                    playerClosestToNextArea = player;
                }
                else if (currentAreaID == areaIDMax)
                {
                    if (playerClosestToNextArea != null &&
                        player.transform.position.y > playerClosestToNextArea.transform.position.y)
                    {
                        playerClosestToNextArea = player;
                    }
                    else if (playerClosestToNextArea != null && Math.Abs(player.transform.position.y -
                                                                         playerClosestToNextArea.transform.position
                                                                             .y) < 0.05
                                                             && player.transform.position.x <
                                                             playerClosestToNextArea.transform.position.x)
                    {
                        playerClosestToNextArea = player;
                    }
                }
            }
            else if (player.transform.position.y > -24.9 && player.transform.position.y < -22.1 &&
                     player.transform.position.x > -19.9 &&
                     player.transform.position.x < -0.6 && Math.Abs(player.transform.eulerAngles.z - 270) < 0.05)
            {
                currentAreaID = 6;
                if (currentAreaID > areaIDMax)
                {
                    areaIDMax = currentAreaID;
                    playerClosestToNextArea = player;
                }
                else if (currentAreaID == areaIDMax)
                {
                    if (playerClosestToNextArea != null &&
                        player.transform.position.x < playerClosestToNextArea.transform.position.x)
                    {
                        playerClosestToNextArea = player;
                    }
                    else if (playerClosestToNextArea != null && Math.Abs(player.transform.position.x -
                                                                         playerClosestToNextArea.transform.position
                                                                             .x) < 0.05
                                                             && player.transform.position.y >
                                                             playerClosestToNextArea.transform.position.y)
                    {
                        playerClosestToNextArea = player;
                    }
                }
            }
            else if (player.transform.position.y > -24.9 && player.transform.position.y < 6.9 &&
                     player.transform.position.x > -4.4 &&
                     player.transform.position.x < -0.6 && Math.Abs(player.transform.eulerAngles.z - 0) < 0.05)
            {
                currentAreaID = 5;
                if (currentAreaID > areaIDMax)
                {
                    areaIDMax = currentAreaID;
                    playerClosestToNextArea = player;
                }
                else if (currentAreaID == areaIDMax)
                {
                    if (playerClosestToNextArea != null &&
                        player.transform.position.y < playerClosestToNextArea.transform.position.y)
                    {
                        playerClosestToNextArea = player;
                    }
                    else if (playerClosestToNextArea != null && Math.Abs(player.transform.position.y -
                                                                         playerClosestToNextArea.transform.position
                                                                             .y) < 0.05
                                                             && player.transform.position.x <
                                                             playerClosestToNextArea.transform.position.x)
                    {
                        playerClosestToNextArea = player;
                    }
                }
            }
            else if (player.transform.position.y > 3.9 && player.transform.position.y < 7.4 &&
                     player.transform.position.x > -17.4 &&
                     player.transform.position.x < -0.6 && Math.Abs(player.transform.eulerAngles.z - 90) < 0.05)
            {
                currentAreaID = 4;
                if (currentAreaID > areaIDMax)
                {
                    areaIDMax = currentAreaID;
                    playerClosestToNextArea = player;
                }
                else if (currentAreaID == areaIDMax)
                {
                    if (playerClosestToNextArea != null &&
                        player.transform.position.x > playerClosestToNextArea.transform.position.x)
                    {
                        playerClosestToNextArea = player;
                    }
                    else if (playerClosestToNextArea != null && Math.Abs(player.transform.position.x -
                                                                         playerClosestToNextArea.transform.position
                                                                             .x) < 0.05
                                                             && player.transform.position.y <
                                                             playerClosestToNextArea.transform.position.y)
                    {
                        playerClosestToNextArea = player;
                    }
                }
            }
            else if (player.transform.position.y > 4.1 && player.transform.position.y < 13.9 &&
                     player.transform.position.x > -17.4 &&
                     player.transform.position.x < -13.6 && Math.Abs(player.transform.eulerAngles.z - 0) < 0.05)
            {
                currentAreaID = 3;
                if (currentAreaID > areaIDMax)
                {
                    areaIDMax = currentAreaID;
                    playerClosestToNextArea = player;
                }
                else if (currentAreaID == areaIDMax)
                {
                    if (playerClosestToNextArea != null &&
                        player.transform.position.y < playerClosestToNextArea.transform.position.y)
                    {
                        playerClosestToNextArea = player;
                    }
                    else if (playerClosestToNextArea != null && Math.Abs(player.transform.position.y -
                                                                         playerClosestToNextArea.transform.position
                                                                             .y) < 0.05
                                                             && player.transform.position.x >
                                                             playerClosestToNextArea.transform.position.x)
                    {
                        playerClosestToNextArea = player;
                    }
                }
            }
            else if (player.transform.position.y > 10.6 && player.transform.position.y < 14.4 &&
                     player.transform.position.x > -30 &&
                     player.transform.position.x < -14 && Math.Abs(player.transform.eulerAngles.z - 90) < 0.05)
            {
                currentAreaID = 2;
                if (currentAreaID > areaIDMax)
                {
                    areaIDMax = currentAreaID;
                    playerClosestToNextArea = player;
                }
                else if (currentAreaID == areaIDMax)
                {
                    if (playerClosestToNextArea != null &&
                        player.transform.position.x > playerClosestToNextArea.transform.position.x)
                    {
                        playerClosestToNextArea = player;
                    }
                    else if (playerClosestToNextArea != null && Math.Abs(player.transform.position.x -
                                                                         playerClosestToNextArea.transform.position
                                                                             .x) < 0.05
                                                             && player.transform.position.y <
                                                             playerClosestToNextArea.transform.position.y)
                    {
                        playerClosestToNextArea = player;
                    }
                }
            }
            else if (player.transform.position.y > -15.9 && player.transform.position.y < 13.9 &&
                     player.transform.position.x > -30.4 &&
                     player.transform.position.x < -26.6 && Math.Abs(player.transform.eulerAngles.z - 180) < 0.05)
            {
                currentAreaID = 1;
                if (currentAreaID > areaIDMax)
                {
                    areaIDMax = currentAreaID;
                    playerClosestToNextArea = player;
                }
                else if (currentAreaID == areaIDMax)
                {
                    if (playerClosestToNextArea != null &&
                        player.transform.position.y > playerClosestToNextArea.transform.position.y)
                    {
                        playerClosestToNextArea = player;
                    }
                    else if (playerClosestToNextArea != null && Math.Abs(player.transform.position.y -
                                                                         playerClosestToNextArea.transform.position
                                                                             .y) < 0.05
                                                             && player.transform.position.x >
                                                             playerClosestToNextArea.transform.position.x)
                    {
                        playerClosestToNextArea = player;
                    }
                }
            }
        }

        // Debug.Log("Gracz najbliżej następnego obszaru: " + playerClosestToNextArea.name);
        int playerToMove = int.Parse(playerClosestToNextArea.name.Substring(playerClosestToNextArea.name.Length - 1));

        return playerToMove;
    }

}

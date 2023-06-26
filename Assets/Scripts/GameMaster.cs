using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameMaster : MonoBehaviour
{
    public static List<GameObject> Players;
    public static GameObject TriggerStart1, TriggerStart2, TriggerStart3, TriggerStart4, TriggerStart5, TriggerStart6, 
        Trigger2Final;
    public int playersNumber;
    public static int CurrentPlayer;
    private bool _isCoroutineRunning;
    private int _startingPlayerIndex;
    private Dictionary<int, int> _sortedPlayerResultMapping;
    
    IEnumerator Start()
    {
        PanelUIMainGameScript.ActivateRollingTheDicePanel = true;
        PanelUIMainGameScript.CurrentPlayerColor = Color.white;
        PanelUIMainGameScript.CurrentPlayer = "Player -";

        Players = new List<GameObject>();
        playersNumber = GameParams.players;
        Players.Add(GameObject.Find("Player1"));
        Players.Add(GameObject.Find("Player2"));

        TriggerStart1 = GameObject.Find("Trigger_start1");
        TriggerStart2 = GameObject.Find("Trigger_start2");
        TriggerStart3 = GameObject.Find("Trigger_start3");
        TriggerStart4 = GameObject.Find("Trigger_start4");
        TriggerStart5 = GameObject.Find("Trigger_start5");
        TriggerStart6 = GameObject.Find("Trigger_start6");
        Trigger2Final = GameObject.Find("Trigger_start2-final");

        Sprite[] carSprites = new Sprite[6];
        carSprites[0] = Resources.Load<Sprite>("Cars/car_blue");
        carSprites[1] = Resources.Load<Sprite>("Cars/car_red");
        carSprites[2] = Resources.Load<Sprite>("Cars/car_green");
        carSprites[3] = Resources.Load<Sprite>("Cars/car_black");
        carSprites[4] = Resources.Load<Sprite>("Cars/car_yellow");
        carSprites[5] = Resources.Load<Sprite>("Cars/car_orange");
        
        List<int> availableIndices = new List<int>();
        for (int i = 0; i < carSprites.Length; i++)
        {
            availableIndices.Add(i);
        }

        // add new players
        if (playersNumber > 2)
        {
            GameObject player1 = Players[0];
            for (int i = 3; i <= playersNumber; i++)
            {
                GameObject newPlayer = Instantiate(player1);
                newPlayer.name = "Player" + i;
                Players.Add(newPlayer);
            }
        }

        // starting positions
        if (playersNumber > 2)
        {
            if (playersNumber == 3)
            {
                Players[0].transform.position = TriggerStart1.transform.position;
                Players[1].transform.position = TriggerStart2.transform.position;
                Players[2].transform.position = TriggerStart3.transform.position;
            }
            else if (playersNumber == 4)
            {
                Players[0].transform.position = TriggerStart1.transform.position;
                Players[1].transform.position = TriggerStart2.transform.position;
                Players[2].transform.position = TriggerStart3.transform.position;
                Players[3].transform.position = TriggerStart4.transform.position;
            }
            else if (playersNumber == 5)
            {
                Players[0].transform.position = TriggerStart1.transform.position;
                Players[1].transform.position = TriggerStart2.transform.position;
                Players[2].transform.position = TriggerStart3.transform.position;
                Players[3].transform.position = TriggerStart4.transform.position;
                Players[4].transform.position = TriggerStart5.transform.position;
            }
            else if (playersNumber == 6)
            {
                Players[0].transform.position = TriggerStart1.transform.position;
                Players[1].transform.position = TriggerStart2.transform.position;
                Players[2].transform.position = TriggerStart3.transform.position;
                Players[3].transform.position = TriggerStart4.transform.position;
                Players[4].transform.position = TriggerStart5.transform.position;
                Players[5].transform.position = TriggerStart6.transform.position;
            }
        }
        else
        {
            Players[0].transform.position = TriggerStart1.transform.position;
            Players[1].transform.position = TriggerStart2.transform.position;
        }
        
        // randomly choose racing cars
        for (int i = 0; i < Players.Count; i++)
        {
            GameObject player = Players[i];

            if (availableIndices.Count > 0)
            {
                int randomIndex = availableIndices[Random.Range(0, availableIndices.Count)];
                Sprite randomSprite = carSprites[randomIndex];
                SpriteRenderer renderer = player.GetComponent<SpriteRenderer>();
                if (renderer != null)
                {
                    renderer.sprite = randomSprite;
                }
                availableIndices.Remove(randomIndex);
            }
        }
        
        // disable movement
        for (int i = 0; i < Players.Count; i++)
        {
           Players[i].GetComponent<Movement>().moveAllowed = false;
        }
        
        yield return new WaitForSeconds(4f);
        PanelUIMainGameScript.ActivateRollingTheDicePanel = false;
        yield return new WaitForSeconds(1f);

        RollTheDice(playersNumber);
        yield return new WaitUntil(() => StartingPlayer > 0);
        
        // change starting positions
        int triggerIndex = 1;
        GameObject firstPlayer = Players[_startingPlayerIndex - 1];
        firstPlayer.transform.position = GameObject.Find("Trigger_start1").transform.position;
        triggerIndex++;

        foreach (var entry in _sortedPlayerResultMapping)
        {
            int playerIdx = entry.Key;
            if (playerIdx != _startingPlayerIndex && triggerIndex <= 6)
            {
                GameObject trigger = GameObject.Find("Trigger_start" + triggerIndex);
                if (trigger != null)
                {
                    Players[playerIdx - 1].transform.position = trigger.transform.position;
                    triggerIndex++;
                }
            }
        }

        CurrentPlayer = _startingPlayerIndex;
        Players[_startingPlayerIndex - 1].GetComponent<Movement>().moveAllowed = true;

        CurrentUIGameMaster();
    }

    public static void MovePlayer(int playerToMove)
    {
        foreach (GameObject player in Players)
        {
            player.GetComponent<Movement>().moveAllowed = false;
        }
        Players[playerToMove - 1].GetComponent<Movement>().moveAllowed = true;
    }

    public static List<Vector3> OtherPlayerPosition(int currentPlayer)
    {
        List<Vector3> positions = new List<Vector3>();

        for (int i = 0; i < Players.Count; i++)
        {
            if (i + 1 != currentPlayer)
            {
                positions.Add(Players[i].transform.position);
            }
        }
        
        return positions;
    }
    
    public static void UpdateLaps(Vector3 transformPosition)
    {
        for (int i = 0; i < Players.Count; i++)
        {
            if (Players[i].GetComponent<Movement>()._movesInGame < 5)
            {
                Players[i].GetComponent<Movement>().lap = 0;
            }
            else if ((((Math.Abs(transformPosition.y - TriggerStart1.transform.position.y) < 0.05 && Math.Abs(transformPosition.x - TriggerStart1.transform.position.x) < 0.05))
                      || ((Math.Abs(transformPosition.y - Trigger2Final.transform.position.y) < 0.05 && Math.Abs(transformPosition.x - Trigger2Final.transform.position.x) < 0.05))
                      || ((Math.Abs(transformPosition.y - TriggerStart2.transform.position.y) < 0.05 && Math.Abs(transformPosition.x - TriggerStart2.transform.position.x) < 0.05)))
                     && 
                     (((Math.Abs((TriggerStart1.transform.position.y - 2) - Players[i].transform.position.y) < 0.05 && Math.Abs(TriggerStart1.transform.position.x - Players[i].transform.position.x) < 0.05))
                      || ((Math.Abs((Trigger2Final.transform.position.y - 2) - Players[i].transform.position.y) < 0.05 && Math.Abs(Trigger2Final.transform.position.x - Players[i].transform.position.x) < 0.05))
                      || ((Math.Abs((TriggerStart2.transform.position.y - 2) - Players[i].transform.position.y) < 0.05 && Math.Abs(TriggerStart2.transform.position.x - Players[i].transform.position.x) < 0.05)))
                     && Players[i].GetComponent<Movement>().moveAllowed)
            {
                if (Players[i].GetComponent<Movement>().lap != -1) { Players[i].GetComponent<Movement>().lap++; }
                if (Players[i].GetComponent<Movement>().lap == GameParams.laps)
                {
                    GameOver.winner = "Player " + (i + 1) + " wins!";
                    SceneManager.LoadScene("TheEnd");
                }
            }
        }
    }
    
    public static void CurrentUIGameMaster()
    {
        for (int i = 0; i < Players.Count; i++)
        {
            if (Players[i].GetComponent<Movement>().moveAllowed)
            {
                PanelUIMainGameScript.CurrentPlayer = "Player " + (i + 1);
                Sprite sprite = Players[i].GetComponent<SpriteRenderer>().sprite;
                String spriteName = sprite.name;

                if (spriteName.Contains("car_red"))
                {
                    PanelUIMainGameScript.CurrentPlayerColor = Color.red;
                }
                else if (spriteName.Contains("car_blue"))
                {
                    PanelUIMainGameScript.CurrentPlayerColor = Color.blue;
                } 
                else if (spriteName.Contains("car_black"))
                {
                    PanelUIMainGameScript.CurrentPlayerColor = Color.black;
                } 
                else if (spriteName.Contains("car_yellow"))
                {
                    PanelUIMainGameScript.CurrentPlayerColor = Color.yellow;
                } 
                else if (spriteName.Contains("car_green"))
                {
                    PanelUIMainGameScript.CurrentPlayerColor = new Color(0f, 0.5f, 0f);;
                }
                else if (spriteName.Contains("car_orange"))
                {
                    PanelUIMainGameScript.CurrentPlayerColor = new Color(1f, 0.5f, 0f);;
                }

                PanelUIMainGameScript.CurrentSpeed = Players[i].GetComponent<Movement>().currentSpeed;
                PanelUIMainGameScript.CurrentTires = Players[i].GetComponent<Movement>().tires;
                PanelUIMainGameScript.CurrentLap = Players[i].GetComponent<Movement>().lap + 1;
                break;
            }
        }
    }
    
    void RollTheDice(int playerNumber) { StartCoroutine(RollDiceStart(playerNumber)); }

    public IEnumerator RollDiceStart(int playerNumber)
    {
        _isCoroutineRunning = true;

        Dictionary<int, int> playerResultMapping = new Dictionary<int, int>();
        int playerIdx = 1;
        while (playerIdx <= playerNumber)
        {
            Dice diceScript = FindObjectOfType<Dice>();
            if (diceScript != null)
            {
                StartCoroutine(diceScript.RollTheDice());
                yield return new WaitUntil(() => diceScript.RandomDiceSlide > 0);
                int result = diceScript.RandomDiceSlide;
                Debug.Log("player" + playerIdx + ": " + result);
                
                playerResultMapping.Add(playerIdx, result);
            }

            yield return new WaitForSeconds(2f);
            
            playerIdx++;
        }
        _sortedPlayerResultMapping = playerResultMapping.OrderByDescending(x => x.Value)
            .ToDictionary(x => x.Key, x => x.Value);

        int maxResult = playerResultMapping.Values.Max();
        List<int> playersWithMaxResult = playerResultMapping.Where(x => 
            x.Value == maxResult).Select(x => x.Key).ToList();

        if (playersWithMaxResult.Count > 1)
        {
            _startingPlayerIndex = playersWithMaxResult[Random.Range(0, playersWithMaxResult.Count)];
        }
        else
        {
            _startingPlayerIndex = playersWithMaxResult[0];
        }

        _isCoroutineRunning = false;
    }
    
    public int StartingPlayer
    {
        get
        {
            if (_isCoroutineRunning)
            {
                return 0;
            }
            else
            {
                return _startingPlayerIndex;
            }
        }
    }
}
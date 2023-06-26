using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameOver : MonoBehaviour
{

    public static String winner;

    public TextMeshProUGUI TextMeshProUGUI;
    
    public static List<GameObject> players;
    public static GameObject triggerStart1, triggerStart2, triggerStart3, triggerStart4, triggerStart5, triggerStart6, 
        trigger2_final;
    public int playersNumber;
    public static int currentPlayer;
    private bool isCoroutineRunning;
    private int startingPlayerIndex;
    private Dictionary<int, int> sortedPlayerResultMapping;

    private IEnumerator Start()
    {
        TextMeshProUGUI.SetText(winner);
        GameParams.players = 2;
        GameParams.tires = 40;
        GameParams.laps = 2;

        // ------------------------------ //
        players = new List<GameObject>();
        playersNumber = GameParams.players;
        players.Add(GameObject.Find("Player1"));
        players.Add(GameObject.Find("Player2"));

        triggerStart1 = GameObject.Find("Trigger_start1");
        triggerStart2 = GameObject.Find("Trigger_start2");
        triggerStart3 = GameObject.Find("Trigger_start3");
        triggerStart4 = GameObject.Find("Trigger_start4");
        triggerStart5 = GameObject.Find("Trigger_start5");
        triggerStart6 = GameObject.Find("Trigger_start6");
        trigger2_final = GameObject.Find("Trigger_start2-final");

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
            GameObject player1 = players[0];
            for (int i = 3; i <= playersNumber; i++)
            {
                GameObject newPlayer = Instantiate(player1);
                newPlayer.name = "Player" + i;
                players.Add(newPlayer);
            }
        }

        // starting positions
        if (playersNumber > 2)
        {
            if (playersNumber == 3)
            {
                players[0].transform.position = triggerStart1.transform.position;
                players[1].transform.position = triggerStart2.transform.position;
                players[2].transform.position = triggerStart3.transform.position;
            }
            else if (playersNumber == 4)
            {
                players[0].transform.position = triggerStart1.transform.position;
                players[1].transform.position = triggerStart2.transform.position;
                players[2].transform.position = triggerStart3.transform.position;
                players[3].transform.position = triggerStart4.transform.position;
            }
            else if (playersNumber == 5)
            {
                players[0].transform.position = triggerStart1.transform.position;
                players[1].transform.position = triggerStart2.transform.position;
                players[2].transform.position = triggerStart3.transform.position;
                players[3].transform.position = triggerStart4.transform.position;
                players[4].transform.position = triggerStart5.transform.position;
            }
            else if (playersNumber == 6)
            {
                players[0].transform.position = triggerStart1.transform.position;
                players[1].transform.position = triggerStart2.transform.position;
                players[2].transform.position = triggerStart3.transform.position;
                players[3].transform.position = triggerStart4.transform.position;
                players[4].transform.position = triggerStart5.transform.position;
                players[5].transform.position = triggerStart6.transform.position;
            }
        }
        else
        {
            players[0].transform.position = triggerStart1.transform.position;
            players[1].transform.position = triggerStart2.transform.position;
        }
        
        // randomly choose racing cars
        for (int i = 0; i < players.Count; i++)
        {
            GameObject player = players[i];

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
        for (int i = 0; i < players.Count; i++)
        {
           players[i].GetComponent<Movement>().moveAllowed = false;
        }
        
        RollTheDice(playersNumber);
        yield return new WaitUntil(() => StartingPlayer > 0);
        
        // change starting positions
        int triggerIndex = 1;
        GameObject firstPlayer = players[startingPlayerIndex - 1];
        firstPlayer.transform.position = GameObject.Find("Trigger_start1").transform.position;
        triggerIndex++;

        foreach (var entry in sortedPlayerResultMapping)
        {
            int playerIdx = entry.Key;
            int result = entry.Value;

            if (playerIdx != startingPlayerIndex && triggerIndex <= 6)
            {
                GameObject trigger = GameObject.Find("Trigger_start" + triggerIndex);
                if (trigger != null)
                {
                    players[playerIdx - 1].transform.position = trigger.transform.position;
                    triggerIndex++;
                }
            }
        }

        currentPlayer = startingPlayerIndex;
        
        players[startingPlayerIndex - 1].GetComponent<Movement>().moveAllowed = true;

        GameMaster.CurrentUIGameMaster();
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    void RollTheDice(int playerNumber) { StartCoroutine(RollDiceStart(playerNumber)); }

    public IEnumerator RollDiceStart(int playerNumber)
    {
        bool isCoroutineRunning = true;

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
        sortedPlayerResultMapping = playerResultMapping.OrderByDescending(x => x.Value)
            .ToDictionary(x => x.Key, x => x.Value);

        int maxResult = playerResultMapping.Values.Max();
        List<int> playersWithMaxResult = playerResultMapping.Where(x => 
            x.Value == maxResult).Select(x => x.Key).ToList();

        if (playersWithMaxResult.Count > 1)
        {
            startingPlayerIndex = playersWithMaxResult[Random.Range(0, playersWithMaxResult.Count)];
        }
        else
        {
            startingPlayerIndex = playersWithMaxResult[0];
        }

        isCoroutineRunning = false;
    }
    
    public int StartingPlayer
    {
        get
        {
            if (isCoroutineRunning)
            {
                return 0;
            }
            else
            {
                return startingPlayerIndex;
            }
        }
    }
}

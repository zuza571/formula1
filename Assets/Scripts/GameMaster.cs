using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour
{
    private static GameObject player1, player2;

    public static GameObject triggerStart1;
    public static GameObject triggerStart2;
    public static GameObject triggerStart3;

    void Start()
    {
        // todo: rzut kostką kto zaczyna
        player1 = GameObject.Find("Player1");
        player2 = GameObject.Find("Player2");

        triggerStart1 = GameObject.Find("Trigger_start1");
        triggerStart2 = GameObject.Find("Trigger_start2");
        triggerStart3 = GameObject.Find("Trigger_start3");

        player1.transform.position = triggerStart1.transform.position;
        player2.transform.position = triggerStart3.transform.position;

        player1.GetComponent<Movement>().moveAllowed = true;
        player2.GetComponent<Movement>().moveAllowed = false;
    }

    public static void MovePlayer(int playerToMove)
    {
        switch (playerToMove)
        {
            case 1:
                player1.GetComponent<Movement>().moveAllowed = true;
                player2.GetComponent<Movement>().moveAllowed = false;
                break;
                
            case 2:
                player2.GetComponent<Movement>().moveAllowed = true;   
                player1.GetComponent<Movement>().moveAllowed = false;
                break;
            
            default:
                player1.GetComponent<Movement>().moveAllowed = false;
                player2.GetComponent<Movement>().moveAllowed = false;
                break;
        }
    }

    public static Vector3 OtherPlayerPosition(int currentPlayer)
    {
        Vector3 position = new (0,0,0);
        switch (currentPlayer)
        {
            case 1:
                position = player2.transform.position;
                break;
            case -1:
                position = player1.transform.position;
                break;
        }

        return position;
    }

    public static void UpdateLaps(Vector3 transformPosition)
    {
        if ((((Math.Abs(transformPosition.y - triggerStart1.transform.position.y) < 0.05 && Math.Abs(transformPosition.x - triggerStart1.transform.position.x) < 0.05))
             || ((Math.Abs(transformPosition.y - triggerStart2.transform.position.y) < 0.05 && Math.Abs(transformPosition.x - triggerStart2.transform.position.x) < 0.05))
             || ((Math.Abs(transformPosition.y - triggerStart3.transform.position.y) < 0.05 && Math.Abs(transformPosition.x - triggerStart3.transform.position.x) < 0.05)))
            && 
            (((Math.Abs((triggerStart1.transform.position.y - 2) - player1.transform.position.y) < 0.05 && Math.Abs(triggerStart1.transform.position.x - player1.transform.position.x) < 0.05))
             || ((Math.Abs((triggerStart2.transform.position.y - 2) - player1.transform.position.y) < 0.05 && Math.Abs(triggerStart2.transform.position.x - player1.transform.position.x) < 0.05))
             || ((Math.Abs((triggerStart3.transform.position.y - 2) - player1.transform.position.y) < 0.05 && Math.Abs(triggerStart3.transform.position.x - player1.transform.position.x) < 0.05)))
            && player1.GetComponent<Movement>().moveAllowed)
        {
            player1.GetComponent<Movement>().lap++;
            if (player1.GetComponent<Movement>().lap == GameParams.laps)
            {
                GameOver.winner = "Player 1 wins!";
                SceneManager.LoadScene("TheEnd");
            }
        }
        else if ((((Math.Abs(transformPosition.y - triggerStart1.transform.position.y) < 0.05 && Math.Abs(transformPosition.x - triggerStart1.transform.position.x) < 0.05))
            || ((Math.Abs(transformPosition.y - triggerStart2.transform.position.y) < 0.05 && Math.Abs(transformPosition.x - triggerStart2.transform.position.x) < 0.05))
            || ((Math.Abs(transformPosition.y - triggerStart3.transform.position.y) < 0.05 && Math.Abs(transformPosition.x - triggerStart3.transform.position.x) < 0.05)))
            && 
            (((Math.Abs((triggerStart1.transform.position.y - 2) - player2.transform.position.y) < 0.05 && Math.Abs(triggerStart1.transform.position.x - player2.transform.position.x) < 0.05))
            || ((Math.Abs((triggerStart2.transform.position.y - 2) - player2.transform.position.y) < 0.05 && Math.Abs(triggerStart2.transform.position.x - player2.transform.position.x) < 0.05))
            || ((Math.Abs((triggerStart3.transform.position.y - 2) - player2.transform.position.y) < 0.05 && Math.Abs(triggerStart3.transform.position.x - player2.transform.position.x) < 0.05)))
            && player2.GetComponent<Movement>().moveAllowed)
        {
            Debug.Log(player2.transform.position);
            Debug.Log(transformPosition);
            
            player2.GetComponent<Movement>().lap++;
            if (player2.GetComponent<Movement>().lap == GameParams.laps)
            {
                GameOver.winner = "Player 2 wins!";
                SceneManager.LoadScene("TheEnd");
            }
        }
    }
    
    public static void CurrentUIGameMaster()
    {
        if (player1.GetComponent<Movement>().moveAllowed)
        {
            PanelUIMainGameScript.CurrentPlayer = "Player 1";
            PanelUIMainGameScript.CurrentSpeed = player1.GetComponent<Movement>().currentSpeed;
            PanelUIMainGameScript.CurrentTires = player1.GetComponent<Movement>().tires;
            PanelUIMainGameScript.CurrentLap = player1.GetComponent<Movement>().lap + 1;
        }
        else if (player2.GetComponent<Movement>().moveAllowed)
        {
            PanelUIMainGameScript.CurrentPlayer = "Player 2";
            PanelUIMainGameScript.CurrentSpeed = player2.GetComponent<Movement>().currentSpeed;
            PanelUIMainGameScript.CurrentTires = player2.GetComponent<Movement>().tires;
            PanelUIMainGameScript.CurrentLap = player2.GetComponent<Movement>().lap + 1;
        }
    }
    
}
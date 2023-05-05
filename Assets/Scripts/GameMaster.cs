using Unity.VisualScripting;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    private static GameObject player1, player2;
    void Start()
    {
        // todo: rzut kostką kto zaczyna
        player1 = GameObject.Find("Player1");
        player2 = GameObject.Find("Player2");
        
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
}

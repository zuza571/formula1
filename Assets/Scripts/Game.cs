using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] private GameParams gameParams;
    [SerializeField] private List<GameObject> players = new List<GameObject>();
    [SerializeField] private List<PlayerParams> playerParamsList = new List<PlayerParams>();
    private Movement movement;

    void Start()
    {
        // todo: rzut kostką kto zaczyna
        gameParams.movementFields = 0;
        gameParams.currentPlayer = Instantiate(players[0]);
    }
    
    void Update()
    {
       Turn();
    }

    void Turn()
    {
        // todo 2: który tor mozliwy
        // todo 3: zakręty - obrót i zmiana pozycji
        
        
         // choose speed, change parameters, move
        // todo: odczytywanie prędkości z UI
        int chosenSpeed = 160;
        // todo: przekazanie aktualnego gracza
        movement = new Movement(playerParamsList[0]);
        gameParams.movementFields = movement.Move(chosenSpeed);
        
        var transformPosition = gameParams.currentPlayer.transform.position;
        while (gameParams.movementFields > 0)
        { 
            // left track
            if (Input.GetAxis("Horizontal") < 0 || Input.GetKey(KeyCode.A)) 
            { 
                if (Quaternion.Equals(gameParams.currentPlayer.transform.rotation.z, 0f)) 
                {
                    transformPosition.x += 1;
                    transformPosition.y -= 1;
                }
                else if (Quaternion.Equals(gameParams.currentPlayer.transform.rotation.z, 90f))
                {
                    transformPosition.x += 1;
                    transformPosition.y += 1;
                } 
                else if (Quaternion.Equals(gameParams.currentPlayer.transform.rotation.z, 180f))
                {
                    transformPosition.x -= 1;
                    transformPosition.y += 1;
                }
                else if (Quaternion.Equals(gameParams.currentPlayer.transform.rotation.z, 270f))
                {
                    transformPosition.x -= 1;
                    transformPosition.y -= 1;
                }
            } 
            // right track
            else if (Input.GetAxis("Horizontal") > 0 || Input.GetKey(KeyCode.D))
            {
                if (Quaternion.Equals(gameParams.currentPlayer.transform.rotation.z, 0f))
                {
                    transformPosition.x -= 1;
                    transformPosition.y -= 1;
                }
                else if (Quaternion.Equals(gameParams.currentPlayer.transform.rotation.z, 90f))
                {
                    transformPosition.x += 1;
                    transformPosition.y -= 1;
                } 
                else if (Quaternion.Equals(gameParams.currentPlayer.transform.rotation.z, 180f))
                {
                    transformPosition.x += 1;
                    transformPosition.y += 1;
                }
                else if (Quaternion.Equals(gameParams.currentPlayer.transform.rotation.z, 270f))
                {
                    transformPosition.x -= 1;
                    transformPosition.y += 1;
                }
            }
            // current track
            else if (Input.GetAxis("Vertical") > 0  || Input.GetKey(KeyCode.W))
            {
                if (Quaternion.Equals(gameParams.currentPlayer.transform.rotation.z, 0f))
                {
                    transformPosition.y -= 2;
                }
                else if (Quaternion.Equals(gameParams.currentPlayer.transform.rotation.z, 90f))
                {
                    transformPosition.x += 2;
                } 
                else if (Quaternion.Equals(gameParams.currentPlayer.transform.rotation.z, 180f))
                {
                    transformPosition.y += 2;
                }
                else if (Quaternion.Equals(gameParams.currentPlayer.transform.rotation.z, 270f))
                {
                    transformPosition.x -= 2;
                }
            }
            Debug.Log(gameParams.currentPlayer.transform.position);
            Debug.Log(transformPosition);
            gameParams.currentPlayer.transform.position = transformPosition;
            Debug.Log(gameParams.currentPlayer.transform.position);

            gameParams.movementFields--;
        }
        
        
    }
}

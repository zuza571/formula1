using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] private GameParams gameParams;
    [SerializeField] private List<GameObject> players = new List<GameObject>();
    [SerializeField] private List<PlayerParams> playerParamsList = new List<PlayerParams>();
    private Movement movement;
    private int currentPlayerIndex;
    private bool isTurnFinished = false;
    private PlayerParams currentPlayerParams;

    void Start()
    {
        // todo: rzut kostką kto zaczyna
        gameParams.movementFields = 0;
        currentPlayerIndex = 0;
        gameParams.currentPlayer = Instantiate(players[currentPlayerIndex]);
        currentPlayerParams = playerParamsList[0];
        EndTurn();
    }
    
    void Update()
    {
        if (isTurnFinished)
        {
            EndTurn();
        }
        else
        {
            Turn();
        }
    }
    
    private void UpdatePlayerParams()
    {
        currentPlayerParams = currentPlayerIndex == 0 ? playerParamsList[0] : playerParamsList[1];
        // currentPlayerParams.tires = 
        // currentPlayerParams.laps = 
    }

    public void EndTurn()
    {
        if (!isTurnFinished) {
            return; // ruch aktualnego gracza nie został zakończony, nie zmieniamy kolejki
        }
        isTurnFinished = false; // resetujemy flagę ruchu

        // change player
        currentPlayerIndex = (currentPlayerIndex + 1) % players.Count;
        gameParams.currentPlayer = Instantiate(players[currentPlayerIndex]);
        
        Turn();
    }
    
    void Turn()
    {
        isTurnFinished = false;
        // todo 2: który tor mozliwy
        // todo 3: zakręty - obrót i zmiana pozycji
        
        
        // choose speed, change parameters, move
        // todo: odczytywanie prędkości z UI
        int chosenSpeed = 160;
        
        // todo: przekazanie aktualnego gracza

        movement = new Movement(currentPlayerParams);
        gameParams.movementFields = movement.Move(chosenSpeed);
        
        Vector3 transformPosition = gameParams.currentPlayer.transform.position;
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
            if (gameParams.movementFields == 0) // jeśli ruch został wykonany, ustawiamy isTurnFinished na true
            {
                isTurnFinished = true;
            }
        }
        
        // wait for move
        if (!isTurnFinished)
        {
            return;
        }
        
        // switch to next player
        currentPlayerIndex = (currentPlayerIndex + 1) % 2;
        gameParams.currentPlayer = Instantiate(players[currentPlayerIndex]);
        
        UpdatePlayerParams();
    }
}

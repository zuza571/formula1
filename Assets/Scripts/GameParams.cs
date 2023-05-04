using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Game params")]
public class GameParams : ScriptableObject
{
    public GameObject currentPlayer;
    [Space][Range(0, 8)]
    public int movementFields;
    
    private void OnValidate()
    {
       
    }
}
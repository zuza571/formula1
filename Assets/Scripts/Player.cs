using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Game gameMaster;
    void Start()
    {
        gameMaster = FindObjectOfType<Game>();
    }

    public void MakeMove()
    {
        gameMaster.EndTurn();
    }
}

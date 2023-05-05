using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private GameMaster gameMaster;
    void Start()
    {
        gameMaster = FindObjectOfType<GameMaster>();
    }
}

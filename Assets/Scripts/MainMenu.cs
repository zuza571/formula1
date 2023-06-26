using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class MainMenu : MonoBehaviour
{
    public TextMeshProUGUI TextMeshProUGUIPlayers;
    public TextMeshProUGUI TextMeshProUGUITires;
    public TextMeshProUGUI TextMeshProUGUILaps;

    private void Start()
    {
        TextMeshProUGUIPlayers.SetText(GameParams.players.ToString());
        TextMeshProUGUITires.SetText(GameParams.tires.ToString());
        TextMeshProUGUILaps.SetText(GameParams.laps.ToString());
        
        GameParams.players = 2;
        GameParams.tires = 40;
        GameParams.laps = 2;
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    
    public void AddPlayers()
    {
        if (GameParams.players < 6)
        {
            GameParams.players++;
            TextMeshProUGUIPlayers.SetText(GameParams.players.ToString());
        }
    }
    
    public void SubtractPlayers()
    {
        if (GameParams.players > 2)
        {
            GameParams.players--;
            TextMeshProUGUIPlayers.SetText(GameParams.players.ToString());
        }
    }
    
    public void AddTires()
    {
        if (GameParams.tires < 100)
        {
            GameParams.tires++;
            TextMeshProUGUITires.SetText(GameParams.tires.ToString());
        }
    }
    
    public void SubtractTires()
    {
        if (GameParams.tires > 0)
        {
            GameParams.tires--;
            TextMeshProUGUITires.SetText(GameParams.tires.ToString());
        }
    }

    public void AddLaps()
    {
        if (GameParams.laps < 6)
        {
            GameParams.laps++;
            TextMeshProUGUILaps.SetText(GameParams.laps.ToString());
        }
    }
    
    public void SubtractLaps()
    {
        if (GameParams.laps > 1)
        {
            GameParams.laps--;
            TextMeshProUGUILaps.SetText(GameParams.laps.ToString());
        }
    }
}

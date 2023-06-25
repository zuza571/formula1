using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PanelUIMainGameScript : MonoBehaviour
{
    //todo: wpisac gracza który zaczyna
    public static string CurrentPlayer = "Player 1";
    public static int CurrentSpeed = 0;
    public static int CurrentTires = GameParams.tires;
    public static int CurrentLap = 1;
    public static int CurrentMovementPoints = 0;
    
    public TextMeshProUGUI TextMeshProUGUICurrentPlayer;
    public TextMeshProUGUI TextMeshProUGUICurrentSpeed;
    public TextMeshProUGUI TextMeshProUGUICurrentTires;
    public TextMeshProUGUI TextMeshProUGUICurrentLap;
    public TextMeshProUGUI TextMeshProUGUICurrentMovementPoints;
    void Update()
    {
        CurrentPlayerTextUpdate();
        CurrentSpeedTextUpdate();
        CurrentTiresTextUpdate();
        CurrentLapTextUpdate();
        CurrentMovementPointsUpdate();
    }

    // Update is called once per frame
    private void CurrentPlayerTextUpdate()
    {
        TextMeshProUGUICurrentPlayer.SetText(CurrentPlayer);
    }
    
    private void CurrentSpeedTextUpdate()
    {
        TextMeshProUGUICurrentSpeed.SetText(CurrentSpeed.ToString());
    }
    
    private void CurrentTiresTextUpdate()
    {
        TextMeshProUGUICurrentTires.SetText(CurrentTires.ToString());
    }
    
    private void CurrentLapTextUpdate()
    {
        TextMeshProUGUICurrentLap.SetText(CurrentLap.ToString());
    }

    private void CurrentMovementPointsUpdate()
    {
        TextMeshProUGUICurrentMovementPoints.SetText(CurrentMovementPoints.ToString());
    }
}

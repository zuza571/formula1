using TMPro;
using UnityEngine;

public class PanelUIMainGameScript : MonoBehaviour
{
    public static Color CurrentPlayerColor = Color.white;
    public static string CurrentPlayer = "Player -";
    public static int CurrentSpeed = 0;
    public static int CurrentTires = GameParams.tires;
    public static int CurrentLap = 1;
    public static int CurrentMovementPoints = 0;
    public static bool ActivateRollingTheDicePanel = true;
    
    public TextMeshProUGUI TextMeshProUGUICurrentPlayer;
    public TextMeshProUGUI TextMeshProUGUICurrentSpeed;
    public TextMeshProUGUI TextMeshProUGUICurrentTires;
    public TextMeshProUGUI TextMeshProUGUICurrentLap;
    public TextMeshProUGUI TextMeshProUGUICurrentMovementPoints;
    public GameObject RollingTheDicePanel;

    private void Start()
    {
        CurrentPlayerTextUpdate();
    }

    void Update()
    {
        CurrentPlayerTextUpdate();
        CurrentSpeedTextUpdate();
        CurrentTiresTextUpdate();
        CurrentLapTextUpdate();
        CurrentMovementPointsUpdate();
        RollingTheDice();
    }

    private void CurrentPlayerTextUpdate()
    {
        TextMeshProUGUICurrentPlayer.SetText(CurrentPlayer);
        TextMeshProUGUICurrentPlayer.color = CurrentPlayerColor;
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

    private void RollingTheDice()
    {
        RollingTheDicePanel.SetActive(ActivateRollingTheDicePanel);
    }
}

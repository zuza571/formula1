using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIScript : MonoBehaviour, IPointerClickHandler
{
    private GameObject speed0, speed40, speed80, speed120, speed160, speed200, speed240, speed280, speed320;
    private Dictionary<GameObject, int> speeds = new Dictionary<GameObject, int>();
    void Start()
    {
        speed0 = GameObject.Find("Button0");
        speed40 = GameObject.Find("Button40");
        speed80 = GameObject.Find("Button80");
        speed120 = GameObject.Find("Button120");
        speed160 = GameObject.Find("Button160");
        speed200 = GameObject.Find("Button200");
        speed240 = GameObject.Find("Button240");
        speed280 = GameObject.Find("Button280");
        speed320 = GameObject.Find("Button320");
        
        speeds.Add(speed0, 0);
        speeds.Add(speed40, 40);
        speeds.Add(speed80, 80);
        speeds.Add(speed120, 120);
        speeds.Add(speed160, 160);
        speeds.Add(speed200, 200);
        speeds.Add(speed240, 240);
        speeds.Add(speed280, 280);
        speeds.Add(speed320, 320);
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        int newSpeed = 0;
        string buttonName = eventData.pointerCurrentRaycast.gameObject.name;

        switch (buttonName)
        {
            case "Button0":
                newSpeed = speeds[speed0];
                break;
            case "Button40":
                newSpeed = speeds[speed40];
                break;
            case "Button80":
                newSpeed = speeds[speed80];
                break;
            case "Button120":
                newSpeed = speeds[speed120];
                break; 
            case "Button160":
                newSpeed = speeds[speed160];
                break; 
            case "Button200":
                newSpeed = speeds[speed200];
                break; 
            case "Button240":
                newSpeed = speeds[speed240];
                break; 
            case "Button280":
                newSpeed = speeds[speed280];
                break; 
            case "Button320":
                newSpeed = speeds[speed320];
                break;
            default:
                break;
        }
        
        
        Debug.Log(newSpeed);
    }
    
}

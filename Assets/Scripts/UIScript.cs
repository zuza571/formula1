using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIScript : MonoBehaviour, IPointerClickHandler
{
    private GameObject speed0, speed40, speed80, speed120, speed160, speed200, speed240, speed280, speed320;
    private Dictionary<GameObject, int> speeds = new Dictionary<GameObject, int>();

    private int chosenSpeed;
    private bool hasChosenSpeed;
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
        
        // EventTrigger for each button
        AddEventTrigger(speed0);
        AddEventTrigger(speed40);
        AddEventTrigger(speed80);
        AddEventTrigger(speed120);
        AddEventTrigger(speed160);
        AddEventTrigger(speed200);
        AddEventTrigger(speed240);
        AddEventTrigger(speed280);
        AddEventTrigger(speed320);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Application.Quit();
            Debug.Log("QUIT");
        }
    }

    private void AddEventTrigger(GameObject button)
    {
        // add EventTrigger if empty
        if (button.GetComponent<EventTrigger>() == null) { button.AddComponent<EventTrigger>(); }

        // add PointerClick event to EventTrigger
        EventTrigger trigger = button.GetComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener((data) => { OnPointerClick((PointerEventData)data); });
        trigger.triggers.Add(entry);
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        string buttonName = eventData.pointerPress.gameObject.name;
        hasChosenSpeed = true;

        switch (buttonName)
        {
            case "Button0":
                chosenSpeed = speeds[speed0];
                break;
            case "Button40":
                chosenSpeed = speeds[speed40];
                break;
            case "Button80":
                chosenSpeed = speeds[speed80];
                break;
            case "Button120":
                chosenSpeed = speeds[speed120];
                break; 
            case "Button160":
                chosenSpeed = speeds[speed160];
                break; 
            case "Button200":
                chosenSpeed = speeds[speed200];
                break; 
            case "Button240":
                chosenSpeed = speeds[speed240];
                break; 
            case "Button280":
                chosenSpeed = speeds[speed280];
                break; 
            case "Button320":
                chosenSpeed = speeds[speed320];
                break;
            default:
                break;
        }
    }

    public bool HasChosenSpeed
    {
        get
        {
            return hasChosenSpeed;
        }
    }

    public int ChosenSpeed
    {
        get
        {
            hasChosenSpeed = false;
            return chosenSpeed;
        }
    }
}

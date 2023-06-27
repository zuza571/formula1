using System.Collections.Generic;
using UnityEngine;

public class SpeedChanging : MonoBehaviour
{
    //private PlayerParams playerParams;

    // public SpeedChanging(PlayerParams playerParams)
    // {
    //     this.playerParams = playerParams;
    // }

    private static Dictionary<int, int> acceleration = new Dictionary<int, int>()
    {
        { 40, 0 },
        { 80, 0 },
        { 120, 0 },
        { 160, 1 },
        { 200, 2 },
        { 240, 4 },
        { 280, 8 },
        { 320, 16 }
    };

    private static Dictionary<int, int> braking = new Dictionary<int, int>()
    {
        { 40, 0 },
        { 80, 1 },
        { 120, 2 },
        { 160, 3 },
        { 200, 4 },
        { 240, 5 },
        { 280, 6 },
        { 320, 7 }
    };

    // change Player parameters because of speed changing
    public static int[] ChangeParams(int chosenSpeed, int currentSpeed, int tires)
    {
        if (currentSpeed < chosenSpeed)
        {
            int key = chosenSpeed - currentSpeed;
            tires -= acceleration[key];
        }
        else if (currentSpeed > chosenSpeed)
        {
            int key = currentSpeed - chosenSpeed;
            tires -= braking[key];
        }

        currentSpeed = chosenSpeed;
        
        int[] playerParams = { currentSpeed, tires };
        
        return playerParams;
    }

}

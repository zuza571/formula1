using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private PlayerParams playerParams;
    private SpeedChanging speedChanging;

    public Movement(PlayerParams playerParams)
    {
        this.playerParams = playerParams;
    }

    private Dictionary<int, int> fields = new Dictionary<int, int>()
    {
        { 0, 0 },
        { 40, 1 },
        { 80, 2 },
        { 120, 3 },
        { 160, 4 },
        { 200, 5 },
        { 240, 6 },
        { 280, 7 },
        { 320, 8 }
    };

    public int Move(int chosenSpeed)
    {
        speedChanging = new SpeedChanging(playerParams);
        speedChanging.ChangeParams(chosenSpeed);
        int movementFields = fields[playerParams.speed];

        return movementFields;
    }
}

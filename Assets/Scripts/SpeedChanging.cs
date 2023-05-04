using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public class SpeedChanging
    {
        private PlayerParams playerParams;

        public SpeedChanging(PlayerParams playerParams)
        {
            this.playerParams = playerParams;
        }

        private Dictionary<int, int> acceleration = new Dictionary<int, int>()
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

        private Dictionary<int, int> braking = new Dictionary<int, int>()
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
        public void ChangeParams(int chosenSpeed)
        {
            // todo: przekazanie konkretnego gracza
            if (playerParams.speed < chosenSpeed)
            {
                int key = chosenSpeed - playerParams.speed;
                playerParams.tires -= acceleration[key];
                playerParams.speed = chosenSpeed;
            }
            else if (playerParams.speed > chosenSpeed)
            {
                int key = playerParams.speed - chosenSpeed;
                playerParams.tires -= braking[key];
                playerParams.speed = chosenSpeed;
            }
        }
    }
}
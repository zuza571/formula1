using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Player params")]
public class PlayerParams : ScriptableObject
{
   [Range(0, 80)]
   public int tires = 40;
   [Space][Range(0, 320)]
   public int speed;
   [Space]
   public int laps = 1;


   private void OnValidate()
   {
      if (speed < 0)
      {
         speed = 0;
      }

      if (speed > 320)
      {
         speed = 320;
      }

      if (tires < 0)
      {
         tires = 0;
      }
   }
}

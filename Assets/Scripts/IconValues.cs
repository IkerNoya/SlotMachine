using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ScorePerIcon
{
   public int IconsMatched;
   public float Score;
}

[System.Serializable]
public struct SlotIcon
{
   public Sprite Icon;
   public List<ScorePerIcon> ScorePerIcons;

   public float GetSlotScore(int Matches)
   {
      foreach (ScorePerIcon Value in ScorePerIcons)
      {
         if (Matches == Value.IconsMatched)
         {
            return Value.Score;
         }
      }

      return 0.0f;
   }
}

[CreateAssetMenu(fileName = "IconValues", menuName = "Scriptable Objects/Icons")]
public class IconValues : ScriptableObject
{
   public List<SlotIcon> IconsData;

   public float GetScore(Sprite Icon, int Matches)
   {
      foreach (SlotIcon Slot in IconsData)
      {
         if (Slot.Icon.texture != Icon.texture)
            continue;

         return Slot.GetSlotScore(Matches);
      }
      return 0.0f;
   }
}

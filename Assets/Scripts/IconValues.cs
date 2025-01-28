using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct SlotIcon
{
   public Sprite Icon;
   public Dictionary<int, float> ScorePerIconsMatched;
}

[CreateAssetMenu(fileName = "IconValues", menuName = "Scriptable Objects/Icons")]
public class IconValues : ScriptableObject
{
   public List<SlotIcon> IconsData;
}

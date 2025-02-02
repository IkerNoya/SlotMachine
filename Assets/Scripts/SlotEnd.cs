using System;
using UnityEngine;
using UnityEngine.Serialization;

public class SlotEnd : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D other)
    {
        SlotColumn slotColumn = other.transform.parent.GetComponent<SlotColumn>();
        if (!slotColumn.IsSpinning) return;
        
        slotColumn.ResetSlotPosition(other.gameObject);
    }
}

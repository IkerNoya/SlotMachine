using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class SlotGrid : MonoBehaviour
{
    private List<GameObject> _triggeredObjects;
    
    void Start()
    {
        _triggeredObjects = new List<GameObject>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_triggeredObjects.Contains(other.gameObject))
            return;
        
        _triggeredObjects.Add(other.gameObject);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!_triggeredObjects.Contains(other.gameObject))
            return;
        
        _triggeredObjects.Remove(other.gameObject);
    }

    public void GetSlotRearrangementData(out Vector2 direction, out float distance)
    {
        direction = Vector3.zero;
        distance = 1000000f;

        if (_triggeredObjects.Count <= 0) return;
        
        Vector2 pos = transform.GetComponent<BoxCollider2D>().bounds.center;
        foreach (GameObject icon in _triggeredObjects)
        {
            float currentDistance = Vector2.Distance(pos, icon.transform.position);
            if (currentDistance > distance) continue;

            Vector2 iconPos = icon.transform.position;
            direction = pos - iconPos;
            distance = currentDistance;
        }
    }
    
    public List<Slot> GetWinningSlots()
    {
        List<Slot> winningSlots = new List<Slot>(_triggeredObjects.Count);
        for (int i = _triggeredObjects.Count - 1; i >= 0; i--)
        {
            winningSlots.Add(_triggeredObjects[i].GetComponent<Slot>());
        }

        return winningSlots;
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class SlotArea : MonoBehaviour
{
    [SerializeField] private SlotColumn slotColumn;
    private List<GameObject> triggeredObjects;
    
    void Start()
    {
        triggeredObjects = new List<GameObject>();
        slotColumn.TryStop += OnSpinEnded;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggeredObjects.Contains(other.gameObject))
            return;
        
        triggeredObjects.Add(other.gameObject);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!triggeredObjects.Contains(other.gameObject))
            return;
        
        triggeredObjects.Remove(other.gameObject);
    }

    void OnSpinEnded()
    {
        if (triggeredObjects.Count <= 0)
            return;
        
        if (triggeredObjects.Count == 1)
        {
            Vector2 objectPos = triggeredObjects[0].transform.position;
            Vector2 areaCenter = transform.GetComponent<BoxCollider2D>().bounds.center;
            Vector2 direction = areaCenter - objectPos;
            direction.Normalize();
            float distance = Vector2.Distance(areaCenter, objectPos);
            slotColumn.Rearrange(direction, distance);
        }
        else
        {
            float minDistance = 1000000f;
            Vector2 pos = transform.GetComponent<BoxCollider2D>().bounds.center;
            Vector2 direction = Vector2.zero;
            foreach (GameObject icon in triggeredObjects)
            {
                float distance = Vector2.Distance(pos, icon.transform.position);
                if (distance > minDistance)
                    continue;

                Vector2 iconPos = icon.transform.position;
                direction = pos - iconPos;
                minDistance = distance;
            }

            slotColumn.Rearrange(direction, minDistance);
        }
    }

    private void OnDisable()
    {
        slotColumn.TryStop -= OnSpinEnded;
    }
}

using System;
using UnityEngine;

public class SlotEnd : MonoBehaviour
{
    [SerializeField] private GameObject distanceReference;
    private float _distance = 0f;

    private void Start()
    {
        _distance = Vector2.Distance(distanceReference.transform.position, transform.position);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        Vector3 newPos = other.transform.position;
        newPos.y += _distance;
        other.transform.localPosition = newPos;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class SlotColumn : MonoBehaviour
{
    //Spawns the sprites in the stored order
    [SerializeField] private List<Sprite> spritesToSpawn;
    [SerializeField] private GameObject spritePrefab;
    [SerializeField] private float spawnOffsetY = 10.0f;
    [SerializeField] private float minSpinSpeed = 10.0f;
    [SerializeField] private float maxSpinSpeed = 10.0f;
    public bool IsSpinning { get; private set; }
    public float Speed { get; private set; }
    
    public Action StartSpin;
    public Action SpinEnded;
    public Action TryStop;

    private List<GameObject> _createdSprites;
    private GameManager _gameManager;

    private void Awake()
    {
        _createdSprites = new List<GameObject>(spritesToSpawn.Count);
    }

    void Start()
    {
        bool spawnedFirst = true;
        float offset = 0;
        foreach (Sprite sprite in spritesToSpawn)
        {
            if (!spawnedFirst)
            {
                offset += spawnOffsetY; 
            }
            
            GameObject newObject = Instantiate(spritePrefab, transform.position, Quaternion.identity);
            newObject.transform.SetParent(transform);
            newObject.GetComponent<SpriteRenderer>().sprite = sprite;
            Vector3 newPosition = transform.position;
            newPosition.y -= offset;
            newObject.transform.localPosition = newPosition;
            spawnedFirst = false;
            _createdSprites.Add(newObject);
            newObject.GetComponent<Slot>().StoppedMoving += SlotStopped;
        }
    }

    void SlotStopped()
    {
        bool allSlotsStopped = true;
        foreach (GameObject slot in _createdSprites)
        {
            if (slot.GetComponent<Slot>().IsMoving())
            {
                allSlotsStopped = false;
                break;
            }

            allSlotsStopped = true;
        }

        if (allSlotsStopped)
        {
            SpinEnded?.Invoke();
        }
    }

    public void Spin(float spinigTime)
    {
        Speed = Random.Range(minSpinSpeed, maxSpinSpeed);
        StartCoroutine(startSpin(spinigTime));
    }

    public void ResetSlotPosition(GameObject slot)
    {
        float minDistance = 10000;
        Vector2 spawnerPos = transform.position;
        float finalPosY = 0f;
        GameObject selectedObject = null;
        foreach (GameObject sprite in _createdSprites)
        {
            Vector2 spritePos = sprite.transform.position;
            float distance = Vector2.Distance(spawnerPos, spritePos);
            if (distance >= minDistance)
                continue;

            minDistance = distance;
            selectedObject = sprite;
        }

        if (selectedObject == null) return;

        Vector3 finalPos = selectedObject.transform.localPosition;
        finalPos.y += spawnOffsetY;
        slot.transform.localPosition = finalPos;
    }

    public void Rearrange(Vector2 direction, float distance)
    {
        foreach (GameObject sprite in _createdSprites)
        {
            sprite.GetComponent<Slot>().RearrangeStart(direction, distance);
        }
    }

    private void OnDisable()
    {
        foreach (GameObject slot in _createdSprites)
        {
            slot.GetComponent<Slot>().StoppedMoving -= SlotStopped;
        }
    }

    IEnumerator startSpin(float spinningTime)
    {
        StartSpin?.Invoke();
        IsSpinning = true;
        float selectedTime = spinningTime;
        yield return new WaitForSeconds(selectedTime);
        IsSpinning = false;
        TryStop?.Invoke();
    }
}

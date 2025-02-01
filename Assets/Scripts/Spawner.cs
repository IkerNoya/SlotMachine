using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    //Spawns the sprites in the stored order
    [SerializeField] private List<Sprite> spritesToSpawn;
    [SerializeField] private GameObject spritePrefab;
    [SerializeField] private float spawnOffsetY = 10.0f;
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
        }
    }
    
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;


public class GameManager : MonoBehaviour
{
    [SerializeField] private List<Sprite> icons;
    [SerializeField] private Vector2 spinTime;
    [SerializeField] private Button spinButton;
    private bool _isSpinning = false;
    public static GameManager Instance { get; private set; }

    public Action StartSpin;
    public Action SpinEnded;
    public float speed = 5f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public List<Sprite> GetIcons()
    {
        return icons;
    }
    
    public void OnSpin()
    {
        StartSpin?.Invoke();
        spinButton.interactable = false;
        StartCoroutine(SpinDelay());
    }
    
    IEnumerator SpinDelay()
    {
        float selectedTime = Random.Range(spinTime.x, spinTime.y);
        yield return new WaitForSeconds(selectedTime);
        SpinEnded?.Invoke();
        spinButton.interactable = true;
    }
}

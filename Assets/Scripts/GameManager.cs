using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;


public class GameManager : MonoBehaviour
{
    [SerializeField] private float spinDelay = 1f;
    [SerializeField] private float minSpinTime = 2f;
    [SerializeField] private float maxSpinTime = 4f;
    [SerializeField] private Button spinButton;
    [SerializeField] private List<SlotColumn> slots;
    public static GameManager Instance { get; private set; }

    public Action SlotMachineStarted;
    public Action SlotMachineStopped;
    
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

    private void Start()
    {
        foreach (SlotColumn slot in slots)
        {
            slot.SpinEnded += SlotColumnStopped;
        }
    }

    void SlotColumnStopped()
    {
        bool allColumnsSpinning = true;
        foreach (SlotColumn slot in slots)
        {
            if (slot.IsSpinning)
            {
                allColumnsSpinning = true;
                break;
            }

            allColumnsSpinning = false;
        }

        if (!allColumnsSpinning)
        {
            SlotMachineStopped?.Invoke();
            spinButton.interactable = true;
        }
    }

    public void OnSpin()
    {
        spinButton.interactable = false;
        SlotMachineStarted?.Invoke();
        StartCoroutine(SpinDelay());
    }
    
    IEnumerator SpinDelay()
    {
        float spinTime = Random.Range(minSpinTime, maxSpinTime);
        foreach (SlotColumn slot in slots)
        {
            slot.Spin(spinTime);
            yield return new WaitForSeconds(spinDelay);
        }
    }

    private void OnDisable()
    {
        foreach (SlotColumn slot in slots)
        {
            slot.SpinEnded -= SlotColumnStopped;
        }
    }
}

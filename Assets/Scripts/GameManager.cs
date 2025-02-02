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
    private Slot[,] winningSlots;

    private int stoppedColumns = 0;
    public static GameManager Instance { get; private set; }

    public Action SlotMachineStarted;
    public Action SlotMachineStopped;
    
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
        winningSlots = new Slot[5, 3];
        foreach (SlotColumn slot in slots)
        {
            slot.SpinEnded += SlotColumnStopped;
        }
    }

    void SlotColumnStopped(SlotColumn stoppedColumn)
    {
        stoppedColumns++;
        bool allColumnsStopped = stoppedColumns >= slots.Count;
        int index = 0;
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i] == stoppedColumn)
            {
                index = i;
            }            
        }

        for (int y = 0; y < winningSlots.GetLength(1); y++)
        {
            if (winningSlots[index, y] != null) return;
            winningSlots[index, y] = stoppedColumn.WinningSlots[y];
        }
        
        if (allColumnsStopped)
        {
            stoppedColumns = 0;
            CheckWinners();
            SlotMachineStopped?.Invoke();
            spinButton.interactable = true;
        }
    }

    void CheckWinners()
    {
        //TODO Check winners using grid
    }
    
    public void OnSpin()
    {
        for (int x = 0; x < winningSlots.GetLength(0); x++)
        {
            for (int y = 0; y < winningSlots.GetLength(1); y++)
            {
                winningSlots[x, y] = null;
            }
        }
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

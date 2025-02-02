using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public struct SlotsMatched
{
    public SpriteRenderer slot;
    public int matches;
}

public class GameManager : MonoBehaviour
{
    [SerializeField] private float spinDelay = 1f;
    [SerializeField] private float minSpinTime = 2f;
    [SerializeField] private float maxSpinTime = 4f;
    [SerializeField] private Button spinButton;
    [SerializeField] private List<SlotColumn> slots;
    [SerializeField] private ScoreValues scoreValues;
    [SerializeField] private string ActivateGlowPropertyName;
    
    private List<SlotsMatched> _slotsMatched;
    private Slot[,] winningSlots;
    
    public float Score { get; private set; }

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
        _slotsMatched = new List<SlotsMatched>();
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
            SetNewScore();
            SlotMachineStopped?.Invoke();
            spinButton.interactable = true;
        }
    }

    void SetNewScore()
    {
        if (_slotsMatched.Count <= 0)  return;
        
        foreach (SlotsMatched winners in _slotsMatched)
        {
            Score += scoreValues.GetScore(winners.slot.sprite, winners.matches);
        }
    }

    void CheckWinners()
    {
        for (int y = 0; y < winningSlots.GetLength(1); y++)
        {
            List<Slot> Row = new List<Slot>();
            for (int x = 0; x < winningSlots.GetLength(0); x++)
            {
                bool isIndexValid = x + 1 < winningSlots.GetLength(0);
                if (!isIndexValid)
                {
                    Row.Add(winningSlots[x, y]);
                    break;
                }

                Slot slot = winningSlots[x, y];
                Row.Add(slot);
                
                SpriteRenderer slotSprite = slot.GetComponent<SpriteRenderer>();
                SpriteRenderer nextSlotSprite = winningSlots[x + 1, y].GetComponent<SpriteRenderer>();
                if(slotSprite.sprite == nextSlotSprite.sprite || x >= 1)
                    slotSprite.material.SetInt(ActivateGlowPropertyName, 1);    
                
                if (slotSprite.sprite != nextSlotSprite.sprite) break;
            }
            
            if (Row.Count > 1)
            {
                SlotsMatched slotsMatched;
                slotsMatched.matches = Row.Count;
                slotsMatched.slot = Row[0].GetComponent<SpriteRenderer>();
                _slotsMatched.Add(slotsMatched);
            }
        }
    }
    
    public void OnSpin()
    {
        ResetValues();
        spinButton.interactable = false;
        SlotMachineStarted?.Invoke();
        StartCoroutine(SpinDelay());
    }

    void ResetValues()
    {
        Score = 0;
        _slotsMatched.Clear();
        for (int x = 0; x < winningSlots.GetLength(0); x++)
        {
            for (int y = 0; y < winningSlots.GetLength(1); y++)
            {
                if (winningSlots[x, y] != null)
                {
                    winningSlots[x,y].GetComponent<SpriteRenderer>().material.SetInt(ActivateGlowPropertyName, 0);
                }
                winningSlots[x, y] = null;
            }
        }
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

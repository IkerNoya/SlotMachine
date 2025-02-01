using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SlotMachine : MonoBehaviour
{
    [SerializeField] private Vector2 spinTime;
    [SerializeField] private Button spinButton;

    public delegate void StartSpin();
    public delegate void StopSpinning();
    
    
    
}

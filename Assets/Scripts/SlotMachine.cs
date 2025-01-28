using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SlotMachine : MonoBehaviour
{
    [SerializeField] private Button SpinButton;
    
    IEnumerator SpinDelay()
    {
        yield return new WaitForSeconds(3.0f);
        SpinButton.interactable = true;
    }

    public void OnSpin()
    {
        SpinButton.interactable = false;
        StartCoroutine(SpinDelay());
    }
}

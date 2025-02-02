using System.Collections;
using TMPro;
using UnityEngine;

public class WinScreen : MonoBehaviour
{
    [SerializeField] private GameObject UI;
    [SerializeField] private TextMeshProUGUI scoreText;
    void Start()
    {
        GameManager.Instance.SlotMachineStopped += OnSlotMachineStopped;
        UI.SetActive(false);
    }

    void OnSlotMachineStopped()
    {
        float score = GameManager.Instance.Score;
        if (score <= 0) return;

        scoreText.SetText(score.ToString());
        GameManager.Instance.audioManager.PlayWinSfx();
        UI.SetActive(true);
    }

    public void OnCloseButton()
    {
        UI.SetActive(false);
    }
     
    void OnDisable()
    {
        GameManager.Instance.SlotMachineStopped -= OnSlotMachineStopped;
    }
}

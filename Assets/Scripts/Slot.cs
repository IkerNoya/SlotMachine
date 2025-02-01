using System;
using UnityEngine;

public class Slot : MonoBehaviour
{
    private Rigidbody2D _rb;
    private GameManager _gameManager;
    private bool _isSpinning = false;
    private float _speed = -5f;
    void Awake()
    {
        _gameManager = GameManager.Instance;
        _gameManager.StartSpin += OnSpinStarted;
        _gameManager.SpinEnded += OnSpinEnded;
    }

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (!_isSpinning)
            return;

        _rb.linearVelocityY = _speed;
    }

    void OnSpinStarted()
    {
        _isSpinning = true;
    }

    void OnSpinEnded()
    {
        _rb.linearVelocityY = 0f;
        _isSpinning = false;
    }

    private void OnDisable()
    {
        _gameManager.StartSpin -= OnSpinStarted;
        _gameManager.SpinEnded -= OnSpinEnded;
    }
}

using System;
using UnityEngine;
using UnityEngine.Rendering;

public class Slot : MonoBehaviour
{
    [SerializeField] private float relocationSpeed = 1f;
    
    private Rigidbody2D _rb;
    private GameManager _gameManager;
    private SlotColumn _slotColumn;
    
    private Vector2 _targetLocation;
    private bool _isSpinning = false;
    private float _speed = 0;

    public Action StoppedMoving;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _gameManager = GameManager.Instance;
        _slotColumn = transform.parent.GetComponent<SlotColumn>();
        _slotColumn.StartSpin += OnSpinStarted;
        _slotColumn.TryStop += OnSpinEnded;
    }

    public bool IsMoving()
    {
        return _targetLocation.Equals(Vector2.zero) && !_isSpinning;
    }

    private void Update()
    {
        if (_isSpinning || _targetLocation.Equals(Vector2.zero))
            return;

        float distance = Vector2.Distance(transform.position, _targetLocation);
        if (distance > 0.001f)
        {
            Vector2 position = transform.position;
            float speed = relocationSpeed * Time.deltaTime;
            Vector2 newPos = Vector2.Lerp(position, _targetLocation, speed);
            transform.position = newPos;
            return;
        }
        
        transform.position = _targetLocation;
        StoppedMoving?.Invoke();
        _targetLocation = Vector2.zero;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (!_targetLocation.Equals(Vector2.zero))
        {
            Gizmos.DrawWireSphere(new Vector3(_targetLocation.x, _targetLocation.y, 0.0f), 1f);
        }
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, 1f);
    }

    private void FixedUpdate()
    {
        if (!_isSpinning)
            return;

        _rb.linearVelocityY = -_speed;
    }

    public void RearrangeStart(Vector2 direction, float distance)
    {
        Vector2 position = transform.position;
        _targetLocation =  position + (direction * distance);
    }

    void OnSpinStarted()
    {
        _isSpinning = true;
        _speed = _slotColumn.Speed;
    }

    void OnSpinEnded()
    {
        _rb.linearVelocityY = 0f;
        _isSpinning = false;
    }

    private void OnDisable()
    {
        _slotColumn.StartSpin -= OnSpinStarted;
        _slotColumn.TryStop -= OnSpinEnded;
    }
}

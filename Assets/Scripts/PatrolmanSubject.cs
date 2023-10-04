using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PatrolmanSubject : MonoBehaviour
{
    public event Action OnPlayerDetected;
    
    [SerializeField] private LayerMask _playerLayer;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _detectionDistance;
    [SerializeField] private float _arcAngle;
    [SerializeField] int _rayCount;
    
    private float _minXToPatrol = -9f;
    private float _maxXToPatrol = 9f;
    private float _minYToPatrol = -3f;
    private float _maxYToPatrol = 3f;
    private bool _isPlayerDetected = false;
    private Vector3 _targetPosition;
    
    
    private void Start()
    {
        _targetPosition = GetRandomPosition();
        InvokeRepeating("RotateRandomly", 5, 5);
    }
    
    private void FixedUpdate()
    {
        if (!_isPlayerDetected)
        {
            DetectPlayer();
            Patrol();
        }
    }

    private void DetectPlayer()
    {
        for (int i = 0; i < _rayCount; i++)
        {
            float angle = (transform.eulerAngles.z) - (_arcAngle / 2) + (_arcAngle / (_rayCount - 1)) * i;
            Vector3 direction = Quaternion.Euler(0, 0, angle) * Vector3.down;

            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, _detectionDistance, _playerLayer);
            
            if (hit.collider != null)
            {
                Debug.Log("Player detected!");
                OnPlayerDetected?.Invoke();
                _isPlayerDetected = true;
                break;
            }
            else
            {
                Debug.DrawRay(transform.position, direction * _detectionDistance, Color.yellow);
            }
        }
    }
    
    private void Patrol()
    {
        if (Vector3.Distance(transform.position, _targetPosition) <= Mathf.Epsilon)
        {
            _targetPosition = GetRandomPosition();
        }

        transform.position = Vector3.MoveTowards(transform.position, _targetPosition, _moveSpeed * Time.deltaTime);
    }

    private Vector3 GetRandomPosition()
    {
        float randomX = Random.Range(_minXToPatrol, _maxXToPatrol);
        float randomY = Random.Range(_minYToPatrol, _maxYToPatrol);
        return new Vector3(randomX, randomY, 0f);
    }
    
    private void RotateRandomly()
    {
        if (!_isPlayerDetected)
        {
            float randomAngle = Random.Range(0f, 360f);
            transform.rotation = Quaternion.Euler(0f, 0f, randomAngle);
        }
    }
}

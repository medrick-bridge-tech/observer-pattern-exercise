using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierObserver : MonoBehaviour
{
    [SerializeField] private PatrolmanSubject _patrolmanSubject;
    [Space]
    [SerializeField] private Transform _player;
    [SerializeField] private float _moveSpeed;

    private bool _isPlayerDetected;
    
    
    private void OnEnable()
    {
        if (_patrolmanSubject != null)
        {
            _patrolmanSubject.OnPlayerDetected += DetectPlayer;
        }
    }

    private void OnDisable()
    {
        if (_patrolmanSubject != null)
        {
            _patrolmanSubject.OnPlayerDetected -= DetectPlayer;
        }
    }

    private void Update()
    {
        if (_isPlayerDetected)
        {
            ChasePlayer();
        }
    }

    private void ChasePlayer()
    {
        transform.position = Vector3.Lerp(transform.position, _player.position, _moveSpeed * Time.deltaTime);
    }

    private void DetectPlayer()
    {
        _isPlayerDetected = true;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class SoldierObserver : MonoBehaviour
{
    [SerializeField] private CCTVSubject _cctvSubject;
    [Space]
    [SerializeField] private float _moveSpeed;

    private const float CHASE_DURATION_AFTER_PLAYER_LOST = 3f;
    
    private Transform _player;
    private Vector3 _initialPosition;
    private bool _isPlayerDetected;
    private float _chaseTimer;
    
    
    private void OnEnable()
    {
        _initialPosition = transform.position;
        
        if (_cctvSubject != null)
        {
            _cctvSubject.OnPlayerDetected += DetectPlayer;
            
            _cctvSubject.OnPlayerHidden += LosePlayer;
        }
    }

    private void OnDisable()
    {
        if (_cctvSubject != null)
        {
            _cctvSubject.OnPlayerDetected -= DetectPlayer;
            
            _cctvSubject.OnPlayerHidden -= LosePlayer;
        }
    }

    private void Update()
    {
        if (_isPlayerDetected)
        {
            ChasePlayer();
        }
        else
        {
            if (_chaseTimer > 0)
            {
                ChasePlayer();
                _chaseTimer -= Time.deltaTime;
            }
            else
            {
                GoBackAtPost();
            }
        }
    }

    private void ChasePlayer()
    {
        transform.position = Vector3.Lerp(transform.position, _player.position, _moveSpeed * Time.deltaTime);
    }

    private void GoBackAtPost()
    {
        transform.position = Vector3.Lerp(transform.position, _initialPosition, _moveSpeed * Time.deltaTime);
    }

    private void DetectPlayer(Transform player)
    {
        _isPlayerDetected = true;
        _player = player;
    }

    private void LosePlayer()
    {
        _isPlayerDetected = false;
        _chaseTimer = CHASE_DURATION_AFTER_PLAYER_LOST;
    }
}

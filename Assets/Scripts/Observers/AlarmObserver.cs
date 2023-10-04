using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlarmObserver : MonoBehaviour
{
    [SerializeField] private PatrolmanSubject _patrolmanSubject;
    [Space]
    [SerializeField] private AudioSource _audioSource;

    private const float BLINK_TIMER = 0.2f;
    
    private SpriteRenderer _spriteRenderer;
    private bool _isPlayerDetected;
    private float _blinkTimer = BLINK_TIMER;


    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    private void OnEnable()
    {
        if (_patrolmanSubject != null)
        {
            _patrolmanSubject.OnPlayerDetected += DetectPlayer;
            _patrolmanSubject.OnPlayerDetected += RaiseAlarm;
        }

        if (_isPlayerDetected)
        {
            _audioSource.Play();
        }
    }

    private void OnDisable()
    {
        if (_patrolmanSubject != null)
        {
            _patrolmanSubject.OnPlayerDetected -= DetectPlayer;
            _patrolmanSubject.OnPlayerDetected -= RaiseAlarm;
        }
        
        _audioSource.Pause();
    }
    
    private void Update()
    {
        if (_isPlayerDetected)
        {
            BlinkAlarm();
        }
    }

    private void RaiseAlarm()
    {
        _audioSource.Play();
    }
    
    private void BlinkAlarm()
    {
        _blinkTimer -= Time.deltaTime;

        if (_blinkTimer <= 0f)
        {
            _spriteRenderer.color = (_spriteRenderer.color == Color.red) ? Color.blue : Color.red;

            _blinkTimer = BLINK_TIMER;
        }
    }
    
    private void DetectPlayer()
    {
        _isPlayerDetected = true;
    }
}

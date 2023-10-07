using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class AlarmObserver : MonoBehaviour
{
    [SerializeField] private CCTVSubject _cctvSubject;
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
        if (_cctvSubject != null)
        {
            _cctvSubject.OnPlayerDetected += _ => DetectPlayer();
            _cctvSubject.OnPlayerDetected += _ => RaiseAlarm();
        }

        if (_isPlayerDetected)
        {
            _audioSource.Play();
        }
    }

    private void OnDisable()
    {
        if (_cctvSubject != null)
        {
            _cctvSubject.OnPlayerDetected -= _ => DetectPlayer();
            _cctvSubject.OnPlayerDetected -= _ => RaiseAlarm();
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

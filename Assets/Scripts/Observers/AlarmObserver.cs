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
    private const float ALARM_DURATION_AFTER_PLAYER_LOST = 3f;
    
    private SpriteRenderer _spriteRenderer;
    private bool _isPlayerDetected;
    private float _blinkTimer = BLINK_TIMER;
    private float _alarmTimer;


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
            
            _cctvSubject.OnPlayerHidden += LosePlayer;
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
            
            _cctvSubject.OnPlayerHidden -= LosePlayer;
        }

        _audioSource.Pause();
    }
    
    private void Update()
    {
        if (_isPlayerDetected)
        {
            BlinkAlarm();
        }
        else
        {
            if (_alarmTimer > 0f)
            {
                BlinkAlarm();
                _alarmTimer -= Time.deltaTime;
            }
            else
            {
                TurnOffAlarm();
            }
        }
    }

    private void RaiseAlarm()
    {
        if (_alarmTimer <= 0f)
        {
            _audioSource.Play();
        }
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

    private void TurnOffAlarm()
    {
        _spriteRenderer.color = Color.green;
        _audioSource.Pause();
    }
    
    private void DetectPlayer()
    {
        _isPlayerDetected = true;
    }

    private void LosePlayer()
    {
        _isPlayerDetected = false;
        _alarmTimer = ALARM_DURATION_AFTER_PLAYER_LOST;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlarmObserver : MonoBehaviour
{
    [SerializeField] private PatrolmanSubject _patrolmanSubject;
    [SerializeField] private AudioClip _alarmAudioClip;


    private void OnEnable()
    {
        if (_patrolmanSubject != null)
        {
            _patrolmanSubject.OnPlayerDetected += RaiseAlarm;
        }
    }

    private void OnDisable()
    {
        if (_patrolmanSubject != null)
        {
            _patrolmanSubject.OnPlayerDetected -= RaiseAlarm;
        }
    }

    private void RaiseAlarm()
    {
        AudioSource.PlayClipAtPoint(_alarmAudioClip, transform.position);
        GetComponent<SpriteRenderer>().color = Color.red;
    }
}

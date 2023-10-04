using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperObserver : MonoBehaviour
{
    [SerializeField] private PatrolmanSubject _patrolmanSubject;
    [Space]
    [SerializeField] private Transform _player;
    [SerializeField] private Transform _gun;
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private AudioClip _shootSound;
    [SerializeField] private float _shootingInterval;

    private float _lastShotTime;
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
            AimGunAtPlayer();
            Fire();
        }
    }
    
    private void AimGunAtPlayer()
    {
        Vector3 direction = _player.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    private void Fire()
    {
        if (Time.time - _lastShotTime >= _shootingInterval)
        {
            GameObject bulletObject = Instantiate(_bulletPrefab, _gun.position, Quaternion.identity);
            Bullet bullet = bulletObject.GetComponent<Bullet>();
            bullet.SetTargetPosition(_player.position);
            AudioSource.PlayClipAtPoint(_shootSound, transform.position);
            
            _lastShotTime = Time.time;
        }
    }

    private void DetectPlayer()
    {
        _isPlayerDetected = true;
    }
}

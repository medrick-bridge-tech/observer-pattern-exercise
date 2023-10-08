using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class SniperObserver : MonoBehaviour
{
    [SerializeField] private CCTVSubject _cctvSubject;
    [Space]
    [SerializeField] private Transform _gun;
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private AudioClip _shootSound;
    [SerializeField] private float _shootingInterval;
    
    private const float AIM_AND_FIRE_DURATION_AFTER_PLAYER_LOST = 3f;

    private Transform _player;
    private float _lastShotTime;
    private bool _isPlayerDetected;
    private float _aimAndFireTimer;
    private Quaternion _initialRotation;


    private void Start()
    {
        _initialRotation = transform.rotation;
    }

    private void OnEnable()
    {
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
            
            _cctvSubject.OnPlayerHidden += LosePlayer;
        }
    }

    private void Update()
    {
        if (_isPlayerDetected)
        {
            AimGunAtPlayer();
            Fire();
        }
        else 
        {
            if (_aimAndFireTimer > 0f)
            {
                AimGunAtPlayer();
                Fire();
                _aimAndFireTimer -= Time.deltaTime;
            }
            else
            {
                ResetRotation();
            }
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

    private void ResetRotation()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, _initialRotation, Time.deltaTime);
    }

    private void DetectPlayer(Transform player)
    {
        _isPlayerDetected = true;
        _player = player;
    }
    
    private void LosePlayer()
    {
        _isPlayerDetected = false;
        _aimAndFireTimer = AIM_AND_FIRE_DURATION_AFTER_PLAYER_LOST;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCTVSubject : MonoBehaviour
{
    public event Action<Transform> OnPlayerDetected;
    public event Action OnPlayerHidden; 

    [SerializeField] private LayerMask _playerLayer;
    [SerializeField] private float _detectionDistance;
    [SerializeField] private float _arcAngle;
    [SerializeField] int _rayCount;
    
    private bool _isPlayerDetected = false;
    
    
    private void FixedUpdate()
    {
        DetectPlayer();
    }
    
    private void DetectPlayer()
    {
        for (int i = 0; i < _rayCount; i++)
        {
            float angle = (transform.eulerAngles.z) - (_arcAngle / 2) + (_arcAngle / (_rayCount - 1)) * i;
            Vector3 direction = Quaternion.Euler(0, 0, angle) * Vector3.down;
            
            Debug.DrawRay(transform.position, direction * _detectionDistance, Color.yellow);
    
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, _detectionDistance, _playerLayer);
            
            if (hit.collider && !_isPlayerDetected)
            {
                Debug.Log("Player detected!");
                OnPlayerDetected?.Invoke(hit.transform);
                _isPlayerDetected = true;
                return;
            }
            else if (hit.collider && _isPlayerDetected)
            {
                return;
            }
            else if (!hit.collider && _isPlayerDetected && i == _rayCount -1)
            {
                Debug.Log("Player hidden!");
                OnPlayerHidden?.Invoke();
                _isPlayerDetected = false;
                return;
            }
        }
    }
}

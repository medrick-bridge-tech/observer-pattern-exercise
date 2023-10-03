using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    
    void Update()
    {
        var horizontalAxisValue = Input.GetAxis("Horizontal");
        var verticalAxisValue = Input.GetAxis("Vertical");
        transform.Translate(_moveSpeed * Time.deltaTime * new Vector2(horizontalAxisValue, verticalAxisValue));
        
        var clampedXPos = Mathf.Clamp(transform.position.x, -9.5f, 9.5f);
        var clampedYPos = Mathf.Clamp(transform.position.y, -4.5f, 4.5f);
        transform.position = new Vector2(clampedXPos, clampedYPos);
    }
}

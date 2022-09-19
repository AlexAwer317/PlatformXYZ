using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrew;

public class DrivingPlatform : MonoBehaviour
{
    [SerializeField] private float _speed;
    private Vector2 _direction;

    private void Update() 
    {
     _direction = new Vector2(transform.position.x * _speed * Time.deltaTime, transform.position.y);
     transform.Translate(_direction);

    
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        Debug.Log("COliZIYA");
       if(other.gameObject.CompareTag("Ground"))
       {
            _speed *= -1;
       }
    }

    private void OnCollisionEnter2D(Collision2D other) 
    {
        Debug.Log("IGROK NA PLATFORME");
        if(other.gameObject.CompareTag("Player"))
        {
            other.transform.parent = transform;
        }
    }

    private void OnCollisionExit2D(Collision2D other) 
    {
        if(other.gameObject.CompareTag("Player"))
        {
            other.transform.parent = null;
        }
    }
}

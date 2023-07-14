using System;
using UnityEngine;
using UnityEngine.Serialization;

public class ParallaxTest : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Player1Controller[] _player1Controllers;

    public float leftThreshold;
    public float rightThreshold;

    private float _inputXValue;
    
    private void Start()
    {
        _player1Controllers = FindObjectsOfType<Player1Controller>();
    }

    private void Update()                                                       
    {
        _inputXValue = Input.GetAxis("Horizontal");

        if (_inputXValue != 0)
        {
            foreach (var e in _player1Controllers)
            {
                e.Run();
            }

            Vector3 movement = new Vector3(_inputXValue, 0f, 0f) * (moveSpeed * Time.deltaTime);
            
            if (CheckThreshold(movement.x))
            {
                return;
            }
            transform.position += movement;
        }

        else
        {
            foreach (var e in _player1Controllers)
            {
                e.RunOff();
            }
        }
    }
    
    private bool CheckThreshold(float movementXValue)
    {
        if (transform.position.x + movementXValue < leftThreshold || transform.position.x + movementXValue> rightThreshold)
        {
            return true;
        }
        
        return false;
    }

    public float GetInputXValue()
    {
        return _inputXValue;
    }
}

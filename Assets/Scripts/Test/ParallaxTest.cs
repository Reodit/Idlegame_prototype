using System;
using UnityEngine;
using UnityEngine.Serialization;

public class ParallaxTest : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Player1Controller _player1Controllers;

    public float leftThreshold;
    public float rightThreshold;
    
    private void Update()                                                       
    {
        Vector3 movement = new Vector3(_player1Controllers.transform.position.x, 0f, 0f) * (moveSpeed * Time.deltaTime);
        
        if (CheckThreshold(movement.x))
        {
            return;
        }
        
        transform.position += movement;
    }
    
    private bool CheckThreshold(float movementXValue)
    {
        if (transform.position.x + movementXValue < leftThreshold || transform.position.x + movementXValue> rightThreshold)
        {
            return true;
        }
        
        return false;
    }
}

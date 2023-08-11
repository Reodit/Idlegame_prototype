using System.Collections.Generic;
using UnityEngine;


public class AutoBattle : MonoBehaviour
{
    [HideInInspector] public float battleSpeed = 5f;
    private Player1Controller[] _player1Controllers;
    
    [HideInInspector] public float leftThreshold;
    [HideInInspector] public float rightThreshold;

    private float _inputXValue;

    [HideInInspector] public List<Transform> mst;

    public static AutoBattle Instance { get; private set; }

    private AutoBattle() { }
    public Queue<Transform> monsterTransform;

    private void Start()
    {
        Instance = this;
        _player1Controllers = FindObjectsOfType<Player1Controller>();
        monsterTransform = new Queue<Transform>();
        foreach (var e in mst)
        {
            monsterTransform.Enqueue(e.GetComponent<Transform>());
        }
    }
    
    private void Update()                                                       
    {
        /*_inputXValue = Input.GetAxis("Horizontal");

        if (_inputXValue != 0)
        {
            foreach (var e in _player1Controllers)
            {
                e.Run();
            }

            Vector3 movement = new Vector3(_inputXValue, 0f, 0f) * (battleSpeed * Time.deltaTime);
            
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
        }*/
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


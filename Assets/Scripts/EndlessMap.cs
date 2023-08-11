using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class EndlessMap : MonoBehaviour
{
    [SerializeField] private Transform[] spriteTransforms;
    [SerializeField] private float speed;
    private void Update()
    {
        for (int i = 0; i < spriteTransforms.Length; i++)
        {
            float spriteOffset = Time.deltaTime * speed;
            
            spriteTransforms[i].position = new Vector3(spriteTransforms[i].position.x - spriteOffset, 0, 0 );        
            
            if (spriteTransforms[i].position.x < -15.9f)
            {
                spriteTransforms[i].position = new Vector3(15.9f, 0f, 0f);
            }   
        }
    }
}

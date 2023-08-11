using UnityEngine;

public class CharacterBounds : MonoBehaviour
{
    public Bounds bounds;
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(bounds.center, bounds.size);
    }
}
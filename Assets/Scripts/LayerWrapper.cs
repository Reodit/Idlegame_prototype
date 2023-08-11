using UnityEngine;

public class LayerWrapper : MonoBehaviour
{
    public Renderer[] layers;
    public float[] speed;

    public float layerWrapperSpeed;
    private Vector2[] _initialOffset;
    
    private void Awake()
    {
        _initialOffset = new Vector2[layers.Length];
        for (int i = 0; i < layers.Length; i++)
        {
            _initialOffset[i] = layers[i].sharedMaterial.GetTextureOffset("_MainTex");
        }
    }
    
    public void OffsetUpdate()
    {
        for (int i = 0; i < layers.Length; i++)
        {
            float offset = speed[i] * Time.deltaTime;
            _initialOffset[i] += new Vector2(offset, 0f);
            layers[i].sharedMaterial.SetTextureOffset("_MainTex", _initialOffset[i]);
        }
    }

    private void OnDestroy()
    {
        // 오브젝트가 파괴될 때 초기 offset으로 복구 (다른 인스턴스에 영향을 미치지 않도록)
        for (int i = 0; i < layers.Length; i++)
        {
            layers[i].sharedMaterial.SetTextureOffset("_MainTex", _initialOffset[i]);
        }
    }
}

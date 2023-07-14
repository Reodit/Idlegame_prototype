using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerWrapper : MonoBehaviour
{
    public Renderer[] layers;  // 무한 스크롤링을 적용할 레이어들의 Renderer 배열
    public float[] speed;  // 각 레이어의 스크롤 속도

    private Vector2[] _initialOffset;  // 각 레이어의 초기 offset 값
    public Transform targetTransform;  // 움직임을 기준으로 삼을 Transform

    private void Awake()
    {
        _initialOffset = new Vector2[layers.Length];
        for (int i = 0; i < layers.Length; i++)
        {
            _initialOffset[i] = layers[i].sharedMaterial.GetTextureOffset("_MainTex");
        }
    }
    
    private void Update()
    {
        if (targetTransform == null)
        {
            return;
        }

        for (int i = 0; i < layers.Length; i++)
        {
            Vector3 targetMovement = targetTransform.position - layers[i].transform.position;
            float offset = targetMovement.x * speed[i];

            Vector2 newOffset = _initialOffset[i] + new Vector2(offset, 0f);
            layers[i].sharedMaterial.SetTextureOffset("_MainTex", newOffset);
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

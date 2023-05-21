using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    [SerializeField] private MapManager.SceneID mapName;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")&&!Fade.isFade)
        {
            MapManager.Instance.CallFadeIn(mapName);//マップマネージャーからフェード処理とマップ移動処理を呼ぶ
        }
    }
}

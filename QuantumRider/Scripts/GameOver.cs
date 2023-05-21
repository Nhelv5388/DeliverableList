using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    [HideInInspector] public static MapManager.SceneID mapName;//ゲームオーバーになったときにMapManagerから直前のシーン名をもらってくる
    public void Retry()
    {
        MapManager.Instance.CallFadeIn(mapName);//フェード処理とシーン遷移
    }
}

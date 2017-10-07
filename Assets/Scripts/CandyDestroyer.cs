using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandyDestroyer : MonoBehaviour 
{
    public CandyHolder candyHolder;
    public int reward;
    public GameObject effectPrefab;
    public Vector3 effectRotation;

    private void OnTriggerEnter(Collider other)
    {
        // 指定数だけCandyのストックを増やす
        candyHolder.AddCandy(reward);

        if (other.gameObject.tag == "Candy")
        {
            // オブジェクトの削除
            Destroy(other.gameObject);

            if (effectPrefab != null)
            {
                // Candyのポジションにエフェクトを生成
                Instantiate(
                    effectPrefab,
                    other.transform.position,
                    Quaternion.Euler(effectRotation)
                );
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pusher : MonoBehaviour {

    Vector3 startPosition;

    public float amplitude;     // 移動量
    public float speed;         // 移動速度

	// Use this for initialization
	void Start () {
        startPosition = transform.localPosition;
		
	}
	
	// Update is called once per frame
	void Update () {
        // 変位を計算
        float z = amplitude * Mathf.Sin(Time.time * speed);     // 移動量計算

        // zを変位させたポジションに再設定
        transform.localPosition = startPosition + new Vector3(0, 0, z);
	}
}

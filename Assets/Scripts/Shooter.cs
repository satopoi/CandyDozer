using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour {

    const int SphereCandyFrequency = 3;
    const int MaxShotPower = 5;
    const int RecoverySeconds = 3;

    int sampleCandyCount;
    int shotPower = MaxShotPower;
    AudioSource shotSound;

    public GameObject[] candyPrefabs;
    public GameObject[] candySquarePrefabs;
    public CandyHolder candyHolder;

    public float shotSpeed;
    public float shotTorque;
    public float baseWidth;

	private GUIStyle style;

    void Start()
    {
		style = new GUIStyle();
		style.fontSize = 30;

        shotSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetButtonDown("Fire1")) Shot();
    }

    // キャンディのプレファブからランダムに１つ選ぶ
    GameObject SampleCandy(){
        GameObject prefab = null;
        // 特定の回数に１回丸いキャンディを選択する
        if (sampleCandyCount % SphereCandyFrequency == 0)
        {
            int index = Random.Range(0, candyPrefabs.Length);
            prefab = candyPrefabs[index];
        } else
        {
            int index = Random.Range(0, candySquarePrefabs.Length);
            prefab = candySquarePrefabs[index];
        }
        sampleCandyCount++;

        return prefab;
    }

    // 
    Vector3 GetInstantiatePosition()
    {
        // 画面サイズとINPUTの割合からキャンディ生成ポジションを計算
        float x = baseWidth * (Input.mousePosition.x / Screen.width) - (baseWidth / 2);
        return transform.position + new Vector3(x, 0, 0);
    }


    public void Shot()
    {
        // キャンディを生成できる条件外ならばShotしない
        if (candyHolder.GetCandyAmount() <= 0) return;
        if (shotPower <= 0) return;

        // プレファブからCandyオブジェクトを生成
        GameObject candy = (GameObject)Instantiate(
            SampleCandy(),
            GetInstantiatePosition(),
            Quaternion.identity
            );

        // 生成したCandyオブジェクトの親をCandyHolderに設定する
        candy.transform.parent = candyHolder.transform;

        // CandyオブジェクトのRigidBodyを取得し力と回転を加える
        Rigidbody candyRigidBody = candy.GetComponent<Rigidbody>();
        candyRigidBody.AddForce(transform.forward * shotSpeed);
        candyRigidBody.AddTorque(new Vector3(0, shotTorque, 0));

        // Candyのストックを消費
        candyHolder.ConstumeCandy();
        // ShotPowerを消費
        ConsumePower();

        // サウンドを再生
        shotSound.Play();
    }

    void OnGUI()
    {
        GUI.color = Color.black;

        // ShotPowerの残数を＋の数で表示
        string label = "";
        for (int i=0; i < shotPower; i++) label = label + "+";

        GUI.Label(new Rect(0, 30, 100, 30), label, style);
    }

    void ConsumePower()
    {
        // ShotPowerを消費すると同時に回復カウントスタート
        shotPower--;
        StartCoroutine(RecoverPower());
    }

    IEnumerator RecoverPower()
    {
        // 一定秒数待ったらShotPower回復
        yield return new WaitForSeconds(RecoverySeconds);
        shotPower++;
    }
}

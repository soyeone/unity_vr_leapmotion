using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spwanmanager : MonoBehaviour {

    public bool enableSpawn = false;
    public GameObject Enemy; //Prefab을 받을 public 변수 입니다.
    void SpawnEnemy()
    {
        float randomX = Random.Range(-0.5f, 0.5f); //적이 나타날 X좌표를 랜덤으로 생성해 줍니다.
        if (enableSpawn)
        {
            GameObject enemy = (GameObject)Instantiate(Enemy, new Vector3(randomX, 1.1f, 0f), Quaternion.identity); //랜덤한 위치와, 화면 제일 위에서 Enemy를 하나 생성해줍니다.
        }
    }

    // Use this for initialization
    void Start () {
        InvokeRepeating("SpawnEnemy", 3, 0.5f);

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class Spawner : MonoBehaviour
{
	//���� ��Ҹ� ���� ����
	public Transform[] spawnPoints;
	public SpawnData[] spawnData;
	public float levelTime;

	int level;
	//���� �ð��� ���� ����
	float timer;

	void Awake()
	{
		spawnPoints = GetComponentsInChildren<Transform>();
		levelTime = GameManager.instance.maxGameTime / spawnData.Length;
	}

	void Update()
	{
		if (!GameManager.instance.isLive)
			return;

		timer += Time.deltaTime;
		//�Ҽ����� ������ ��Ʈ������ ��ȯ
		level = Mathf.Min(Mathf.FloorToInt(GameManager.instance.gameTime / levelTime),spawnData.Length - 1);

		if(timer > spawnData[level].spawnDelay)
		{
			timer = 0;
			Spawn();
		}
	}

	void Spawn()
	{ 
		GameObject enemy = GameManager.instance.pool.Get(0);
		enemy.transform.position = spawnPoints[Random.Range(1, spawnPoints.Length)].position;
		enemy.GetComponent<Enemy>().Init(spawnData[level]);
	}
}

[System.Serializable]
public class SpawnData
{
	public float spawnDelay;
	public int spriteType;
	public int health;
	public float speed;
}

using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    //프리팹을 보관할 변수
    public GameObject[] prefabs;
    //풀을 담당하는 리스트
    List<GameObject>[] pools;

	void Awake()
	{
		//배열 초기화
		pools = new List<GameObject>[prefabs.Length];

		for (int i=0; i<pools.Length; i++)
		{
			//배열의 각 인덱스에 게임오브젝트의 배열을 선언
			pools[i] = new List<GameObject>();
		}
	}

	//오브젝트 풀 안에서 비활성화 되어있는 오브젝트를 탐색하여 가져오는 함수
	public GameObject Get(int index)
	{
		GameObject select = null;

		//비활성화 오브젝트 탐색하여 접근
		//발견하면 select 변수에 할당
		foreach(GameObject item in pools[index])
		{
			if (!item.activeSelf)
			{
				select = item;
				select.SetActive(true);
				break;
			}
		}

		//비활성화 오브젝트를 찾지 못했다면(전부 사용중이라면)
		//새롭게 생성하고 select 변수에 할당
		if(!select)
		{
			select = Instantiate(prefabs[index], transform);
			pools[index].Add(select);
		}
		return select;
	}
}

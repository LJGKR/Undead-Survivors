using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    //�������� ������ ����
    public GameObject[] prefabs;
    //Ǯ�� ����ϴ� ����Ʈ
    List<GameObject>[] pools;

	void Awake()
	{
		//�迭 �ʱ�ȭ
		pools = new List<GameObject>[prefabs.Length];

		for (int i=0; i<pools.Length; i++)
		{
			//�迭�� �� �ε����� ���ӿ�����Ʈ�� �迭�� ����
			pools[i] = new List<GameObject>();
		}
	}

	//������Ʈ Ǯ �ȿ��� ��Ȱ��ȭ �Ǿ��ִ� ������Ʈ�� Ž���Ͽ� �������� �Լ�
	public GameObject Get(int index)
	{
		GameObject select = null;

		//��Ȱ��ȭ ������Ʈ Ž���Ͽ� ����
		//�߰��ϸ� select ������ �Ҵ�
		foreach(GameObject item in pools[index])
		{
			if (!item.activeSelf)
			{
				select = item;
				select.SetActive(true);
				break;
			}
		}

		//��Ȱ��ȭ ������Ʈ�� ã�� ���ߴٸ�(���� ������̶��)
		//���Ӱ� �����ϰ� select ������ �Ҵ�
		if(!select)
		{
			select = Instantiate(prefabs[index], transform);
			pools[index].Add(select);
		}
		return select;
	}
}

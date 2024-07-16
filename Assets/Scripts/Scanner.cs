using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    public float scanRange; //���͸� Ž���� ����
    public LayerMask targetLayer; //���̾� ������ ���� ����
    public RaycastHit2D[] targets; //Ž���� ��ü���� ������ ������ �迭
    public Transform nearestTarget; //���� ����� Ÿ���� ��ġ�� ������ ����

	void FixedUpdate()
	{
		targets = Physics2D.CircleCastAll(transform.position, scanRange, Vector2.zero, 0, targetLayer);
		nearestTarget = GetNearest();
	}

	Transform GetNearest()
	{
		Transform nearTarget = null;
		float diff = 100;

		foreach(RaycastHit2D target in targets)
		{
			Vector3 myPos = transform.position;
			Vector3 targetPos = target.transform.position;
			float curDiff = Vector3.Distance(myPos, targetPos); //�÷��̾���� �Ÿ� ����

			if(curDiff < diff)
			{
				diff = curDiff; //�� ���� �Ÿ��� �ʱ�ȭ
				nearTarget = target.transform; //�� �Ÿ��� ����� Ÿ������ �ʱ�ȭ
			}
		}

		return nearTarget;
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    public float scanRange; //몬스터를 탐색할 범위
    public LayerMask targetLayer; //레이어 구별을 위한 변수
    public RaycastHit2D[] targets; //탐색한 물체들의 정보를 저장할 배열
    public Transform nearestTarget; //가장 가까운 타겟의 위치를 저장할 변수

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
			float curDiff = Vector3.Distance(myPos, targetPos); //플레이어와의 거리 측정

			if(curDiff < diff)
			{
				diff = curDiff; //더 작은 거리로 초기화
				nearTarget = target.transform; //더 거리가 가까운 타겟으로 초기화
			}
		}

		return nearTarget;
	}
}

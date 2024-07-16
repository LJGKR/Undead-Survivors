using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reposition : MonoBehaviour
{
	Collider2D coll;

	void Awake()
	{
		coll = GetComponent<Collider2D>();
	}
	void OnTriggerExit2D(Collider2D collision)
	{
		if (!collision.CompareTag("Area"))
			return;

		Vector3 playerPos = GameManager.instance.player.transform.position;
		Vector3 myPos = transform.position;

		

		switch (transform.tag)
		{
			case "Ground":
				//플레이어와 배경 필드의 각 좌표별 차이값을 절댓값으로 구함
				float diffX = playerPos.x - myPos.x;
				float diffY = playerPos.y - myPos.y;

				//플레이어가 바라보는 방향을 구함
				float dirX = diffX < 0 ? -1 : 1;
				float dirY = diffY < 0 ? -1 : 1;
				
				diffX = Mathf.Abs(diffX);
				diffY = Mathf.Abs(diffY);

				if (diffX > diffY)
				{
					//플레이어가 좌우로 더 많이 움직였다면 필드를 플레이어가 바라보는 방향 일정 거리 앞으로 재이동시킴
					transform.Translate(Vector3.right * dirX * 40);
				}
				else if(diffX < diffY)
				{
					transform.Translate(Vector3.up * dirY * 40);
				}
				break;
			case "Enemy":
				if (coll.enabled)
				{
					Vector3 dist = playerPos - myPos;
					Vector3 ran = new Vector3(Random.Range(-3, 3), Random.Range(-3, 3), 0);
					transform.Translate(ran + dist * 2);
				}
				break;
		}
	}
}

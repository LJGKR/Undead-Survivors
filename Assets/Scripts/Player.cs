using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
	public Vector2 inputVec;
	public Scanner scanner;
	public Hand[] hands;

	Rigidbody2D rigid;
	SpriteRenderer sprite;
	Animator anim;
	public RuntimeAnimatorController[] animCons;

	public float speed;

	void Awake()
	{
		rigid = GetComponent<Rigidbody2D>();
		sprite = GetComponent<SpriteRenderer>();
		anim = GetComponent<Animator>();
		scanner = GetComponent<Scanner>();
		hands = GetComponentsInChildren<Hand>(true);
	}

	void OnEnable()
	{
		speed *= Character.Speed;
		anim.runtimeAnimatorController = animCons[GameManager.instance.playerId];
	}

	void Update()
	{
		if (!GameManager.instance.isLive)
			return;

		//inputVec.x = Input.GetAxisRaw("Horizontal");
		//inputVec.y = Input.GetAxisRaw("Vertical");
	}
	void FixedUpdate()
	{
		if (!GameManager.instance.isLive)
			return;

		Vector2 nextVec = inputVec.normalized * speed * Time.fixedDeltaTime;
		rigid.MovePosition(rigid.position + nextVec);
	}

	void LateUpdate()
	{
		if (!GameManager.instance.isLive)
			return;

		anim.SetFloat("Speed", inputVec.magnitude);

		if (inputVec.x != 0)
		{
			sprite.flipX = inputVec.x < 0;
		}
	}

	void OnCollisionStay2D(Collision2D collision)
	{
		if (!GameManager.instance.isLive)
			return;

		GameManager.instance.health -= Time.deltaTime * 10;

		if(GameManager.instance.health < 0)
		{
			//플레이어의 자식 오브젝트를 그림자 뺴고 전부 비활성화
			for(int i=2; i < transform.childCount; i++)
			{
				transform.GetChild(i).gameObject.SetActive(false);
			}

			anim.SetTrigger("Dead");
			GameManager.instance.GameOver();
		}
	}

	//input 시스템을 이용한 움직임
	void OnMove(InputValue value)
	{
		inputVec = value.Get<Vector2>();
	}
}

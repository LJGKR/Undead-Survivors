using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	public float speed;
	public float health;
	public float maxHealth;
	public RuntimeAnimatorController[] animCon;
	public Rigidbody2D target; //플레이어

	bool isLive;

	Rigidbody2D rigid;
	Collider2D coll;
	Animator anim;
	SpriteRenderer sprite;

	WaitForFixedUpdate wait;

	void Awake()
	{
		rigid = GetComponent<Rigidbody2D>();
		sprite = GetComponent<SpriteRenderer>();
		anim = GetComponent<Animator>();
		coll = GetComponent<Collider2D>();
		wait = new WaitForFixedUpdate();
	}

	void FixedUpdate()
	{
		if (!GameManager.instance.isLive)
			return;

		if (!isLive || anim.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
			return;

		Vector2 dirVec = target.position - rigid.position;
		Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
		rigid.MovePosition(rigid.position + nextVec);
		rigid.velocity = Vector2.zero;
	}

	void LateUpdate()
	{
		if (!GameManager.instance.isLive)
			return;

		if (!isLive)
			return;

		sprite.flipX = target.position.x < rigid.position.x;
	}

	void OnEnable()
	{
		target = GameManager.instance.player.GetComponent<Rigidbody2D>();
		isLive = true;
		coll.enabled = true; //피격 범위 켜기
		rigid.simulated = true; //물리 연산 켜기
		sprite.sortingOrder = 2;
		anim.SetBool("Dead", false);
		health = maxHealth;
	}

	public void Init(SpawnData data)
	{
		anim.runtimeAnimatorController = animCon[data.spriteType];
		speed = data.speed;
		maxHealth = data.health;
		health = data.health;
	}

	void OnTriggerEnter2D(Collider2D collision)
	{
		if (!collision.CompareTag("Bullet") || !isLive)
			return;

		health -= collision.GetComponent<Bullet>().damage;
		StartCoroutine(KnockBack());

		if (health > 0)
		{
			//피격 효과
			anim.SetTrigger("Hit");
			AudioManager.instance.PlaySfx(AudioManager.Sfx.Hit);
		}
		else
		{
			//사망
			isLive = false;
			coll.enabled = false; //피격 범위 끄기
			rigid.simulated = false; //물리 연산 끄기
			sprite.sortingOrder = 1;
			anim.SetBool("Dead", true);

			//플레이어 정보 로직
			GameManager.instance.killCount++;
			GameManager.instance.GetExp();

			if(GameManager.instance.isLive)
			AudioManager.instance.PlaySfx(AudioManager.Sfx.Dead);
		}
	}

	IEnumerator KnockBack()
	{

		yield return wait; //fixedUpdate 1프레임만큼 딜레이
		Vector3 playerPos = GameManager.instance.player.transform.position;
		Vector3 dirVec = transform.position - playerPos;
		rigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse);
	}

	void Dead()
	{
		gameObject.SetActive(false);
	}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int id;
    public int prefabId;
    public float damage;
    public int count;
    public float speed;

    float timer;

    Player player;

	void Awake()
	{
        player = GameManager.instance.player;
	}

	void Update()
    {
		if (!GameManager.instance.isLive)
			return;

		switch (id)
		{
			case 0:
                transform.Rotate(Vector3.back * speed * Time.deltaTime);

				break;

			default:
                timer += Time.deltaTime;

                if(timer > speed)
                {
                    timer = 0;
                    Fire();
                }
				break;
		}
	}

    public void LevelUp(float damage, int count)
    {
        this.damage = damage * Character.Damage;
        this.count += count;

        if(id == 0)
            Arrangement();

        player.BroadcastMessage("ApplyGear",SendMessageOptions.DontRequireReceiver);
    }

    public void Init(ItemData data)
    {
        //무기들의 기본 세팅
        name = "Weapon " + data.itemId;
        transform.parent = player.transform;
        transform.localPosition = Vector3.zero;

        //데이터 세팅
        id = data.itemId;
        damage = data.baseDamage * Character.Damage;
        count = data.baseCount + Character.Count;

        for(int i=0; i<GameManager.instance.pool.prefabs.Length; i++)
        {
            if(data.projectile == GameManager.instance.pool.prefabs[i])
            {
                prefabId = i;
                break;
            }
        }

        switch (id)
        {
            case 0:
                //시계방향으로 돌리기 위해
                speed = 150 * Character.WeaponSpeed;
                Arrangement();

				break;

            default:
                //원거리 공격의 speed는 공격속도
                speed = 0.5f * Character.WeaponRate;
                break;
        }

        Hand hand = player.hands[(int)data.itemType]; //형변환 (enum의 인덱스값 활용)
        hand.sprite.sprite = data.hand;
        hand.gameObject.SetActive(true);

        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }

    //무기 배치
    void Arrangement()
    {
        for(int i=0; i < count; i++)
        {
            Transform bullet;
            if(i < transform.childCount)
            {
                //기존에 사용하고 있던 무기를 재활용 하여 먼저 배치
                bullet = transform.GetChild(i);
            }
            else
            {
                //카운트만큼 모자란 무기는 풀링에서 채우기
                bullet = GameManager.instance.pool.Get(prefabId).transform;
                bullet.parent = transform;
			}

			bullet.localPosition = Vector3.zero;
            bullet.localRotation = Quaternion.identity;

            //무기 수에 맞게 360도를 나누어 무기 배치
            Vector3 rotVec = Vector3.forward * 360 * i / count;
            bullet.Rotate(rotVec);
            bullet.Translate(bullet.up * 1.5f, Space.World);
            bullet.GetComponent<Bullet>().Init(damage, -100,Vector3.zero); //-1은 계속 관통
        }
    }

    void Fire()
    {
        if (!player.scanner.nearestTarget)
            return;

        Vector3 targetPos = player.scanner.nearestTarget.position; ;
        Vector3 dir = targetPos - transform.position;
        dir = dir.normalized;

        Transform bullet = GameManager.instance.pool.Get(prefabId).transform;
        bullet.position = transform.position;
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);
        bullet.GetComponent<Bullet>().Init(damage, count, dir);

		AudioManager.instance.PlaySfx(AudioManager.Sfx.Range);
	}
}

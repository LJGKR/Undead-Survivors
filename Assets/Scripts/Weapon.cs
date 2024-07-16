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
        //������� �⺻ ����
        name = "Weapon " + data.itemId;
        transform.parent = player.transform;
        transform.localPosition = Vector3.zero;

        //������ ����
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
                //�ð�������� ������ ����
                speed = 150 * Character.WeaponSpeed;
                Arrangement();

				break;

            default:
                //���Ÿ� ������ speed�� ���ݼӵ�
                speed = 0.5f * Character.WeaponRate;
                break;
        }

        Hand hand = player.hands[(int)data.itemType]; //����ȯ (enum�� �ε����� Ȱ��)
        hand.sprite.sprite = data.hand;
        hand.gameObject.SetActive(true);

        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }

    //���� ��ġ
    void Arrangement()
    {
        for(int i=0; i < count; i++)
        {
            Transform bullet;
            if(i < transform.childCount)
            {
                //������ ����ϰ� �ִ� ���⸦ ��Ȱ�� �Ͽ� ���� ��ġ
                bullet = transform.GetChild(i);
            }
            else
            {
                //ī��Ʈ��ŭ ���ڶ� ����� Ǯ������ ä���
                bullet = GameManager.instance.pool.Get(prefabId).transform;
                bullet.parent = transform;
			}

			bullet.localPosition = Vector3.zero;
            bullet.localRotation = Quaternion.identity;

            //���� ���� �°� 360���� ������ ���� ��ġ
            Vector3 rotVec = Vector3.forward * 360 * i / count;
            bullet.Rotate(rotVec);
            bullet.Translate(bullet.up * 1.5f, Space.World);
            bullet.GetComponent<Bullet>().Init(damage, -100,Vector3.zero); //-1�� ��� ����
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

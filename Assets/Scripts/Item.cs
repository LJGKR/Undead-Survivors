using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public ItemData data;
    public int level;
    public Weapon weapon;
	public Gear gear;

    Image icon;
    Text levelText;
    Text nameText;
    Text descText;

	void Awake()
	{
		icon = GetComponentsInChildren<Image>()[1];
        icon.sprite = data.itemIcon;

        Text[] texts = GetComponentsInChildren<Text>();
        levelText = texts[0];
		nameText = texts[1];
		descText = texts[2];
		nameText.text = data.itemName;
	}

	void OnEnable()
	{
		levelText.text = "Lv." + level;

		switch (data.itemType)
		{
			case ItemData.ItemType.Melee:
			case ItemData.ItemType.Range:
				descText.text = string.Format(data.itemDesc, data.damages[level] * 100, data.counts[level]);
				break;
			case ItemData.ItemType.Glove:
			case ItemData.ItemType.Shoes:
				descText.text = string.Format(data.itemDesc, data.damages[level] * 100);
				break;
			default:
				descText.text = string.Format(data.itemDesc);
				break;
		}
		
	}
	
	public void OnClick()
	{
		switch (data.itemType)
		{
			case ItemData.ItemType.Melee:
			case ItemData.ItemType.Range:
				if(level == 0)
				{
					GameObject newWeapon = new GameObject();
					weapon = newWeapon.AddComponent<Weapon>(); //���� ��ũ��Ʈ(������Ʈ) �߰�
					weapon.Init(data);
				}
				else
				{
					float nextDamage = data.baseDamage;
					int nextCount = 0;

					nextDamage += data.baseDamage * data.damages[level];
					nextCount += data.counts[level];

					weapon.LevelUp(nextDamage, nextCount);
				}
				level++;
				break;
			case ItemData.ItemType.Glove:
			case ItemData.ItemType.Shoes:
				if(level == 0)
				{
					GameObject newGear = new GameObject();
					gear = newGear.AddComponent<Gear>();
					gear.Init(data);
				}
				else
				{
					float nextRate = data.damages[level];
					gear.LevelUp(nextRate);
				}
				level++;
				break;
			case ItemData.ItemType.Heal:
				GameManager.instance.health = GameManager.instance.maxHealth;
				break;
		}

		if(level == data.damages.Length) //������ �ִ밡 �ȴٸ�
		{
			GetComponent<Button>().interactable = false; //��ư ��ȣ�ۿ� ��Ȱ��ȭ
		}
	}
}

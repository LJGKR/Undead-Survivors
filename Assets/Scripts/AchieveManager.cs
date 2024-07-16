using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchieveManager : MonoBehaviour
{
    public GameObject[] lockCharacter;
    public GameObject[] unlockCharacter;
    public GameObject uiNotice;

    enum Achieve { UnlockPotato, UnlockBean}
    Achieve[] achieves;

    WaitForSecondsRealtime wait;

	void Awake()
	{
        //�ʱ�ȭ
        achieves = (Achieve[])Enum.GetValues(typeof(Achieve));
        wait = new WaitForSecondsRealtime(5);

        if (!PlayerPrefs.HasKey("MyData"))
            Init();
	}

    void Init()
    {
        PlayerPrefs.SetInt("MyData", 1);

        foreach(Achieve achievement in achieves)
        {
			PlayerPrefs.SetInt(achievement.ToString(), 0);
		}
    }

	void Start()
	{
        UnlockCharacter();
	}

    void UnlockCharacter()
    {
        for(int i=0; i<lockCharacter.Length; i++)
        {
            string achieveName = achieves[i].ToString();
            bool isUnlock = PlayerPrefs.GetInt(achieveName) == 1;
            lockCharacter[i].SetActive(!isUnlock);
            unlockCharacter[i].SetActive(isUnlock);
        }
    }

	void LateUpdate()
    {
        foreach(Achieve achieve in achieves)
        {
            CheckAchieve(achieve);
        }
    }

    void CheckAchieve(Achieve achieve)
    {
        bool isAchieve = false;

        switch (achieve)
        {
            case Achieve.UnlockPotato:
                isAchieve = GameManager.instance.killCount >= 100;
                break;
            case Achieve.UnlockBean:
                isAchieve = GameManager.instance.gameTime == GameManager.instance.maxGameTime;
                break;
        }

        //�ر� ������ �޼��� ���� & �ر��� ���� �Ǿ����� ���� ���¶��
        if(isAchieve && PlayerPrefs.GetInt(achieve.ToString()) == 0)
        {
            //Int �� 1�� ����� �ر�
            PlayerPrefs.SetInt(achieve.ToString(), 1);

            for(int i=0; i<uiNotice.transform.childCount; i++)
            {
                bool isActive = i == (int)achieve;
                uiNotice.transform.GetChild(i).gameObject.SetActive(isActive);
            }

            StartCoroutine(NoticeRoutine());
        }
    }

    IEnumerator NoticeRoutine()
    {
        uiNotice.SetActive(true);

		AudioManager.instance.PlaySfx(AudioManager.Sfx.LevelUp);

		yield return wait;

        uiNotice.SetActive(false);
    }
}

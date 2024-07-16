using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    public bool isLeft;
    public SpriteRenderer sprite;

    SpriteRenderer playerSprite;

	Vector3 rightPos = new Vector3(0.35f, -0.15f, 0);
	Vector3 flipRightPos = new Vector3(-0.15f, -0.15f, 0);
	Quaternion leftRot = Quaternion.Euler(0, 0, -35);
	Quaternion FlipLeftRot = Quaternion.Euler(0, 0, -135);

	void Awake()
	{
		playerSprite = GetComponentsInParent<SpriteRenderer>()[1];
	}

	void LateUpdate()
	{
		bool isFlip = playerSprite.flipX;

		if (isLeft){
			transform.localRotation = isFlip ? FlipLeftRot : leftRot;
			sprite.flipY = isFlip;
			sprite.sortingOrder = isFlip ? 4 : 6;
		}
		else
		{
			transform.localPosition = isFlip ? flipRightPos : rightPos;
			sprite.flipX = isFlip;
			sprite.sortingOrder = isFlip ? 6 : 4;
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
	int playerSkill = 0;

    public void AddPlayerSkill(int value)
	{
		playerSkill += value;
	}

	public int GetPlayerSkill()
	{
		return playerSkill;
	}
}

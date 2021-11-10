using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
	UIManager uiManager;

	int playerSkill = 0;

	private void Awake()
	{
		uiManager = FindObjectOfType<UIManager>();
	}

	public void AddPlayerSkill(int value)
	{
		playerSkill += value;

		uiManager.GetSkillValueText().text = playerSkill.ToString();
	}

	public int GetPlayerSkill()
	{
		return playerSkill;
	}
}

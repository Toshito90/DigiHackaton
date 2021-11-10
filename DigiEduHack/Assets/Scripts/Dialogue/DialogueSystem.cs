using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueSystem : MonoBehaviour
{
    [SerializeField] Dialogue[] dialogues;

	int currentIndex = 0;

	GameObject player;

	DialogueUI dialogueUI;
	UIManager uiManager;

	private void Awake()
	{
		uiManager = FindObjectOfType<UIManager>();

		dialogueUI = uiManager.GetDialogueUI();
	}

	public void StartDialogue()
	{
		dialogueUI.gameObject.SetActive(true);
		dialogueUI.SetDialogueEntity(this);
		dialogueUI.SetDialogueProperties(currentIndex);

		player = GameObject.FindGameObjectWithTag("Player");
	}

	public void ResetIndex()
	{
		currentIndex = 0;
	}

	public void SetNextDialogue()
	{
		currentIndex++;
	}

	public void SetNewDialogueSet(Dialogue[] newDialogueSet)
	{
		dialogues = newDialogueSet;
	}

	public Dialogue GetCurrentDialogue(int index)
	{
		return dialogues[index];
	}

	public Dialogue GetCurrentDialogue()
	{
		return dialogues[currentIndex];
	}

	public int GetCurrentIndex()
	{
		return currentIndex;
	}

	public Dialogue[] GetDialogueList()
	{
		return dialogues;
	}

	public Dialogue GetNextDialogue()
	{
		currentIndex++;
		return dialogues[currentIndex];
	}

	public void SetOptionAnswer(int answerID)
	{
		if (player == null) return;

		Skill playerSkill = player.GetComponent<Skill>();

		int skillValue = dialogues[currentIndex].GetSkillReward(answerID);

		playerSkill.AddPlayerSkill(skillValue);
	}
}

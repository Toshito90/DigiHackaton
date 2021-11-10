using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeDialogueSet : MonoBehaviour
{
	[SerializeField] Dialogue[] dialogueSet;

	DialogueSystem dialogueSystem;

	private void Awake()
	{
		dialogueSystem = GetComponent<DialogueSystem>();
	}

	public void SetNewDialogueSet()
	{
		dialogueSystem.SetNewDialogueSet(dialogueSet);
	}

	public Dialogue[] GetDialogueSet()
	{
		return dialogueSet;
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] DialogueUI dialogueWindow = null;
	[SerializeField] TextMeshProUGUI skillValueText = null;

    public DialogueUI GetDialogueUI()
	{
		return dialogueWindow;
	}

	public TextMeshProUGUI GetSkillValueText()
	{
		return skillValueText;
	}
}

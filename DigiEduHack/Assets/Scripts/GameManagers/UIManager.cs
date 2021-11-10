using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] DialogueUI dialogueWindow = null;

    public DialogueUI GetDialogueUI()
	{
		return dialogueWindow;
	}
}

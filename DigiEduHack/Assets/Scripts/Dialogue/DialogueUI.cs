using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class DialogueUI : MonoBehaviour
{
	[SerializeField] TextMeshProUGUI actorNameText;
	[SerializeField] TextMeshProUGUI dialogueText;
	[SerializeField] RectTransform optionButtonsParent;
	[SerializeField] Button buttonNext;
	[SerializeField] Button optionButtonPrefab;

	DialogueSystem dialogueSystem;

	List<Button> buttonOptionsList;

	private void Awake()
	{
		buttonOptionsList = new List<Button>();
	}

	public void SetActorName(string actorName)
	{
		actorNameText.text = actorName;
	}

	public void SetDialogueText(string dialogueText)
	{
		this.dialogueText.text = dialogueText;
	}

	public void SetButtonNext(bool toShow)
	{
		buttonNext.gameObject.SetActive(toShow);
	}

	public void SetDialogueEntity(DialogueSystem dialogueSystem)
	{
		this.dialogueSystem = dialogueSystem;
	}

	public void SetDialogueProperties(int index)
	{
		if (dialogueSystem == null) return;

		Dialogue dialogue = dialogueSystem.GetCurrentDialogue(index);
		SetActorName(dialogue.GetActorName());
		SetDialogueText(dialogue.GetDialogueText());

		var dialogueAnswers = dialogue.GetAnswersOptions();
		if (dialogueAnswers.Length == 0)
		{
			SetButtonNext(true);
		}
		else
		{
			for(int i = 0; i < dialogueAnswers.Length; i++)
			{
				Button go = Instantiate(optionButtonPrefab, optionButtonsParent);

				go.GetComponentInChildren<TextMeshProUGUI>().text = dialogueAnswers[i];
				
				int closureIndex = i;
				go.onClick.AddListener(() => ButtonOptionClickedRef(closureIndex));

				buttonOptionsList.Add(go);
			}
		}
	}

	public void ButtonOptionClickedRef(int closureIndex)
	{
		dialogueSystem.SetOptionAnswer(closureIndex);
		NextButton();
	}

	public void NextButton()
	{
		if(buttonOptionsList.Count > 0)
		{
			foreach (Button button in buttonOptionsList)
			{
				Destroy(button.gameObject);
			}
			buttonOptionsList.Clear();
		}

		if (dialogueSystem.GetCurrentIndex() >= dialogueSystem.GetDialogueList().Length-1)
		{
			// We reached the end of the dialogue

			ChangeDialogueSet changeDialogueSet = dialogueSystem.GetComponent<ChangeDialogueSet>();
			if (changeDialogueSet != null && changeDialogueSet.GetDialogueSet().Length > 0)
			{
				changeDialogueSet.SetNewDialogueSet();
				dialogueSystem.ResetIndex();
			}

			InteractableComponent interactComp = dialogueSystem.GetComponent<InteractableComponent>();
			if(interactComp != null)
			{
				interactComp.SetSpawnFloatingText(true);
			}

			gameObject.SetActive(false);
			return;
		}

		SetNextDialogue();
	}

	private void SetNextDialogue()
	{
		dialogueSystem.SetNextDialogue();
		int index = dialogueSystem.GetCurrentIndex();
		SetDialogueProperties(index);
	}
}

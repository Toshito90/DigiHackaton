using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New DialogueText", menuName = "Dialogues/Dialogue", order = 0)]
public class Dialogue : ScriptableObject
{
    [SerializeField] string actorName;
    [SerializeField] string dialogueText;
    [SerializeField] string[] answerOptions;
    [SerializeField] Player.Stats[] skillReward;
    [SerializeField] bool[] toContinue;

    public string GetActorName()
	{
        return actorName;
    }

    public string GetDialogueText()
	{
        return dialogueText;
    }

    public string[] GetAnswersOptions()
	{
        return answerOptions;
    }

    public string GetAnswerOption(int index)
	{
        return answerOptions[index];
    }

    public Player.Stats GetSkillReward(int optionID)
	{
        if (skillReward.Length == 0) return null;

        return skillReward[optionID];
    }

    public bool HasSkillReward()
	{
        return skillReward.Length > 0;
    }

    public bool IsGoingToContinue(int optionID)
	{
        return toContinue[optionID];
    }

    public bool HasContinuationInfo()
	{
        return toContinue.Length > 0;
    }
}

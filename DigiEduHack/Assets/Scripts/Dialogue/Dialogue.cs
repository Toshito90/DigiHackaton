using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New DialogueText", menuName = "Dialogues/Dialogue", order = 0)]
public class Dialogue : ScriptableObject
{
    [SerializeField] string actorName;
    [SerializeField] string dialogueText;
    [SerializeField] string[] answerOptions;
    [SerializeField] int[] skillReward;

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

    public int GetSkillReward(int optionID)
	{
        return skillReward[optionID];
    }
}

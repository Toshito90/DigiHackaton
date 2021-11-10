using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] DialogueUI dialogueWindow = null;

	[Header("Stats Texts")]
	[SerializeField] TextMeshProUGUI conhecimentoValueText = null;
	[SerializeField] TextMeshProUGUI atencaoDetalheValueText = null;
	[SerializeField] TextMeshProUGUI iniciativaValueText = null;
	[SerializeField] TextMeshProUGUI comunicacaoVerbalValueText = null;
	[SerializeField] TextMeshProUGUI pensamentoAnaliticoValueText = null;

	public DialogueUI GetDialogueUI()
	{
		return dialogueWindow;
	}

	public TextMeshProUGUI GetConhecimentoValueText()
	{
		return conhecimentoValueText;
	}

	public TextMeshProUGUI GetAtencaoDetalheValueText()
	{
		return atencaoDetalheValueText;
	}

	public TextMeshProUGUI GetIniciativaValueText()
	{
		return iniciativaValueText;
	}

	public TextMeshProUGUI GetComunicacaoVerbalValueText()
	{
		return comunicacaoVerbalValueText;
	}

	public TextMeshProUGUI GetPensamentoAnaliticoValueText()
	{
		return pensamentoAnaliticoValueText;
	}
}

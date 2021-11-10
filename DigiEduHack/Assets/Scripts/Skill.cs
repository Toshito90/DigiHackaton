using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
	UIManager uiManager;

	Player.Stats playerSkill;

	private void Awake()
	{
		playerSkill = new Player.Stats();
		uiManager = FindObjectOfType<UIManager>();
	}

	public void AddPlayerSkill(Player.Stats stats)
	{
		playerSkill.atencaoDetalhe += stats.atencaoDetalhe;
		playerSkill.comunicacaoVerbal += stats.comunicacaoVerbal;
		playerSkill.conhecimento += stats.conhecimento;
		playerSkill.iniciativa += stats.iniciativa;
		playerSkill.pernsamentoAnalitico += stats.pernsamentoAnalitico;

		uiManager.GetConhecimentoValueText().text = playerSkill.conhecimento.ToString();
		uiManager.GetAtencaoDetalheValueText().text = playerSkill.atencaoDetalhe.ToString();
		uiManager.GetIniciativaValueText().text = playerSkill.iniciativa.ToString();
		uiManager.GetComunicacaoVerbalValueText().text = playerSkill.comunicacaoVerbal.ToString();
		uiManager.GetPensamentoAnaliticoValueText().text = playerSkill.pernsamentoAnalitico.ToString();
	}

	public Player.Stats GetPlayerSkill()
	{
		return playerSkill;
	}
}

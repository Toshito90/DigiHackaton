using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public enum StatsList
	{
        conhecimento,
        atencaoDetalhe,
        iniciativa,
        comunicacaoVerbal,
        pernsamentoAnalitico
    }

    [Serializable]
    public class Stats
	{
        public int conhecimento = 0;
        public int atencaoDetalhe = 0;
        public int iniciativa = 0;
        public int comunicacaoVerbal = 0;
        public int pernsamentoAnalitico = 0;
    }
}

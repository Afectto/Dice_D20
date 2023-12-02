using System.Collections.Generic;
using UnityEngine;

public class DiceChanger : MonoBehaviour
{
    [SerializeField]private DiceRoll dice;
    [SerializeField]private DiceRollAdvantage diceAdvantage1;
    [SerializeField]private DiceRollAdvantage diceAdvantage2;

    private void Start()
    {
        dice.gameObject.SetActive(true);
        diceAdvantage1.gameObject.SetActive(false);
        diceAdvantage2.gameObject.SetActive(false);
    }

    public void SetDiceActive()
    {
        if (!dice.isActiveAndEnabled)
        {
            dice.gameObject.SetActive(true);
            diceAdvantage1.gameObject.SetActive(false);
            diceAdvantage2.gameObject.SetActive(false);
        }
    }

    public void SetDiceAdvantageActive()
    {
        if (!diceAdvantage1.isActiveAndEnabled)
        {
            dice.gameObject.SetActive(false);
            diceAdvantage1.gameObject.SetActive(true);
            diceAdvantage2.gameObject.SetActive(true);
        }
    }

    public DiceRoll GetActiveDice()
    {
        return dice.enabled ? dice : null;
    }
    
    public List<DiceRollAdvantage> GetActiveDiceAdvantage()
    {
        List<DiceRollAdvantage> rezult = new List<DiceRollAdvantage>();
        if (diceAdvantage1.enabled)
        {
            rezult.Add(diceAdvantage1);
            rezult.Add(diceAdvantage2);
        }
        return rezult;
    }
}

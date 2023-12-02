using System.Collections;
using UnityEngine;

public class BuffAdvantage : Buff
{
    private DiceChanger _diceChanger;

    private void Start()
    {
        _diceChanger = FindObjectOfType<DiceChanger>();
        if (transform.parent.name == "BuffsList")
        {
            _diceChanger.SetDiceAdvantageActive();
            StartAdvantageAnimation();
        }
    }

    public void StartAdvantageAnimation()
    {
        var diсeAdvantage = _diceChanger.GetActiveDiceAdvantage();

        var dice1Advantage = diсeAdvantage[0];
        var dice2Advantage = diсeAdvantage[1];
        
        dice1Advantage.transform.position = new Vector3(0, 0, 90f);
        dice2Advantage.transform.position = new Vector3(0, 0, 90f);
        
        dice2Advantage.SetIsSecond();
        
        StartCoroutine(MoveDice(dice1Advantage.gameObject, new Vector3(10, 0, 90f), 0.5f));
        StartCoroutine(MoveDice(dice2Advantage.gameObject, new Vector3(-10, 0, 90f), 0.5f));
    }
    
    private IEnumerator MoveDice(GameObject dice ,Vector3 targetPosition, float timeToMove)
    {
        Vector3 startPosition = dice.transform.position;
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / timeToMove;
            dice.transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            yield return null;
        }
    }

    public override void StartAnimationValue()
    {
        InvokeTextMoveComplete(0);
    }
}

using System;
using System.Collections;
using UnityEngine;

public class BuffAdvantage : Buff
{
    private DiceRoll Dice;
    private GameObject Dice_1;
    private GameObject Dice_2;

    private void Start()
    {
        StartAdvantageAnimation();
    }

    public void StartAdvantageAnimation()
    {
        Dice = FindObjectOfType<DiceRoll>();
        GameObject prefabDiceAdvantage = Resources.Load<GameObject>("Prefabs/DiceAdvantage");
        
        var parent = Dice.transform.parent;
        Dice_1= Instantiate(prefabDiceAdvantage, parent);
        Dice_2= Instantiate(prefabDiceAdvantage, parent);

        var dice2Advantage = Dice_2.GetComponent<DiceRollAdvantage>();
        dice2Advantage.SetIsSecond();
        
        var position = Dice.transform.position;
        StartCoroutine(MoveDice(Dice_1.gameObject, position + new Vector3(10, 0, 0), 0.5f));
        StartCoroutine(MoveDice(Dice_2.gameObject, position + new Vector3(-10, 0, 0), 0.5f));
        Destroy(Dice.gameObject);
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

using System.Collections;
using TMPro;
using UnityEngine;

public class Buff : MonoBehaviour
{
    public string Name;
    public string Value;

    [SerializeField]private TextMeshProUGUI nameText;
    [SerializeField]private TextMeshProUGUI valueText;

    public delegate void TextMoveCompleteAction(int intValue);
    public static event TextMoveCompleteAction OnTextMoveComplete;
    
    private void Awake()
    {
        Initialize();
    }

    public void Initialize()
    {
        nameText.text = Name;
        valueText.text = Value;
    }

    public void StartAnimationValue()
    {
        TextMeshProUGUI copiedText = Instantiate(valueText, valueText.rectTransform.position, Quaternion.identity);
        copiedText.transform.SetParent(transform.parent.transform,false);
        // Задаем начальные параметры копии
        copiedText.text = valueText.text;
        copiedText.rectTransform.position = valueText.rectTransform.position;
        
        StartCoroutine(MoveText(copiedText, new Vector3(0,1,90f), 0.8f));
    }
    
    private IEnumerator MoveText(TextMeshProUGUI textToMove, Vector3 targetPosition, float timeToMove)
    {
        Vector3 startPosition = textToMove.rectTransform.position;
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / timeToMove;
            textToMove.rectTransform.position = Vector3.Lerp(startPosition, targetPosition, t);
            yield return null;
        } 
        
        int intValue = 0;
        if (int.TryParse(textToMove.text, out int parsedValue))
        {
            intValue = parsedValue;
        }
        
        // Увеличиваем значение выпавшее на кубике
        OnTextMoveComplete?.Invoke(intValue);
        // Уничтожаем текст после завершения перемещения
        Destroy(textToMove.gameObject);
    }
    
}
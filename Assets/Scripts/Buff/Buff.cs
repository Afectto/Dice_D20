using System.Collections;
using TMPro;
using UnityEngine;

public class Buff : MonoBehaviour
{
    public string Name;
    public string Value;

    [SerializeField]private TextMeshProUGUI nameText;
    [SerializeField]protected TextMeshProUGUI valueText;

    public delegate void TextMoveCompleteAction(int intValue);
    public static event TextMoveCompleteAction OnTextMoveComplete;
    
    private void Awake()
    {
        Initialize();
    }

    public virtual void Initialize()
    {
        nameText.text = Name;
        valueText.text = Value;
    }

    public virtual void StartAnimationValue()
    {
        TextMeshProUGUI copiedText = Instantiate(valueText, valueText.rectTransform.position, Quaternion.identity);
        copiedText.transform.SetParent(transform.parent.transform,false);
        // Задаем начальные параметры копии
        copiedText.text = valueText.text;
        copiedText.rectTransform.position = valueText.rectTransform.position;
        copiedText.enabled = true;
        
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
            // Увеличиваем значение выпавшее на кубике
            InvokeTextMoveComplete(intValue);
        }
        
        // Уничтожаем текст после завершения перемещения
        Destroy(textToMove.gameObject);
    }

    public void OnClick()
    {
        if (transform.parent.name == "CanAddBuffsList")
        {
            var canAddBuffsList = transform.parent.GetComponent<CanAddBuffsList>();
            canAddBuffsList.AddToBuffList(name);
            canAddBuffsList.RemoveToBuffList(gameObject);
        }
        else
        {
            var buffsList = transform.parent.GetComponent<BuffsList>();
            buffsList.RemoveBuff(gameObject);
        }
    }

    public static void InvokeTextMoveComplete(int value)
    {
        OnTextMoveComplete?.Invoke(value);
    }
}
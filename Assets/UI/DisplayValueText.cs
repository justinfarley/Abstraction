using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class DisplayValueText : MonoBehaviour
{
    public enum Value
    {
        Lives,
        Money,
        Round
    }
    [SerializeField] private TMP_Text _text;
    [SerializeField] private Value _value;
    private void Start()
    {
        StartCoroutine(InitEvents());
    }
    //Waits for the GameManager instance to not be null, THEN assigns the events
    private IEnumerator InitEvents()
    {
        yield return new WaitUntil(() => GameManager.Instance != null);
        AbstractLevel.LevelProperties props = GameManager.Instance.CurrentLevel.Properties;
        props.OnLivesAmountChanged += UpdateText;
        props.OnCashAmountChanged += UpdateText;
        PlayRound.OnRoundStarted += UpdateText; 
        GameManager.Instance.CurrentLevel.Properties = props;
        GameManager.Instance.CurrentLevel.Properties.OnCashAmountChanged?.Invoke();
    }
    private void UpdateText()
    {
        _text.text = (_value) switch
        {
            Value.Money => "Money: " + GameManager.Instance.CurrentLevel.Properties.Cash,
            Value.Lives => "Lives: " + GameManager.Instance.CurrentLevel.Properties.Lives,
            Value.Round => "Round: " + PlayRound.CurrentRound,
            _ => "AN ERROR OCCURRED....",
        };
    }
}

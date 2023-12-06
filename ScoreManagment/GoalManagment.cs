using JetBrains.Annotations;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class GoalManagment : NetworkBehaviour, IScore
{
    public UnityEvent GetScore;
    [SyncVar(hook = nameof(ScoreChanged))]
    public string scoreke;

    public TextMeshProUGUI scoreText;
    public void Score()
    {
        int temp = int.Parse(scoreText.text);
      scoreke = (temp + 1).ToString();

        GetScore?.Invoke();
        if(scoreText.text == "5")
        {
            Debug.Log("You Win");
        }
    }

    public void ScoreChanged(string _Old, string _New)
    {
        scoreText.text = _New;

    }
}

interface IScore
{
    void Score();
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;
public class ScoreSystem : NetworkBehaviour
{
    public TextMeshProUGUI scoreText;

    public TextMeshProUGUI player1, player2;
    public static ScoreSystem instance;

    [SyncVar(hook = nameof(OnNameToLocal))]
    public string localName;
    [SyncVar(hook = nameof(OnNameToClient))]
    public string clientName;
    [SyncVar(hook = nameof(OnPlayable))]
    public bool playable;
    private void Awake()
    {
        instance = this;
        playable = true;
    }
    public void OnPlayable(bool _Old, bool _New)
    {
        playable = _New;
    }
    public void OnNameToLocal(string _Old, string _New)
    {
            
            player1.text = _New;
        
    }
    public void OnNameToClient(string _Old,string _New)
    {
        
           player2.text = _New;
    }
    public void ScoreUpdate()
    {
        int score = int.Parse(scoreText.text);
        scoreText.text = score++.ToString();
    }
    
    
       
    
}

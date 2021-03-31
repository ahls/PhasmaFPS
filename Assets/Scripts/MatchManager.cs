using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameMode {deathMatch,rounds }
public class MatchManager : MonoBehaviour
{
    List<List<GameObject>> Teams = new List<List<GameObject>>(); // holds the list of teams, which holds the players
    private GameMode _gameMode;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void setup(GameMode whichMode)
    {
        _gameMode = whichMode;
        switch (whichMode)
        {
            case GameMode.deathMatch:
                Teams.Add(new List<GameObject>());//add two temas
                Teams.Add(new List<GameObject>());

                break;
            case GameMode.rounds:
                break;
            default:
                break;
        }
    }
}

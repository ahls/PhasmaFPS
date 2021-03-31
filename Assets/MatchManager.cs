using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameMode {deathMatch,rounds }
public class MatchManager : MonoBehaviour
{
    List<List<GameObject>> Teams = new List<List<GameObject>>(); // holds the list of teams, which holds the players


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
        switch (whichMode)
        {
            case GameMode.deathMatch:



                break;
            case GameMode.rounds:
                break;
            default:
                break;
        }
    }
}

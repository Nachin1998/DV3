using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : BaseEnemy
{
    new void Start()    
    {
        base.Start();      
    }

    new void Update()
    {
        base.Update();

        switch (GameManager.Instance.gameMode)
        {
            case GameManager.GameMode.None:
                break;

            case GameManager.GameMode.Survival:
                ChasePlayer();
                break;

            case GameManager.GameMode.HoldZone:
                SearchZone();
                break;

            default:
                break;
        }        
    }
}

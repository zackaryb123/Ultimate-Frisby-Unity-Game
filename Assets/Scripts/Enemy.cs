using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MovingObject {

    protected override void Start ()
    {
        GameManager.instance.AddEenemyToList(this);
        base.Start();
	}

    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        base.AttemptMove<T>(xDir, yDir);
    }

    public void MoveEnemy()
    {
        int yDir = -1;

        AttemptMove<Player>(0, yDir);
    }
}

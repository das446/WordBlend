using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class FreezePowerUp : PowerUp {

    public static event Action<int> FreezeTimer;

    public override void Init(Tile t) {
        t.FreezeTile();
    }

    public override void Submit(Tile t) {
        FreezeTimer(5);
        Debug.Log("Freeze");
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PowerUp : ScriptableObject {
    public Sprite background;
    public abstract void Submit(Tile t);
    public abstract void Init(Tile t);
}
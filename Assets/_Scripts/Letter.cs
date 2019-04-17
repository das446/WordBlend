using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class Letter : ScriptableObject {
    public Sprite image;

    public new string ToString => name;

}
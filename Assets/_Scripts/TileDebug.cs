using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileDebug : MonoBehaviour {

    [SerializeField] Text text;

    // Start is called before the first frame update
    void Start() {
        Tile.OnHover += UpdateDebug;
    }

    // Update is called once per frame
    void UpdateDebug(Tile t) {
        string locked = t.moveable? "not locked": "locked";
        text.text = t.letter.name + "," + t.pos.x + "," + t.pos.y + "," + locked;
    }
}
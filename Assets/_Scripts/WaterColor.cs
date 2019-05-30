using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterColor : MonoBehaviour {
    [SerializeField] SpriteRenderer sprite;
    [SerializeField] Color targetColor;
    [SerializeField] Color[] colors;
    [SerializeField] float speed = 1;

    void Start() {
        StartCoroutine(ChooseColor());
    }

    // Update is called once per frame
    void Update() {
        Color c = sprite.color;
        c = Color.Lerp(c, targetColor, Time.deltaTime * 0.05f);
        sprite.color = c;

        Vector2 v = transform.position;
        v.y -= Time.deltaTime * speed;
        transform.position = v;
    }

    IEnumerator ChooseColor() {
        while (true) {
            targetColor = colors.RandomItem();
            yield return new WaitForSeconds(15);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpriteSelector : MonoBehaviour
{
    public Sprite[] sprites;
    // Start is called before the first frame update
    void Awake()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        int index = Random.Range(0,sprites.Length);
        spriteRenderer.sprite = sprites[index];
    }
}

using UnityEngine;
using System.Collections;

public class SicBoAnimAdapter : MonoBehaviour
{
    public SpriteRenderer SpriteShow;
    public UI2DSprite TextureShow;
    private Sprite NowSprite = null;

    // Use this for initialization
    void Start()
    {
        SpriteShow.enabled = false;
        TextureShow.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.localScale != Vector3.one * 100)
            transform.localScale = Vector3.one * 100;
        if (NowSprite != TextureShow.sprite2D)
        {
            NowSprite = TextureShow.sprite2D;
            TextureShow.sprite2D = SpriteShow.sprite;
            TextureShow.color = SpriteShow.color;
            TextureShow.MakePixelPerfect();
        }
    }
}
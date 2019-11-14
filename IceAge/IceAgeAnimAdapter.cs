using UnityEngine;
using System.Collections;

public class IceAgeAnimAdapter : MonoBehaviour
{
    public Animator m_AnimControl;
    public SpriteRenderer SpriteShow;
    public UI2DSprite TextureShow;
    public Sprite NowSprite = null;
    public AudioSource BiteSound;

    // Use this for initialization
    void Start()
    {
        m_AnimControl = this.GetComponent<Animator>();
        SpriteShow = GetComponent<SpriteRenderer>();
        SpriteShow.enabled = false;
        TextureShow = GetComponent<UI2DSprite>();
        TextureShow.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        TextureShow.sprite2D = SpriteShow.sprite;
        TextureShow.color = SpriteShow.color;
        if (NowSprite != TextureShow.sprite2D)
        {
            TextureShow.MakePixelPerfect();
            NowSprite = TextureShow.sprite2D;
        }
    }

    public void PlayBiteSound()
    {
        if (BiteSound != null) BiteSound.Play();
    }
}

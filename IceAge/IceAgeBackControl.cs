using UnityEngine;
using System.Collections;

public class IceAgeBackControl : MonoBehaviour
{
    public static IceAgeBackControl Inst;
    public Animator m_AnimControl;
    public SpriteRenderer SpriteShow;
    public UI2DSprite TextureShow;
    public Sprite NowSprite = null;

    void Awake()
    {
        Inst = this;
    }

    // Use this for initialization
    void Start()
    {
        m_AnimControl = this.GetComponent<Animator>();
        SpriteShow = GetComponent<SpriteRenderer>();
        TextureShow = GetComponent<UI2DSprite>();
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
}
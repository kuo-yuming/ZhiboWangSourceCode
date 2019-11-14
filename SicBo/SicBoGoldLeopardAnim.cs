using UnityEngine;
using System.Collections;

public class SicBoGoldLeopardAnim : MonoBehaviour
{
    public Animator GoldLeopardAnim;    //Animator
    public SpriteRenderer SpriteShow;   //SpriteRenderer
    public UI2DSprite TextureShow;  //UI2DSprite
    public GameObject EffectBack;   //放射線背景
    public TweenScale EffectBackScale;  //放射線大小
    public TweenAlpha EffectBackAlpha;  //放射線透明度
    public GameObject MaskObject;   //遮罩
    public UITexture FlashTexture;  //閃光圖片
    public TweenAlpha FlashAlpha;   //閃光透明度    
    public AudioSource GoldLeopardSound;    //黃金豹音效

    public void ShowAnimation()
    {
        SpriteShow.enabled = true;  //顯示Sprite
        TextureShow.enabled = true;
        EffectBack.SetActive(true); //開啟放射線背景
        MaskObject.SetActive(true); //開啟遮罩
        EffectBackScale.ResetToBeginning();
        EffectBackScale.enabled = true; //開始放大放射線
        GoldLeopardSound.Play();    //播放音效
    }

    public void ShowSecondStep()
    {
        EffectBackAlpha.ResetToBeginning();
        EffectBackAlpha.enabled = true; //開啟放射線透明度 - 隱藏
    }

    public void ShowThirdStep()
    {
        FlashTexture.enabled = true;//開啟閃光圖片
        FlashAlpha.ResetToBeginning();
        FlashAlpha.enabled = true;  //開啟閃光透明度 -隱藏
        MaskObject.SetActive(false);//關閉遮罩
    }

    public void CloseAnimation()
    {
        SpriteShow.enabled = false; //關閉Sprite
        TextureShow.enabled = false;
        EffectBack.SetActive(false);//關閉放射線背景 並初始化
        EffectBackScale.enabled = false;
        EffectBackAlpha.enabled = false;
        MaskObject.SetActive(false);//關閉遮罩
        FlashTexture.enabled = false;   //閃光圖片
        FlashAlpha.enabled = false; //閃光透明度
        GoldLeopardAnim.SetBool("PlayAnim", false);  //播放黃金豹動畫
    }
}
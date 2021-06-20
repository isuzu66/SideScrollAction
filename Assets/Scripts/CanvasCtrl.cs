using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using UnityEngine.UI;
using DG.Tweening;


public class CanvasCtrl : MonoBehaviour
{


    private RectTransform rectTransform;


    private GameObject imageObject;

    private Image imageComponent;

    public GameObject textObject;

    private Text scoreText;

    public int Score = 0;


    private GameObject fadeObject;

    private RectTransform fadeRectT;




    // Start is called before the first frame update
    void Start()
    {
        imageObject = GameObject.Find("Image");
        imageComponent = imageObject.GetComponent<Image>();
        imageComponent.enabled = false;

        textObject = GameObject.Find("Text");
        scoreText = textObject.GetComponent<Text>();
        scoreText.text = "000000";

        fadeObject = GameObject.Find("Fade");
        fadeRectT = fadeObject.GetComponent<RectTransform>();

        fadeIn();

    }


    void displayTelop ( string telopName )
    {
        imageComponent.enabled = true;

        //Resourceからテクスチャを取得
        Texture2D telopImage = Resources.Load(telopName) as Texture2D;

        imageComponent.sprite = Sprite.Create(telopImage,
        new Rect(0, 0, telopImage.width, telopImage.height),
        Vector2.zero);

        imageComponent.SetNativeSize();
    }


    void addScore ( int score )
    {
        Score += score;

        scoreText.text = Score.ToString("000000");
    }


    void fadeIn()
    {
        fadeRectT.DOScale(new Vector3(1, 0, 1), 1.5f)
        .SetEase(Ease.InOutQuint);


    }

    void fadeOut()
    {
        fadeRectT.DOScale(new Vector3(1, 1, 1), 1.5f)
        .SetEase(Ease.InOutQuint);


    }


    // Update is called once per frame
    void Update()
    {
        
    }
}

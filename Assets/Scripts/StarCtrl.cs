using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;


public class StarCtrl : MonoBehaviour
{

    private SpriteRenderer spriteRenderer;

    private GameObject canvas;


    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        canvas = GameObject.Find("Canvas");
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void OnTriggerEnter2D( Collider2D col )
    {
        if( col.gameObject.name == "Player" )
        {
            Vector3 pos = transform.localPosition + Vector3.up * 1.5f;
            transform.DOLocalMove(pos, 0.25f);
            transform.DOLocalRotate(new Vector3(0, 180, 0), 1.0f);
            spriteRenderer.DOFade(0, 1.5f);

            canvas.SendMessage("addScore" , 150 );

            GetComponent<BoxCollider2D>().enabled = false;



            Invoke("DestroyObject", 2);
        }
    }

    void DestroyObject()
    {
        Destroy(this.gameObject);
    }


}



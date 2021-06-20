using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrexCtrl : MonoBehaviour
{

    private Animator anim;

    private SpriteRenderer spRenderer;

    private Rigidbody2D rb2d;

    private GameObject player;


    public GameObject fxhit;

    public float speed = 25;


    //当たり判定

    private HitChecker sChecker;//横の当たり判定

    private HitChecker gChecker;//地面の当たり判定


    private bool isAttack = false;

    private bool isIdle = false;

    private bool isDead = false;



    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        anim = GetComponent<Animator>();
        spRenderer = GetComponent<SpriteRenderer>();
        rb2d = GetComponent<Rigidbody2D>();

        //横のチェック
        sChecker = transform.Find("sideChecker").gameObject.GetComponent<HitChecker>();
        //下のチェック
        gChecker = transform.Find("groundChecker").gameObject.GetComponent<HitChecker>();

    }

    // Update is called once per frame
    void Update()
    {
        float x = 1;

        //Debug.Log(this.transform.eulerAngles.y);

        if ( this.transform.eulerAngles.y == 180 )
        {
            x = -1;
        }
        else
        {
            x = 1;
        }

        CheckValue();

        if( sChecker.isPlayerHit )
        {
            if( !isAttack )
            {
                StartCoroutine("Attack");
            }
            
        }

        if( !isIdle & !isAttack & !isDead )
        {
            anim.SetBool("isWalk", true);
            rb2d.AddForce(Vector2.right * x * speed);
        }
        else
        {
            anim.SetBool("isWalk", false);
            rb2d.velocity = new Vector2(0, 0);
        }

    }


    private void OnCollisionEnter2D(Collision2D col)
    {
        if( col.gameObject.name == "Player" )
        {
            StartCoroutine("Dead");
            anim.SetTrigger("trgDead");
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<CircleCollider2D>().enabled = false;

        }
    }



    private void CheckValue()
    {
        //地面にヒットしていない時　かつ　待機状態ではない時
        if( !gChecker.isGroundHit & !isIdle )
        {
            gChecker.isGroundHit = true;
            StartCoroutine("ChangeRotate");

        }

        if( sChecker.isEnemyHit & !isIdle )
        {
            sChecker.isEnemyHit = false;
            StartCoroutine("ChangeRotate");

        }
        if (sChecker.isGroundHit & !isIdle)
        {
            sChecker.isGroundHit = false;
            StartCoroutine("ChangeRotate");

        }

    }



    IEnumerator ChangeRotate()
    {
        isIdle = true;
        
        yield return new WaitForSeconds(2.0f);

        isIdle = true;
        if (this.transform.eulerAngles.y == 180)
        {
            this.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            this.transform.rotation = Quaternion.Euler(0, 180, 0);
        }

        isIdle = false;
    }


    IEnumerator Attack()
    {
        isAttack = true;
        anim.SetTrigger("trgAttack");
        yield return new WaitForSeconds(2.0f);
        
        isAttack = false;
    }

    IEnumerator Dead()
    {
        isDead = true;

        yield return new WaitForSeconds(1.5f);

        Instantiate(fxhit, transform.position, transform.rotation);

        Destroy(this.gameObject);

    }


}

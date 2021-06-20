using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    public float speed = 5;

    public float jumpForce = 400f;


    public LayerMask groundLayer;

    private Rigidbody2D rb2d;

    private Animator anim;

    private SpriteRenderer spRenderer;

    private bool isGround;

    private bool isSloped;

    private bool isDead = false;


    private GameObject canvas;


    // Start is called before the first frame update
    void Start()
    {
        this.rb2d = GetComponent<Rigidbody2D>();
        this.anim = GetComponent<Animator>();
        this.spRenderer = GetComponent<SpriteRenderer>();
        canvas = GameObject.Find("Canvas");

        Sound.LoadSe("dead", "dead");

    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxisRaw("Horizontal"); //左-1・何もしない0・右1

        //スプライトの向きを変える
        if( x < 0)
        {
            spRenderer.flipX = true;
        }
        else if( x > 0 )
        {
            spRenderer.flipX = false;
        }

        anim.SetFloat("Speed", Mathf.Abs(x * speed)); //歩くアニメーション

        if (!isDead)
        {
            rb2d.AddForce(Vector2.right * x * speed); //横方向に力を加える 
        }


        if(isSloped)
        {
            this.gameObject.transform.Translate(0.05f * x, 0.0f, 0.0f);
        }


        if( Input.GetButtonDown("Jump") & isGround )
        {
            anim.SetBool("isJump", true);
            rb2d.AddForce(Vector2.up * jumpForce);
        }

        if( isGround ) //地面にいるときはジャンプモーションOFF
        {
            anim.SetBool("isJump", false);
            anim.SetBool("isFall", false);
        }


        float velX = rb2d.velocity.x;
        float velY = rb2d.velocity.y;

        if ( velY > 0.5f ) //velocityが上向に働いていたらジャンプ
        {
            anim.SetBool("isJump", true);
        }
        if (velY < -0.1f) //velocityが下向に働いていたら落ちる
        {
            anim.SetBool("isFall", true);
        }

        if ( Mathf.Abs(velX) > 5 )
        {
            if( velX > 5.0f )
            {
                rb2d.velocity = new Vector2( 5.0f, velY);
            }
            if (velX < -5.0f)
            {
                rb2d.velocity = new Vector2( -5.0f, velY);
            }
        }

    }

    private void FixedUpdate()
    {
        isGround = false;

        float x = Input.GetAxisRaw("Horizontal");

        //自分の立っている場所
        Vector2 groundPos = new Vector2( transform.position.x , transform.position.y );

        //地面の判定エリア
        Vector2 groundArea = new Vector2( 0.5f , 0.4f );

        Vector2 wallArea1 = new Vector2( x * 0.8f, 1.5f);
        Vector2 wallArea2 = new Vector2( x * 0.3f, 1.0f);

        Vector2 wallArea3 = new Vector2(x * 1.5f, 0.6f);
        Vector2 wallArea4 = new Vector2(x * 1.0f, 0.1f);

        Debug.DrawLine(groundPos + wallArea1, groundPos + wallArea2, Color.red);
        Debug.DrawLine(groundPos + wallArea3, groundPos + wallArea4, Color.red);

        isGround = Physics2D.OverlapArea(groundPos + groundArea, groundPos - groundArea, groundLayer);


        bool area1 = false;
        bool area2 = false;

        area1 = Physics2D.OverlapArea(groundPos + wallArea1, groundPos + wallArea2, groundLayer);
        area2 = Physics2D.OverlapArea(groundPos + wallArea3, groundPos + wallArea4, groundLayer);



        if( !area1 & area2 )
        {
            isSloped = true;
        }
        else
        {
            isSloped = false;
        }


        //Debug.Log(isSloped);

    }


    IEnumerator Dead()
    {
        anim.SetBool("isDamage", true);

        yield return new WaitForSeconds(0.5f);

        canvas.SendMessage("displayTelop", "gameover");

        rb2d.AddForce(Vector2.up * jumpForce);
        GetComponent<CircleCollider2D>().enabled = false;
        Sound.PlaySe("dead", 0);

        yield return new WaitForSeconds(1.0f);

        canvas.SendMessage("fadeOut");

    }


    void OnTriggerEnter2D(Collider2D col) //重なったとき
    {
        if (col.gameObject.tag == "Enemy")
        { 
            isDead = true;
            StartCoroutine("Dead");
        }

        if (col.gameObject.tag == "Finish")
        {
            GameObject.Find("ClearPoint").GetComponent<BoxCollider2D>().enabled = false;
            //this.enabled = false;

            canvas.SendMessage("displayTelop", "clear");
            canvas.SendMessage("fadeOut");
        }





    }


    void OnCollisionEnter2D(Collision2D col) //乗ったとき
        { 
         if( col.gameObject.tag == "Enemy" )
            {
            anim.SetBool("isJump", true);
            rb2d.AddForce(Vector2.up * jumpForce);
            }

        if(col.gameObject.tag == "Damage")
            {
            isDead = true;

            StartCoroutine("Dead");

            //Debug.Log("damage");
            }
        }

}





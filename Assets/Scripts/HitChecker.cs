using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitChecker : MonoBehaviour
{

    public bool isGroundHit;

    public bool isPlayerHit;

    public bool isEnemyHit;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnTriggerEnter2D( Collider2D col ) //接触した
    {
        if( col.gameObject.name == "StageMap" )
        {
            isGroundHit = true;
            //Debug.Log("Ground");
        }
        if (col.gameObject.name == "Player")
        {
            isPlayerHit = true;
            //Debug.Log("Player");
        }
        if (col.gameObject.tag == "Enemy")
        {
            isEnemyHit = true;
            //Debug.Log("Enemy");
        }
    }


    void OnTriggerExit2D(Collider2D col)
    {
        //ステージマップから離れた
        if( col.gameObject.name == "StageMap" )
        {
            isGroundHit = false;
        }
        //プレイヤーから離れた
        if (col.gameObject.name == "Player")
        {
            isPlayerHit = false;
        }
        //敵から離れた
        if (col.gameObject.tag == "Enemy")
        {
            isEnemyHit = false;
        }
    }

}

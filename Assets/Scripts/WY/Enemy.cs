using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    ///<summary> 플레이어 데미지 처리를 위한 요소 </summary>
    BattleManager BM;
    ///<summary> 비활성화 시 풀에 삽입 </summary>
    Pool<Enemy> pool;

    ///<summary> 프리팹에서 설정, 강약 몬스터 여부 </summary>
    [SerializeField] bool isWeak;
    ///<summary> 몬스터 스프라이트, 이동 방향에 따라 좌우반전 </summary>
    [SerializeField] SpriteRenderer spriteRenderer;
    ///<summary> 넉백 처리 </summary>
    [SerializeField] Rigidbody2D myRigid;
    ///<summary> 이동 방향 </summary>
    Vector2 moveVector;

    ///<summary> 체력 </summary>
    [Header("Stat")]
    int health;
    ///<summary> 공격력 </summary>
    int atk;
    ///<summary> 이동 속도 </summary>
    float speed;

    float knockbackRate = 20f;
    public bool isKnockback = false;


    ///<summary> 위치 설정, 스텟 설정 </summary>
    public void Initialize(BattleManager bm, int stageLvl, Pool<Enemy> p, Vector3 pos)
    {
        BM = bm;
        pool = p;

        isKnockback = false;
        StatLoad(stageLvl);
        transform.position = pos;
        moveVector = -pos.normalized;
        spriteRenderer.flipX = moveVector.x < 0;
    }
    ///<summary> 스테이지 레벨에 따른 스텟 로드 </summary>
    void StatLoad(int stageLvl)
    {
        if(isWeak)
        {
            health = Random.Range(stageLvl, 2 * stageLvl + 1);
            atk = Random.Range(stageLvl, 3 * stageLvl + 1);
            speed = Random.Range(0.05f * stageLvl, 0.08f * stageLvl) + 0.15f;
        }
        else
        {
            health = (int)Random.Range(2 * stageLvl, Mathf.Pow(stageLvl, 2.5f) + 2 * stageLvl);
            atk = Random.Range(2 * stageLvl, 5 * stageLvl + 1);
            speed = Random.Range(0.03f * stageLvl, 0.05f * stageLvl) + 0.1f;
         }
    }

    ///<summary> 이동 시작 </summary>
    public void StartMove() => StartCoroutine(MoveCoroutine());
    ///<summary> 이동 코루틴 </summary>
    IEnumerator MoveCoroutine()
    {
        while(isActiveAndEnabled && !BM.isEnd)
        {
            if (!isKnockback)
            {
                moveVector = -transform.position.normalized;
                spriteRenderer.flipX = moveVector.x < 0;
                transform.Translate(moveVector * 0.03f * speed);
            }
            yield return new WaitForSeconds(0.03f);
        }

        yield return null;
    }

    ///<summary> 총알에 맞을 시 호출 </summary>
    public void GetDamage(int dmg)
    {
        health -= dmg;
        if(health <= 0)
        {
            pool.Push(this);
            gameObject.SetActive(false);
            BM.kill++;
        }
    }

    ///<summary> 가드에게 충돌 시 넉백 </summary>
    void GetKnockBack()
    {
                isKnockback = true;
        myRigid.AddForce(-moveVector * knockbackRate);
        if(isActiveAndEnabled) StartCoroutine(KnockbackEnd());
    }
    ///<summary> 넉백 속도 감소시키기 </summary>
    IEnumerator KnockbackEnd()
    {
        yield return new WaitForSeconds(0.2f);
        while(myRigid.velocity.magnitude > 0.1f) {myRigid.velocity *= 0.7f; yield return new WaitForSeconds(0.1f);}
        myRigid.velocity = new Vector2(0, 0);
        isKnockback = false;
    }

    ///<summary> 캐릭터와 충돌 처리 </summary>
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(!isKnockback)
        {
            if(other.tag == "Guard")
            {
                GetKnockBack();
                GetDamage(BM.reflect);
                BM.GetDamage_Guard(atk);
            }
            else if (other.tag == "Shoter")
            {
                GetKnockBack();
                BM.GetDamage_Shoter(atk);
            }
        }
    }
}

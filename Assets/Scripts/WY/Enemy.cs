using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Pool<Enemy> pool;

    [SerializeField] bool isWeak;
    ///<summary> 몬스터 스프라이트, 이동 방향에 따라 좌우반전 </summary>
    [SerializeField] SpriteRenderer spriteRenderer;
    ///<summary> 이동 방향 </summary>
    Vector2 moveVector;

    ///<summary> 체력 </summary>
    [Header("Stat")]
    int health;
    ///<summary> 공격력 </summary>
    int atk;
    ///<summary> 이동 속도 </summary>
    float speed;


    ///<summary> 위치 설정, 스텟 설정 </summary>
    public void Initialize(int stageLvl, Pool<Enemy> p, Vector3 pos)
    {
        pool = p;

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
        while(isActiveAndEnabled)
        {
            moveVector = -transform.position.normalized;
            transform.Translate(moveVector * 0.03f * speed);
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
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Pool<Bullet> pool;

    ///<summary> 총알 스피드, 불변 </summary>
    const float speed = 6f;
    int dmg = 3;

    ///<summary> 풀 연결, 나아갈 방향 설정 </summary>
    public void Initialize(Pool<Bullet> p, Vector2 direction)
    {
        pool = p;
        transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg, Vector3.forward);
    }

    ///<summary> 발사, 이동 시작 </summary>
    public void StartMove() => StartCoroutine(MoveCoroutine());
    
    ///<summary> 이동 시작 </summary>
    IEnumerator MoveCoroutine()
    {
        while(isActiveAndEnabled)
        {
            transform.Translate(Vector3.right * speed * 0.03f);
            yield return new WaitForSeconds(0.03f);
        }
    }

    ///<summary> 충돌 처리 </summary>
    private void OnTriggerEnter2D(Collider2D collider) 
    {
        if(collider.tag == "Enemy")
        {
            collider.GetComponent<Enemy>().GetDamage(dmg);
            pool.Push(this);
            gameObject.SetActive(false);
        }
        else if(collider.tag == "Bound")
        {
            pool.Push(this);
            gameObject.SetActive(false);
        }
    }
}

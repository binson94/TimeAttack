using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    float speed = 6f;

    public void SetDirection(Vector2 direction)
    {
        transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg, Vector3.forward);
    
    }

    public void StartMove()
    {
        StartCoroutine(MoveCoroutine());
    }

    IEnumerator MoveCoroutine()
    {
        while(isActiveAndEnabled)
        {
            transform.Translate(Vector3.right * speed * 0.03f);
            yield return new WaitForSeconds(0.03f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider) 
    {
        if(collider.tag == "Enemy")
        {
            BattleManager.instance.bulletPool.Push(this);
            gameObject.SetActive(false);
        }
        else if(collider.tag == "Bound")
        {
            BattleManager.instance.bulletPool.Push(this);
            gameObject.SetActive(false);
        }
    }
}

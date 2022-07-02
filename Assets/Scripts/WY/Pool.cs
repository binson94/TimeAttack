using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool<T> where T : MonoBehaviour
{
    Queue<T> poolQueue = new Queue<T>();

    public T GetToken(GameObject prefab, Transform parent)
    {
        if(poolQueue.Count > 0)
            return poolQueue.Dequeue();
        else
        {
            T b = GameObject.Instantiate(prefab).GetComponent<T>();
            b.transform.SetParent(parent);
            return b;
        }
    }

    public void Push(T obj) => poolQueue.Enqueue(obj);
}

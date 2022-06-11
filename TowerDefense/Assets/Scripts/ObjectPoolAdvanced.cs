using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolAdvanced : MonoBehaviour
{
    private Dictionary<string, Queue<GameObject>> objectPool = new Dictionary<string, Queue<GameObject>>();

    public GameObject GetObject(GameObject gameObject)
    {
        if (objectPool.TryGetValue(gameObject.name, out Queue <GameObject> objectList))
        {
            if(objectList.Count == 0)
            {
                return CreateNewObject(gameObject);
            } else
            {
                GameObject _object = objectList.Dequeue();
                _object.SetActive(true);
                return _object;
            }
        } else
        {
            return CreateNewObject(gameObject);
        }
    }

    public GameObject CreateNewObject(GameObject gameObject)
    {
        GameObject _newObject = Instantiate(gameObject);
        _newObject.name = gameObject.name;
        return _newObject;
    }

    public void ReturnGameObject(GameObject gameObject)
    {
        if(objectPool.TryGetValue(gameObject.name, out Queue<GameObject> objectList))
        {
            objectList.Enqueue(gameObject);
        }
        else
        {
            Queue<GameObject> newObjectQueue = new Queue<GameObject>();
            newObjectQueue.Enqueue(gameObject);
            objectPool.Add(gameObject.name, newObjectQueue);
        }

        gameObject.SetActive(false);
    }
}

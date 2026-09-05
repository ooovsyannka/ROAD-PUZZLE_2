using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : MonoBehaviour
{
    private Queue<T> _pool = new Queue<T>();
    private T _prefab;
    private List<T> _prefabs;

    public ObjectPool(T prefab)
    {
        _prefab = prefab;
    }

    public ObjectPool(List<T> prefabs)
    {
        _prefabs = prefabs;

        foreach (T prefas in prefabs)
        {
            T currentObject = Object.Instantiate(prefas);
            currentObject.gameObject.SetActive(false);
            _pool.Enqueue(currentObject);
        }
    }

    public T GetObject(Transform parent)
    {
        if (_pool.Count > 0)
        {
            return _pool.Dequeue();
        }

        return CrateObject(parent);
    }

    public T GetObjectFromList()
    {
        if (_pool.Count > 0)
        {
            return _pool.Dequeue();
        }

        return CrateObjectFromList();
    }

    public void PutObject(T obj)
    {
        _pool.Enqueue(obj);
        obj.gameObject.SetActive(false);
    }

    private T CrateObject(Transform parent)
    {
        T currentObject = Object.Instantiate(_prefab);
        currentObject.transform.parent = parent;
        currentObject.gameObject.SetActive(false);

        return currentObject;
    }

    private T CrateObjectFromList()
    {
        T currentObject = Object.Instantiate(_prefabs[Random.Range(0,_prefabs.Count)]);
        currentObject.gameObject.SetActive(false);

        return currentObject;
    }
}
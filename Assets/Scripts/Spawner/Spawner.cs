using System.Collections.Generic;
using UnityEngine;

public class Spawner<T> where T : MonoBehaviour
{
    private ObjectPool<T> _pool;
    private List<T> _activeObjects = new List<T>();

    public Spawner(T prefab)
    {
        _pool = new ObjectPool<T>(prefab);
    }

    public Spawner(List<T> prefabs)
    {
        _pool = new ObjectPool<T>(prefabs);
    }

    public T Spawn(Vector3 spawnPosition, Transform parent)
    {
        T currentObject = _pool.GetObject(parent);
        _activeObjects.Add(currentObject);
        currentObject.transform.position = spawnPosition;
        currentObject.gameObject.SetActive(true);

        return currentObject;
    }

    public T SpawnObjectFromList(Vector3 spawnPosition)
    {
        T currentObject = _pool.GetObjectFromList();
        _activeObjects.Add(currentObject);
        currentObject.transform.position = spawnPosition;
        currentObject.gameObject.SetActive(true);

        return currentObject;
    }

    public void ReturnObjectInPool(T returnedObject)
    {
        _activeObjects.Remove(returnedObject);
        _pool.PutObject(returnedObject);
    }

    public void CleanActiveObject()
    {
        if (_activeObjects.Count > 0)
        {
            for (int i = _activeObjects.Count - 1; i >= 0; i--)
            {
                T currentObject = _activeObjects[i];

                currentObject.gameObject.SetActive(false);
                ReturnObjectInPool(currentObject);
            }
        }
    }
}
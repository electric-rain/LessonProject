using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : Component
{
    private readonly T _prefab;
    private readonly Transform _parent;
    private readonly Queue<T> _avalible = new Queue<T>();

    public ObjectPool(T prefab, Transform parent, int prewarmCount = 0)
    {
        _prefab = prefab;
        _parent = parent;

        for (int i = 0; i < prewarmCount; i++)
        {
            T obj = Object.Instantiate(_prefab, _parent);
            obj.gameObject.SetActive(false);
            _avalible.Enqueue(obj);
        }
    }

    public T Get()
    {
        T obj;

        if (_avalible.Count > 0)
            obj = _avalible.Dequeue();
        else
            obj = Object.Instantiate(_prefab, _parent);

        obj.gameObject.SetActive(true);
        return obj;

    }

    public void Return(T obj)
    {
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(_parent);
        _avalible.Enqueue(obj);
    }
}
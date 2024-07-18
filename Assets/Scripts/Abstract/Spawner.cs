using UnityEngine;
using UnityEngine.Pool;

public abstract class Spawner<T> : MonoBehaviour where T : Item
{
    [SerializeField] protected int _defaultCapacity;
    [SerializeField] protected int _maxSize;
    [SerializeField] protected T _prefab;

    public float AmountActiveObjects => _pool.CountActive;
    public float TotalCreatedObjects => _totalCreatedObjects;

    protected ObjectPool<T> _pool;

    protected Vector3 _spawnPosition;
    protected float _totalCreatedObjects;

    protected void Awake()
    {
        _pool = new ObjectPool<T>(
            createFunc: CreateItem,
            actionOnGet: OnGetItem,
            actionOnRelease: OnReleaseItem,
            actionOnDestroy: OnDestroyItem,
            collectionCheck: true,
            defaultCapacity: _defaultCapacity,
            maxSize: _maxSize);
    }

    protected virtual T CreateItem()
    {
        T item = Instantiate(_prefab);
        SubscribeOnEvents(item);
        return item;
    }

    protected virtual void OnGetItem(T item)
    {
        item.transform.position = _spawnPosition;
        item.gameObject.SetActive(true);
    }

    protected virtual void OnReleaseItem(T item)
    {
        item.gameObject.SetActive(false);
    }

    protected virtual void OnDestroyItem(T item)
    {
        UnSubscribeOnEvents(item);
        Destroy(item.gameObject);
    }

    protected void ReturnToPool(T item)
    {
        _pool.Release(item);
    }

    protected abstract Vector3 GetSpawnPosition();
    protected abstract void SubscribeOnEvents(T item);
    protected abstract void UnSubscribeOnEvents(T item);
}
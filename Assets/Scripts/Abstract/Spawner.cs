using System;
using UnityEngine;
using UnityEngine.Pool;

public abstract class Spawner<T> : MonoBehaviour where T : Item
{
    [SerializeField] private int _defaultCapacity;
    [SerializeField] private int _maxSize;
    [SerializeField] private T _prefab;

    protected ObjectPool<T> _pool;
    private float _amountCreatedItems;
    private float _amountSpawnedItems;

    public event Action CountCreatedItemsChanged;
    public event Action CountSpawnedItemsChanged;
    public event Action CountActiveItemsChanged;

    public float AmountCreatedItems => _amountCreatedItems;
    public float AmountSpawnedItems => _amountSpawnedItems;
    public float AmountActiveItems => _pool.CountActive;

    protected virtual void OnGetItem(T item)
    {
        item.transform.position = GetSpawnPosition();
        item.gameObject.SetActive(true);
        CountActiveItemsChanged?.Invoke();
    }

    protected virtual void OnReleaseItem(T item)
    {
        item.gameObject.SetActive(false);
        CountActiveItemsChanged?.Invoke();
    }

    protected void Spawn(Vector3 spawnPosition)
    {
        SetSpawnPosition(spawnPosition);
        IncreaseSpawnedItems();
        _pool.Get();
    }

    protected void OnDestroyItem(T item)
    {
        UnSubscribeOnEvents(item);
        Destroy(item.gameObject);
    }

    protected void ReturnToPool(T item)
    {
        _pool.Release(item);
    }

    protected void IncreaseSpawnedItems()
    {
        _amountSpawnedItems++;
        CountSpawnedItemsChanged?.Invoke();
    }

    private void Awake()
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

    private T CreateItem()
    {
        T item = Instantiate(_prefab);
        SubscribeOnEvents(item);
        _amountCreatedItems++;
        CountCreatedItemsChanged?.Invoke();
        return item;
    }

    protected abstract Vector3 GetSpawnPosition();
    protected abstract void SetSpawnPosition(Vector3 position);
    protected abstract void SubscribeOnEvents(T item);
    protected abstract void UnSubscribeOnEvents(T item);
}

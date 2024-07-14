using UnityEngine;
using UnityEngine.Pool;

public class BombSpawner : MonoBehaviour
{
    [SerializeField] private CubeSpawner _cubeSpawner;
    [SerializeField] private Bomb _bombPrefab;
    [SerializeField] private int _defaultCapacity;
    [SerializeField] private int _maxSize;

    private ObjectPool<Bomb> _bombPool;

    private Vector3 _spawnPosition;

    private void Awake()
    {
        _bombPool = new ObjectPool<Bomb>(
            createFunc: CreateBomb,
            actionOnGet: OnGetBomb,
            actionOnRelease: OnReleaseBomb,
            actionOnDestroy: OnDestroyBomb,
            collectionCheck: true,
            defaultCapacity: _defaultCapacity,
            maxSize: _maxSize);
    }

    private void OnEnable()
    {
        _cubeSpawner.CubeDisappeared += Spawn;
    }

    private void OnDisable()
    {
        _cubeSpawner.CubeDisappeared -= Spawn;
    }

    private Bomb CreateBomb()
    {

        Bomb bomb = Instantiate(_bombPrefab);
        return bomb;
    }

    private void OnGetBomb(Bomb bomb)
    {
        bomb.transform.position = _spawnPosition;
        bomb.gameObject.SetActive(true);
    }

    private void OnReleaseBomb(Bomb bomb)
    {
        bomb.gameObject.SetActive(false);
    }

    private void OnDestroyBomb(Bomb bomb)
    {
        Destroy(bomb.gameObject);
    }

    private void Spawn(Vector3 spawnPosition)
    {
        SelectSpawnPosition(spawnPosition);
        _bombPool.Get();
    }

    private void SelectSpawnPosition(Vector3 spawnPosition)
    {
        _spawnPosition = spawnPosition;
    }
}
using UnityEngine;

public class BombSpawner : BaseSpawner<Bomb>
{
    [SerializeField] private CubeSpawner _cubeSpawner;

    private void OnEnable()
    {
        _cubeSpawner.CubeDisappeared += Spawn;
    }

    private void OnDisable()
    {
        _cubeSpawner.CubeDisappeared -= Spawn;
    }

    private void Spawn(Vector3 spawnPosition)
    {
        _spawnPosition = spawnPosition;
        _pool.Get();
    }

    protected override Vector3 GetSpawnPosition()
    {
        return _spawnPosition;
    }

    protected override void SubscribeOnEvents(Bomb bomb)
    {
        bomb.Exploded += ReturnToPool;
    }

    protected override void UnSubscribeOnEvents(Bomb bomb)
    {
        bomb.Exploded -= ReturnToPool;
    }
}
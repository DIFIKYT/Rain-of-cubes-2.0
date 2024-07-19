using UnityEngine;

public class BombSpawner : Spawner<Bomb>
{
    [SerializeField] private CubeSpawner _cubeSpawner;

    private Vector3 _spawnPosition;

    protected override Vector3 GetSpawnPosition()
    {
        return _spawnPosition;
    }

    protected override void SetSpawnPosition(Vector3 position)
    {
        _spawnPosition = position;
    }

    protected override void SubscribeOnEvents(Bomb bomb)
    {
        bomb.Exploded += ReturnToPool;
    }

    protected override void UnSubscribeOnEvents(Bomb bomb)
    {
        bomb.Exploded -= ReturnToPool;
    }

    private void OnEnable()
    {
        _cubeSpawner.CubeDisappeared += Spawn;
    }

    private void OnDisable()
    {
        _cubeSpawner.CubeDisappeared -= Spawn;
    }
}

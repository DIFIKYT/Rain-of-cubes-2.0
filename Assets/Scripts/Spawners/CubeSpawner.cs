using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSpawner : Spawner<Cube>
{
    [SerializeField] private float _yCoordinateSpawn;
    [SerializeField] private float _xMinCoordinateSpawn;
    [SerializeField] private float _xMaxCoordinateSpawn;
    [SerializeField] private float _zMinCoordinateSpawn;
    [SerializeField] private float _zMaxCoordinateSpawn;
    [SerializeField] private List<Color> _colors;

    readonly float _delay = 0.5f;
    private Vector3 _spawnPosition;

    public event Action<Vector3> CubeDisappeared;

    protected override void OnGetItem(Cube cube)
    {
        base.OnGetItem(cube);
        cube.transform.position = GetSpawnPosition();
        cube.gameObject.SetActive(true);
    }

    protected override void OnReleaseItem(Cube cube)
    {
        base.OnReleaseItem(cube);
        CubeDisappeared?.Invoke(cube.transform.position);
    }

    protected override Vector3 GetSpawnPosition()
    {
        return _spawnPosition;
    }

    protected override void SetSpawnPosition(Vector3 position)
    {
        _spawnPosition = position;
    }

    protected override void SubscribeOnEvents(Cube cube)
    {
        cube.LifeTimeOut += ReturnToPool;
        cube.LifeTimeOut += ChangeColorToDefault;
        cube.ContactWithPlatform += ChangeColor;
    }

    protected override void UnSubscribeOnEvents(Cube cube)
    {
        cube.LifeTimeOut -= ReturnToPool;
        cube.LifeTimeOut -= ChangeColorToDefault;
        cube.ContactWithPlatform -= ChangeColor;
    }

    private void Start()
    {
        StartCoroutine(SpawnCoroutine());
    }

    private Vector3 GetRandomSpawnPosition()
    {
        float xSpawnCoordinate = UnityEngine.Random.Range(_xMinCoordinateSpawn, _xMaxCoordinateSpawn);
        float zSpawnCoordinate = UnityEngine.Random.Range(_zMinCoordinateSpawn, _zMaxCoordinateSpawn);
        return new Vector3(xSpawnCoordinate, _yCoordinateSpawn, zSpawnCoordinate);
    }

    private IEnumerator SpawnCoroutine()
    {
        while (true)
        {
            Spawn(GetRandomSpawnPosition());
            yield return new WaitForSeconds(_delay);
        }
    }

    private void ChangeColor(Cube cube)
    {
        if (cube.TryGetComponent<Renderer>(out Renderer renderer))
            renderer.material.color = _colors[UnityEngine.Random.Range(0, _colors.Count)];
    }

    private void ChangeColorToDefault(Cube cube)
    {
        if(cube.TryGetComponent(out Renderer renderer))
            renderer.material.color = Color.white;
    }
}
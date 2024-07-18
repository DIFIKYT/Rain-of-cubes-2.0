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

    public event Action<Vector3> CubeDisappeared;

    private float _delay = 0.5f;

    private void Start()
    {
        StartCoroutine(Spawn());
    }

    protected override void OnGetItem(Cube cube)
    {
        cube.transform.position = GetSpawnPosition();
        cube.gameObject.SetActive(true);
    }

    protected override void OnReleaseItem(Cube cube)
    {
        cube.gameObject.SetActive(false);
        CubeDisappeared?.Invoke(cube.gameObject.transform.position);
    }

    private IEnumerator Spawn()
    {
        while (true)
        {
            _pool.Get();
            _totalCreatedObjects++;
            yield return new WaitForSeconds(_delay);
        }
    }

    private void ChangeColor(Cube cube)
    {
        if (cube.TryGetComponent<Renderer>(out Renderer renderer))
            renderer.material.color = _colors[UnityEngine.Random.Range(0, _colors.Count)];
    }

    protected override Vector3 GetSpawnPosition()
    {
        float xSpawnCoordinate = UnityEngine.Random.Range(_xMinCoordinateSpawn, _xMaxCoordinateSpawn);
        float zSpawnCoordinate = UnityEngine.Random.Range(_zMinCoordinateSpawn, _zMaxCoordinateSpawn);
        return new Vector3(xSpawnCoordinate, _yCoordinateSpawn, zSpawnCoordinate);
    }

    protected override void SubscribeOnEvents(Cube cube)
    {
        cube.LifeTimeOut += ReturnToPool;
        cube.ContactWithPlatform += ChangeColor;
    }

    protected override void UnSubscribeOnEvents(Cube cube)
    {
        cube.LifeTimeOut -= ReturnToPool;
        cube.ContactWithPlatform -= ChangeColor;
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class CubeSpawner : MonoBehaviour
{
    [SerializeField] private float _yCoordinateSpawn;
    [SerializeField] private float _xMinCoordinateSpawn;
    [SerializeField] private float _xMaxCoordinateSpawn;
    [SerializeField] private float _zMinCoordinateSpawn;
    [SerializeField] private float _zMaxCoordinateSpawn;
    [SerializeField] private int _defaultCapacity;
    [SerializeField] private int _maxSize;
    [SerializeField] private Cube _cubePrefab;
    [SerializeField] private List<Color> _colors;

    public event Action<Vector3> CubeDisappeared;

    private ObjectPool<Cube> _cubePool;
    private float _delay = 0.5f;

    private void Awake()
    {
        _cubePool = new ObjectPool<Cube>(
            createFunc: CreateCube,
            actionOnGet: OnGetCube,
            actionOnRelease: OnReleaseCube,
            actionOnDestroy: OnDestroyCube,
            collectionCheck: true,
            defaultCapacity: _defaultCapacity,
            maxSize: _maxSize);
    }

    private void Start()
    {
        StartCoroutine(Spawn());
    }

    private Cube CreateCube()
    {
        Cube cube = Instantiate(_cubePrefab);
        SubscribeOnEvents(cube);
        return cube;
    }

    private void OnGetCube(Cube cube)
    {
        cube.transform.position = SetSpawnCoordinate();
        cube.gameObject.SetActive(true);
    }

    private void OnReleaseCube(Cube cube)
    {
        cube.gameObject.SetActive(false);
        CubeDisappeared?.Invoke(cube.gameObject.transform.position);
    }

    private void OnDestroyCube(Cube cube)
    {
        Destroy(cube.gameObject);
    }

    private IEnumerator Spawn()
    {
        while (true)
        {
            _cubePool.Get();
            yield return new WaitForSeconds(_delay);
        }
    }

    private void SubscribeOnEvents(Cube cube)
    {
        cube.LifeTimeOut += ReturnToPool;
        cube.ContactWithPlatform += ChangeColor;
    }

    private Vector3 SetSpawnCoordinate()
    {
        float xSpawnCoordinate = UnityEngine.Random.Range(_xMinCoordinateSpawn, _xMaxCoordinateSpawn);
        float zSpawnCoordinate = UnityEngine.Random.Range(_zMinCoordinateSpawn, _zMaxCoordinateSpawn);
        return new Vector3(xSpawnCoordinate, _yCoordinateSpawn, zSpawnCoordinate);
    }

    private void ChangeColor(Cube cube)
    {
        if (cube.TryGetComponent<Renderer>(out Renderer renderer))
            renderer.material.color = _colors[UnityEngine.Random.Range(0, _colors.Count)];
    }

    private void ReturnToPool(Cube cube)
    {
        _cubePool.Release(cube);
    }
}
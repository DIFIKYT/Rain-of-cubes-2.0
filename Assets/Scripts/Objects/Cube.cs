using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class Cube : Item
{
    public event Action<Cube> LifeTimeOut;
    public event Action<Cube> ContactWithPlatform;

    private bool _isContactWithPlatform = false;

    private void OnTriggerEnter(Collider collider)
    {
        if (_isContactWithPlatform == false)
        {
            if (collider.GetComponent<Platform>())
            {
                StartCoroutine(LifeTimeCoroutine());
                ContactWithPlatform?.Invoke(this);
                _isContactWithPlatform = true;
            }
        }
    }

    private IEnumerator LifeTimeCoroutine()
    {
        yield return new WaitForSeconds(GetRandomLifeTime());

        LifeTimeOut?.Invoke(this);
        _isContactWithPlatform = false;
    }

    private int GetRandomLifeTime()
    {
        int _minLifeTime = 2;
        int _maxLifeTime = 5;

        return UnityEngine.Random.Range(_minLifeTime, _maxLifeTime);
    }
}
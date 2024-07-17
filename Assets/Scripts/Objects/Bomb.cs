using System;
using System.Collections;
using UnityEngine;

public class Bomb : Item
{
    [SerializeField] private float _fadeDuration;
    [SerializeField] private Renderer _renderer;
    [SerializeField] private Explosion _explosion;

    public event Action<Bomb> Exploded;

    private void OnEnable()
    {
        StartCoroutine(IncreasingTransparency());
    }

    private void OnDisable()
    {
        _renderer.material.color = Color.black;
    }

    private IEnumerator IncreasingTransparency()
    {
        Material material = _renderer.material;
        Color originalColor = material.color;
        float elapsedTime = 0f;

        while (elapsedTime < _fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(1f, 0f, elapsedTime / _fadeDuration);
            material.color = new Color(originalColor.r, originalColor.g, originalColor.b, newAlpha);
            yield return null;
        }

        _explosion.Explode(this.gameObject.transform.position);

        Exploded?.Invoke(this);
    }
}
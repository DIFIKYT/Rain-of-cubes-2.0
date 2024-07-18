using TMPro;
using UnityEngine;

public abstract class SpawnerUI<T> : MonoBehaviour where T : Item
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Spawner<T> _spawner;

    protected virtual void Update()
    {
        _text.text = ($"{typeof(T).Name}s Created: {_spawner.TotalCreatedObjects} Active: {_spawner.AmountActiveObjects}");
    }
}
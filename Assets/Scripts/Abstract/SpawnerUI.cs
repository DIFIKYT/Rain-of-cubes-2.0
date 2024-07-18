using TMPro;
using UnityEngine;

public class SpawnerUI<T> : MonoBehaviour where T : Item
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Spawner<T> _spawner;

    private void Update()
    {
        _text.text = ($"{typeof(T).Name}s Created: {_spawner.TotalCreatedObjects} Active: {_spawner.AmountActiveObjects}");
    }
}
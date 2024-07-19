using TMPro;
using UnityEngine;

public abstract class SpawnerUI<T> : MonoBehaviour where T : Item
{
    [SerializeField] private TextMeshProUGUI _createItemsText;
    [SerializeField] private TextMeshProUGUI _spawnItemsText;
    [SerializeField] private TextMeshProUGUI _activeItemsText;
    [SerializeField] private Spawner<T> _spawner;

    private void ViewCreateItems()
    {
        _createItemsText.text = ($"Created {typeof(T)}s: {_spawner.AmountCreatedItems}");
    }

    private void ViewSpawnItems()
    {
        _spawnItemsText.text = ($"Spawned {typeof(T)}s: {_spawner.AmountSpawnedItems}");
    }

    private void ViewActiveItems()
    {
        _activeItemsText.text = ($"Active {typeof(T)}s: {_spawner.AmountActiveItems}");
    }

    private void OnEnable()
    {
        _spawner.CountCreatedItemsChanged += ViewCreateItems;
        _spawner.CountSpawnedItemsChanged += ViewSpawnItems;
        _spawner.CountActiveItemsChanged += ViewActiveItems;
    }

    private void OnDisable()
    {
        _spawner.CountCreatedItemsChanged -= ViewCreateItems;
        _spawner.CountSpawnedItemsChanged -= ViewSpawnItems;
        _spawner.CountActiveItemsChanged -= ViewActiveItems;
    }
}

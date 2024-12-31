using UnityEngine;

public class SheepSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _sheepPrefab;
    [SerializeField] private int _sheepCount = 10;
    [SerializeField] private Transform _spawnTransformMin;
    [SerializeField] private Transform _spawnTransformMax;

    void Start()
    {
        LevelController.Instance.SheepAbducted.AddListener(SpawnSheep);
        LevelController.Instance.GameReset.AddListener(() => {
            ClearSheep();
            SpawnSheep(_sheepCount);
        });
        
    }

    private void ClearSheep()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void SpawnSheep(int count)
    {
        for (int i = 0; i < count; i++)
        {
            SpawnSheep();
        }
    }

    private void SpawnSheep()
    {
        Debug.Log($"Spawning sheep at {transform.position}");
        float x = Random.Range(_spawnTransformMin.position.x, _spawnTransformMax.position.x);
        float z = Random.Range(_spawnTransformMin.position.z, _spawnTransformMax.position.z);
        Instantiate(_sheepPrefab, new Vector3(x, _spawnTransformMin.position.y, z), Quaternion.identity, transform);
    }
}

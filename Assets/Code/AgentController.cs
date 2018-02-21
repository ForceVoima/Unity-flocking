using UnityEngine;

public class AgentController : MonoBehaviour
{
    public GameObject _prefab;
    public int _prefabsToSpawn = 20;
    public float _spawnRadius = 20f;
    public float _agentVelocity = 5f;
    public float _agentDistance = 3f;
    public float _randomRotation = 5f;
    public LayerMask _agentMask;

    public float Velocity { get { return _agentVelocity; } }

    private void Start()
    {
        if (_prefab == null)
        {
            Debug.LogError("Define prefab in " + gameObject);
            return;
        }

        for (int i = 0; i < _prefabsToSpawn; i++)
        {
            SpawnPrefab(i + 1);
        }
    }

    private void Update()
    {
        transform.position += transform.forward * Velocity * Time.deltaTime;
    }

    private void SpawnPrefab(int number)
    {
        float x = Random.Range(-_randomRotation, _randomRotation);
        float y = Random.Range(-_randomRotation, _randomRotation);
        float z = Random.Range(-_randomRotation, _randomRotation);

        Quaternion rotation = Quaternion.Euler(x, y, z);
        Vector3 position = UnityEngine.Random.insideUnitSphere * _spawnRadius;

        GameObject go = Instantiate(_prefab, position, rotation, transform);
        go.name = "Agent " + number;

        AgentBehaviour behaviour = go.GetComponent<AgentBehaviour>();

        behaviour.AddController(this);
    }
}

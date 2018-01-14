using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public List<Spawner> Spawners = new List<Spawner>();
    public int SpwnNB = 3;
    public float SpaceBetweenToSpawn = 3.0f;

    [SerializeField] private Transform SpwnParent;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start ()
    {
        SpawnSpawner();
    }
	
	void Update ()
    {
		
	}

    void SpawnSpawner()
    {
        SpwnParent = transform.Find("Spawns");
        float space = (SpaceBetweenToSpawn * (SpwnNB - 1)) / 2.0f;
        float x = -space;
        for (int i = 0; i < SpwnNB; i++)
        {
            GameObject spawn = new GameObject("Spawn_" + i );
            Spawners.Add( spawn.AddComponent<Spawner>() );
            spawn.transform.parent = SpwnParent.transform;
            spawn.transform.position = new Vector3( x, SpwnParent.position.y, SpwnParent.position.z);
            spawn.transform.localRotation = Quaternion.identity;

            x += SpaceBetweenToSpawn;
        }
    }

    private void OnDrawGizmos()
    {
        DrawSpawner();
    }

    private void DrawSpawner()
    {
        foreach (Spawner spawn in Spawners)
        {
            // origin
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(spawn.transform.position, 0.1f);
            Gizmos.DrawLine(spawn.transform.position, spawn.transform.position + spawn.transform.forward * 50);

            //destination
            Gizmos.DrawWireSphere(spawn.transform.position + spawn.transform.forward * 50, 0.1f);
        }
    }
}

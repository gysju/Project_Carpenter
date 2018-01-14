using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public List<Spawner> Spawners = new List<Spawner>();
    public List<Transform> PlayerAnchors = new List<Transform>();
    public int SpwnNB = 3;
    public float SpaceBetweenToSpawn = 3.0f;

    [SerializeField] private Transform SpwnParent;
    [SerializeField] private Transform PlayerAnchorsParent;

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

    [ContextMenu("Init game")]
    void Start ()
    {
        if (Spawners != null || PlayerAnchors != null)
            Clean();

        SpawnSpawner();
        SpawnPlayerAnchor();

        if(Player.Instance != null)
            Player.Instance.InitPlayer((SpwnNB - 1) / 2);
    }

    void Clean()
    {
        foreach (Spawner spawn in Spawners)
        {
            Destroy(spawn.gameObject);
        }

        foreach (Transform trans in PlayerAnchors)
        {
            Destroy(trans.gameObject);
        }

        Spawners.Clear();
        PlayerAnchors.Clear();
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

    void SpawnPlayerAnchor()
    {
        PlayerAnchorsParent = transform.Find("PlayerAnchors");
        float space = (SpaceBetweenToSpawn * (SpwnNB - 1)) / 2.0f;
        float x = -space;

        for (int i = 0; i < SpwnNB; i++)
        {
            GameObject anchor = new GameObject("Anchor_" + i);
            PlayerAnchors.Add(anchor.transform);
            anchor.transform.parent = PlayerAnchorsParent.transform;
            anchor.transform.position = new Vector3(x, PlayerAnchorsParent.position.y, PlayerAnchorsParent.position.z);
            anchor.transform.localRotation = Quaternion.identity;

            x += SpaceBetweenToSpawn;
        }
    }

    private void OnDrawGizmos()
    {
        DrawSpawners();
        DrawPlayerAnchors();
    }

    private void DrawSpawners()
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

    private void DrawPlayerAnchors()
    {
        foreach (Transform anchor in PlayerAnchors)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(anchor.position, 0.1f);
        }
    }
}

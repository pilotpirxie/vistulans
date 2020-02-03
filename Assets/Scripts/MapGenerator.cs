using System;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    /// <summary>
    /// Prefabs of objects to spawn
    /// </summary>
    public List<GameObject> Trees = new List<GameObject>();
    public List<GameObject> Rocks = new List<GameObject>();
    public List<GameObject> Bushes = new List<GameObject>();
    public List<GameObject> Mushrooms = new List<GameObject>();

    /// <summary>
    /// Maximum number of objects to spawn
    /// </summary>
    public int MaximumTreesCount = 1000;
    public int MaximumRocks = 100;
    public int MaximumBushes = 100;
    public int MaximumMushrooms = 100;

    /// <summary>
    /// Spawn area on the X-axis 
    /// </summary>
    public Vector2 HorizontalSpawnArea = new Vector2(-15f, 15f);

    /// <summary>
    /// Spawn area on the Z-axis
    /// </summary>
    public Vector2 VerticalSpawnArea = new Vector2(-15f, 15f);

    /// <summary>
    /// References to all vertices in the game
    /// </summary>
    private GameObject[] _vertices;

    /// <summary>
    /// References to all edges in the game
    /// </summary>
    private GameObject[] _edges;

    private void Start()
    {
        Invoke("DelayedStart", 0.5f);
    }

    /// <summary>
    /// Start little after initialization of entire game
    /// </summary>
    void DelayedStart()
    {
        _vertices = GameObject.FindGameObjectsWithTag("Vertex");
        _edges = GameObject.FindGameObjectsWithTag("Edge");

        for (int i = 0; i < MaximumTreesCount / 100; i++)
        {
            Invoke("SpawnTrees", i * 0.025f);
        }

        for (int i = 0; i < MaximumBushes / 100; i++)
        {
            Invoke("SpawnBushes", i * 0.025f);
        }

        for (int i = 0; i < MaximumRocks / 100; i++)
        {
            Invoke("SpawnRocks", i * 0.025f);
        }

        for (int i = 0; i < MaximumMushrooms / 100; i++)
        {
            Invoke("SpawnMushrooms", i * 0.025f);
        }
    }

    /// <summary>
    /// Spawn trees on the map
    /// </summary>
    void SpawnTrees()
    {
        for (int i = 0; i < 100; i++)
        {
            Vector3 spawnPosition = new Vector3(UnityEngine.Random.Range(HorizontalSpawnArea.x, HorizontalSpawnArea.y), -0.1f, UnityEngine.Random.Range(VerticalSpawnArea.x, VerticalSpawnArea.y));
            if (CanSpawn(spawnPosition))
            {
                int randomValue = (int)Math.Ceiling((double)UnityEngine.Random.Range(0.01f, 2));
                Instantiate(Trees[randomValue - 1], spawnPosition, Quaternion.identity);
            }
        }
    }

    /// <summary>
    /// Spawn rocks on the map
    /// </summary>
    void SpawnRocks()
    {
        for (int i = 0; i < 100; i++)
        {
            Vector3 spawnPosition = new Vector3(UnityEngine.Random.Range(HorizontalSpawnArea.x, HorizontalSpawnArea.y), -0.1f, UnityEngine.Random.Range(VerticalSpawnArea.x, VerticalSpawnArea.y));
            if (CanSpawn(spawnPosition))
            {
                int randomValue = (int)Math.Ceiling((double)UnityEngine.Random.Range(0.01f, 3));
                Instantiate(Rocks[randomValue - 1], spawnPosition, Quaternion.identity);
            }
        }
    }

    /// <summary>
    /// Spawn mushrooms on the map
    /// </summary>
    void SpawnMushrooms()
    {
        for (int i = 0; i < 100; i++)
        {
            Vector3 spawnPosition = new Vector3(UnityEngine.Random.Range(HorizontalSpawnArea.x, HorizontalSpawnArea.y), -0.1f, UnityEngine.Random.Range(VerticalSpawnArea.x, VerticalSpawnArea.y));
            if (CanSpawn(spawnPosition))
            {
                int randomValue = (int)Math.Ceiling((double)UnityEngine.Random.Range(0.01f, 2));
                Instantiate(Mushrooms[randomValue - 1], spawnPosition, Quaternion.identity);
            }
        }
    }

    /// <summary>
    /// Spawn bushes on the map
    /// </summary>
    void SpawnBushes()
    {
        for (int i = 0; i < 100; i++)
        {
            Vector3 spawnPosition = new Vector3(UnityEngine.Random.Range(HorizontalSpawnArea.x, HorizontalSpawnArea.y), -0.1f, UnityEngine.Random.Range(VerticalSpawnArea.x, VerticalSpawnArea.y));
            if (CanSpawn(spawnPosition))
            {
                int randomValue = (int)Math.Ceiling((double)UnityEngine.Random.Range(0.01f, 2));
                Instantiate(Bushes[randomValue - 1], spawnPosition, Quaternion.identity);
            }
        }
    }

    /// <summary>
    /// Check if spawn at specified position is possible
    /// </summary>
    /// <param name="spawnPosition"></param>
    /// <returns></returns>
    bool CanSpawn(Vector3 spawnPosition)
    {
        bool spawnPossible = true;
        foreach (GameObject vertex in _vertices)
        {
            if (GetDistanceFromVertex(spawnPosition, vertex) < 2f)
            {
                spawnPossible = false;
            }
        }

        foreach (GameObject edge in _edges)
        {
            EdgeController edgeController = edge.GetComponent<EdgeController>();

            Vector2 point = new Vector2(spawnPosition.x, spawnPosition.z);
            Vector2 start = new Vector2(edgeController.StartPosition.x, edgeController.StartPosition.z);
            Vector2 end = new Vector2(edgeController.EndPosition.x, edgeController.EndPosition.z);

            if (GetDistanceFromEdge(point, start, end) < 1f)
            {
                spawnPossible = false;
            }
        }

        return spawnPossible;
    }

    /// <summary>
    /// Calculate distance between selected position and position of vertex
    /// </summary>
    /// <param name="spawnPosition"></param>
    /// <param name="vertex"></param>
    /// <returns></returns>
    float GetDistanceFromVertex(Vector3 spawnPosition, GameObject vertex)
    {
        return Vector3.Distance(spawnPosition, vertex.transform.position);
    }

    /// <summary>
    /// Calculate distance of point from a line
    /// https://brilliant.org/wiki/dot-product-distance-between-point-and-a-line/
    /// https://stackoverflow.com/questions/849211/shortest-distance-between-a-point-and-a-line-segment
    /// </summary>
    /// <param name="point">Point</param>
    /// <param name="start">Start of the line</param>
    /// <param name="end">End of the line</param>
    /// <returns></returns>
    float GetDistanceFromEdge(Vector2 point, Vector2 start, Vector2 end)
    {
        float A = point.x - start.x;
        float B = point.y - start.y;
        float C = end.x - start.x;
        float D = end.y - start.y;

        float dotProduct = A * C + B * D;
        float lengthSquare = C * C + D * D;

        float param = -1;

        if (lengthSquare != 0)
        {
            param = dotProduct / lengthSquare;
        }

        float xx, yy;

        if (param < 0)
        {
            xx = start.x;
            yy = start.y;
        }
        else if (param > 1)
        {
            xx = end.x;
            yy = end.y;
        }
        else
        {
            xx = start.x + param * C;
            yy = start.y + param * D;
        }

        float dx = point.x - xx;
        float dy = point.y - yy;

        return Mathf.Sqrt(dx * dx + dy * dy);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Represent graph vertices and connections
/// </summary>
/// <typeparam name="T"></typeparam>
public class Graph<T>
{
    /// <summary>
    /// Instantiate new graph
    /// </summary>
    /// <param name="vertices">Vertices</param>
    /// <param name="edges">Edges</param>
    public Graph(IEnumerable<T> vertices, IEnumerable<Tuple<T, T>> edges)
    {
        // Add every vertex to adjacency list
        foreach (var vertex in vertices)
        {
            AddVertex(vertex);
        }

        // Add every vertex to adjacency list
        foreach (var edge in edges)
        {
            AddEdge(edge);
        }
    }

    // Adjacency list, represents vertices and connections between them
    public Dictionary<T, HashSet<T>> AdjacencyList { get; } = new Dictionary<T, HashSet<T>>();

    /// <summary>
    /// Add vertex to adjacency list
    /// </summary>
    /// <param name="vertex"></param>
    private void AddVertex(T vertex)
    {
        AdjacencyList[vertex] = new HashSet<T>();
    }

    /// <summary>
    /// Add edge to adjacency list
    /// </summary>
    /// <param name="edge"></param>
    private void AddEdge(Tuple<T, T> edge)
    {
        if (AdjacencyList.ContainsKey(edge.Item1) && AdjacencyList.ContainsKey(edge.Item2))
        {
            AdjacencyList[edge.Item1].Add(edge.Item2);
            AdjacencyList[edge.Item2].Add(edge.Item1);
        }
    }
}

public class AIController : MonoBehaviour
{
    /// <summary>
    /// Reference to graph controller
    /// </summary>
    [SerializeField]
    GraphController _graphController;

    /// <summary>
    /// Reference go gameplay controller
    /// </summary>
    [SerializeField]
    GameplayController _gameplayController;

    /// <summary>
    /// List of vertices from config file
    /// </summary>
    List<int> _vertices = new List<int>();

    /// <summary>
    /// List of edges as tuple (from, to)
    /// </summary>
    List<Tuple<int, int>> _edges = new List<Tuple<int, int>>();

    /// <summary>
    /// Graph instance object
    /// </summary>
    Graph<int> _graph;

    void Start()
    {
        if (_graphController == null)
        {
            _graphController = GameObject.Find("Mechanism").GetComponent<GraphController>();
        }

        if (_gameplayController == null)
        {
            _gameplayController = GameObject.Find("Mechanism").GetComponent<GameplayController>();
        }

        Invoke("DelayedStart", 0.1f);
    }

    void DelayedStart()
    {
        // Get current level index
        int levelToPlay = PlayerPrefs.GetInt("LevelToPlayIndex", 0);

        // Initialize vertices
        foreach (VertexConfig vertex in _graphController.LevelConfig.levels[levelToPlay].verticies)
        {
            _vertices.Add(vertex.id);
        }

        // Initialize edges
        foreach (EdgeConfig edge in _graphController.LevelConfig.levels[levelToPlay].edges)
        {
            _edges.Add(Tuple.Create(edge.a, edge.b));
        }

        // Instantiate new graph
        _graph = new Graph<int>(_vertices, _edges);

        // Every 3 seconds, after 5 seconds of gameplay time move ai
        InvokeRepeating("MoveAI", 5.0f, 3.0f);
    }

    /// <summary>
    /// Moves units between vertices depending on vertex state
    /// </summary>
    void MoveAI()
    {
        foreach(VertexController vertex in _gameplayController.VertexList)
        {
            // Check if current vertex isn't player or wild
            if (vertex.Owner != OwnerType.Player && vertex.Owner != OwnerType.Wild)
            {
                // Check if vertex has neighbour, 
                // if not then switch to second state
                List<VertexController> enemyNeighbours = new List<VertexController>();

                foreach (GameObject connection in vertex.Connections)
                {
                    VertexController connectedVertex = connection.GetComponent<VertexController>();

                    if (vertex.Owner != connectedVertex.Owner)
                    {
                        // First state, use Dijkstra to 
                        enemyNeighbours.Add(connectedVertex);
                    }
                }

                // Sort list of enemy vertices to find vertex with lowest army power (weight)
                List<VertexController> sortedEnemyNeighbours = enemyNeighbours.OrderBy(o => o.ArmyPower).ToList();

                if (enemyNeighbours.Count > 0)
                {
                    foreach (VertexController enemyVertex in sortedEnemyNeighbours)
                    {
                        // Check if vertex has sufficient amount of army power to move
                        if (vertex.ArmyPower > enemyVertex.ArmyPower * 1.3f)
                        {
                            vertex.SendArmy(enemyVertex.Id, (int)(enemyVertex.ArmyPower * 1.3f));
                        }
                    }
                }

                // Second state, use Breadth-first search algorithm to determine where to move units
                if (enemyNeighbours.Count == 0)
                {
                    List<VertexController> verticesWithEnemyNeighbours = new List<VertexController>();

                    // Search for all vertices of current vertex
                    foreach (VertexController tempVertex in _gameplayController.VertexList)
                    {
                        if (tempVertex.Owner == vertex.Owner)
                        {
                            bool hasEnemyNeighbours = false;

                            foreach(GameObject connectedToEnemyVertex in tempVertex.Connections)
                            {
                                if (connectedToEnemyVertex.GetComponent<VertexController>().Owner != tempVertex.Owner)
                                {
                                    hasEnemyNeighbours = true;
                                }
                            }

                            if (hasEnemyNeighbours)
                            {
                                verticesWithEnemyNeighbours.Add(tempVertex);
                            }
                        }
                    }

                    // Function, which returns shortes path between this vertex and picked
                    Func<int, IEnumerable<int>> shortestPath = ShortestPath(_graph, vertex.Id);

                    int indexOfVertexToTraverse = -1;
                    int armyPowerOfVertexToTraverse = int.MaxValue;
                    int jumpDistanceToNearestMatchingVertex = int.MaxValue;

                    foreach(VertexController tempVertex in verticesWithEnemyNeighbours)
                    {
                        // Shortest path to tempVertex
                        List<int> pathJumps = shortestPath(tempVertex.Id).ToList();

                        if (pathJumps.Count < jumpDistanceToNearestMatchingVertex)
                        {
                            jumpDistanceToNearestMatchingVertex = pathJumps.Count;
                            indexOfVertexToTraverse = pathJumps[1];
                            armyPowerOfVertexToTraverse = tempVertex.ArmyPower;
                        }
                        else if (pathJumps.Count == jumpDistanceToNearestMatchingVertex && armyPowerOfVertexToTraverse > tempVertex.ArmyPower)
                        {
                            jumpDistanceToNearestMatchingVertex = pathJumps.Count;
                            indexOfVertexToTraverse = pathJumps[1];
                            armyPowerOfVertexToTraverse = tempVertex.ArmyPower;
                        }
                    }

                    // If found vertex to traverse, then send army
                    if (indexOfVertexToTraverse != -1)
                    {
                        // Print result
                        Debug.Log($"From {vertex.Id} to {indexOfVertexToTraverse}");

                        if (vertex.ArmyPower > 1)
                        {
                            vertex.SendArmy(indexOfVertexToTraverse, vertex.ArmyPower - 1);
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// Get shortest path from start
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="graph"></param>
    /// <param name="start"></param>
    /// <returns></returns>
    private static Func<T, IEnumerable<T>> ShortestPath<T>(Graph<T> graph, T start)
    {
        // Contains previous vertex neighbours
        Dictionary<T, T> previousVertex = new Dictionary<T, T>();

        Queue<T> queue = new Queue<T>();
        queue.Enqueue(start);

        // Perform until traverse all vertices (empty queue)
        // and in every step add neighbours of current vertex
        while (queue.Count > 0)
        {
            // Get first vertex in the queue to scan for neighbours
            var vertex = queue.Dequeue();

            // For each connected neighbour in adjacency list
            foreach (var neighbour in graph.AdjacencyList[vertex])
            {
                if (previousVertex.ContainsKey(neighbour))
                {
                    continue;
                }

                previousVertex[neighbour] = vertex;

                // Add every neighbour to que
                queue.Enqueue(neighbour);
            }
        }

        // Prepare path of jumps to selected vertex
        IEnumerable<T> ShortestPath(T end)
        {
            List<T> pathOfJumps = new List<T>();

            // Set current to current
            var currentVertex = end;

            // Traverse backward until reach start vertex
            while (!currentVertex.Equals(start))
            {
                // Add current vertex to jump list
                pathOfJumps.Add(currentVertex);
                currentVertex = previousVertex[currentVertex];
            }

            // Add jump at the end
            pathOfJumps.Add(start);

            // Reverse list to order from start to end
            pathOfJumps.Reverse();

            return pathOfJumps;
        }

        return ShortestPath;
    }
}

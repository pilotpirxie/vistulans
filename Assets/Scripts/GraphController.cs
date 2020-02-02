using UnityEngine;

public class GraphController : MonoBehaviour
{
    /// <summary>
    /// Object used on instatiating vertices at start
    /// </summary>
    [SerializeField]
    public GameObject VertexObjectPrefab;

    /// <summary>
    /// Object used on instantiating when player choose two
    /// vertices and send army between them
    /// </summary>
    [SerializeField]
    public GameObject ArmyObjectPrefab;

    /// <summary>
    /// Object with plane connecting two verices
    /// </summary>
    [SerializeField]
    public GameObject RoadObjectPrefab;

    /// <summary>
    /// Reference to gameplay controller from Mechanism object
    /// </summary>
    GameplayController _gameplayController;

    void Start()
    {
        _gameplayController = GameObject.FindWithTag("Mechanism").GetComponent<GameplayController>();

        // Load JSON and parse it
        TextAsset levelConfigContent = Resources.Load<TextAsset>("Config/levels");
        LevelConfig levelConfig = JsonUtility.FromJson<LevelConfig>(levelConfigContent.text);

        // Instantiate vertex for every entry in the LevelConfig
        foreach (VertexConfig vertexConfig in levelConfig.levels[0].verticies)
        {
            // Instantiate and set position based on coordinates
            GameObject newVertex = Instantiate(VertexObjectPrefab, new Vector3(vertexConfig.x * 1f, 0.5f, -vertexConfig.y * 1f), Quaternion.identity);

            // Get vertex controller from new vertex
            VertexController vertexController = newVertex.GetComponent<VertexController>();

            // And set values based on Vertex Config from Level Config
            vertexController.Id = vertexConfig.id;
            vertexController.X = vertexConfig.x;
            vertexController.Y = vertexConfig.y;

            vertexController.Owner = (OwnerType)vertexConfig.owner;
            vertexController.Type = (VertexType)vertexConfig.type;

            vertexController.ArmyPower = vertexConfig.power;
            vertexController.Level = vertexConfig.level;

            // Set meta-data, name and tag name used for future identifying vertex
            newVertex.tag = "Vertex";
            newVertex.name = $"vertex{vertexConfig.id}";
        }

        // Set edges between vertices
        foreach (EdgeConfig connection in levelConfig.levels[0].edges)
        {
            GameObject vertexA = GameObject.Find($"vertex{connection.a}");
            GameObject vertexB = GameObject.Find($"vertex{connection.b}");

            vertexA.GetComponent<VertexController>().Connections.Add(vertexB);
            vertexB.GetComponent<VertexController>().Connections.Add(vertexA);

            SpawnRoad(vertexA.transform.position, vertexB.transform.position);
        }
    }

    /// <summary>
    /// Spawn road using prefab, stretch it to match 
    /// distances and set on the center, between two positions
    /// </summary>
    /// <param name="positionA"></param>
    /// <param name="positionB"></param>
    void SpawnRoad(Vector3 positionA, Vector3 positionB)
    {
        Vector3 targetDirection = positionB - positionA;
        targetDirection.y = 0;
        Quaternion rotation = Quaternion.LookRotation(targetDirection);

        Vector3 center = (positionA + positionB) / 2;
        center.y = 0.02f;
        GameObject road = Instantiate(RoadObjectPrefab, center, rotation);

        Vector3 currentScale = road.transform.localScale;
        float nScale = Vector3.Distance(positionA, positionB);
        Debug.Log(currentScale.y + " " + nScale);
        currentScale.z = nScale / 10;
        road.transform.localScale = currentScale;
    }

    public void FixedUpdate()
    {
        SelectVertices();
        CheckIfSendArmy();
    }

    /// <summary>
    /// Triggered when player touch the vertex
    /// Set selected first or second vertex
    /// </summary>
    /// <param name="id"></param>
    public void OnVertexTouch(int id)
    {
        GameObject vertex = GameObject.Find($"vertex{id}");

        if (_gameplayController.SelectedVertexA == null)
        {
            _gameplayController.SelectedVertexA = vertex.GetComponent<VertexController>();
        }
        else
        {
            _gameplayController.SelectedVertexB = vertex.GetComponent<VertexController>();
        }

        _gameplayController.SetPositionOfSunshaft();
    }

    /// <summary>
    /// Check if player selected two vertices
    /// and they are connected to each other
    /// and has army power higher than 1
    /// and spell to cast is not selected
    /// then send army from A to B
    /// and clear selection
    /// </summary>
    void CheckIfSendArmy()
    {
        // Check if both vertices are selected
        if (_gameplayController.SelectedVertexA != null && _gameplayController.SelectedVertexB != null)
        {
            // Get first vertex
            GameObject firstVertex = GameObject.Find($"vertex{_gameplayController.SelectedVertexA.Id}");

            // Check if second vertex is connected to the first one
            foreach (GameObject connectedVertex in firstVertex.GetComponent<VertexController>().Connections)
            {
                if (connectedVertex.GetComponent<VertexController>().Id == _gameplayController.SelectedVertexB.Id)
                {
                    // Check if firstVertex has more than 1 army power to split, you cannot send last unit
                    if (firstVertex.GetComponent<VertexController>().ArmyPower > 1 && _gameplayController.SpellToCast == -1)
                    {
                        // Calculate how many army power units are going to be send based on transport part
                        // Floor((total - 1) * transportPart) casted to Int
                        int armyPowerToSend = (int)Mathf.Floor((firstVertex.GetComponent<VertexController>().ArmyPower - 1) * _gameplayController.TransportPart);

                        // Send signal to send army from vertex A to B with amount of units calculated above
                        SendArmy(_gameplayController.SelectedVertexA.Id, _gameplayController.SelectedVertexB.Id, armyPowerToSend);
                        Debug.Log($"Sent unit from {_gameplayController.SelectedVertexA.Id} to {_gameplayController.SelectedVertexB.Id} with {armyPowerToSend} army power");
                    }
                }
            }

            // Clear highlights from vertices
            ClearSelection();
        }
    }

    /// <summary>
    /// If only one vertex is selected highlight selected by player vertex
    /// and each vertex connected to it 
    /// </summary>
    void SelectVertices()
    {
        if (_gameplayController.SelectedVertexA != null && _gameplayController.SelectedVertexB == null)
        {
            GameObject selectedVertex = GameObject.Find($"vertex{_gameplayController.SelectedVertexA.Id}");
            selectedVertex.GetComponent<VertexController>().Selected = true;

            foreach (GameObject connectedVertex in selectedVertex.GetComponent<VertexController>().Connections)
            {
                connectedVertex.GetComponent<VertexController>().Highlighted = true;
            }
        }
    }

    /// <summary>
    /// Hide highlights from every vertex
    /// </summary>
    public void ClearSelection()
    {
        foreach (GameObject vertex in GameObject.FindGameObjectsWithTag("Vertex"))
        {
            vertex.GetComponent<VertexController>().Selected = false;
            vertex.GetComponent<VertexController>().Highlighted = false;
        }

        _gameplayController.SelectedVertexA = null;
        _gameplayController.SelectedVertexB = null;
    }

    /// <summary>
    /// Instantiate army object and send it from origin to target
    /// </summary>
    /// <param name="origin">Vertex A</param>
    /// <param name="target">Vertex B</param>
    /// <param name="amount">Army power to send</param>
    public void SendArmy(int origin, int target, int amount)
    {
        GameObject vertexA = GameObject.Find($"vertex{origin}");
        VertexController originVertexController = vertexA.GetComponent<VertexController>();

        if (originVertexController.ArmyPower >= amount)
        {
            // Substract army power from the origin
            originVertexController.ArmyPower -= amount;

            // Position for new army object
            Vector3 spawnPosition = vertexA.gameObject.transform.position;
            spawnPosition.y = 0.25f;

            // Instantiate army object
            GameObject newArmy = Instantiate(ArmyObjectPrefab, spawnPosition, Quaternion.identity);
            ArmyController armyController = newArmy.GetComponent<ArmyController>();

            // Set army controller data
            armyController.Owner = originVertexController.Owner;
            armyController.ArmyPower = amount;
            armyController.OriginVertexIndexId = origin;
            armyController.TargetVertexIndexId = target;
        }
    }
}

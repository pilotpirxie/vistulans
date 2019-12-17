using UnityEngine;

public class GraphController : MonoBehaviour
{
    [SerializeField]
    public GameObject VertexObject;

    [SerializeField]
    public GameObject ArmyObject;

    private GameplayController _gameplayController;

    void Start()
    {
        _gameplayController = GameObject.FindWithTag("Mechanism").GetComponent<GameplayController>();

        TextAsset levelConfigContent = Resources.Load<TextAsset>("Config/levels");
        Debug.Log($"Loaded level configuration: {levelConfigContent}");
        LevelConfig levelConfig = JsonUtility.FromJson<LevelConfig>(levelConfigContent.text);

        foreach (VertexConfig vertexConfig in levelConfig.levels[0].verticies)
        {
            GameObject newVertex = GameObject.Instantiate(VertexObject, new Vector3(vertexConfig.x * 1f, 0.5f, -vertexConfig.y * 1f), Quaternion.identity);
            newVertex.GetComponent<VertexController>().X = vertexConfig.x;
            newVertex.GetComponent<VertexController>().Y = vertexConfig.y;
            newVertex.GetComponent<VertexController>().Owner = (OwnerType)vertexConfig.owner;
            newVertex.GetComponent<VertexController>().Type = (VertexType)vertexConfig.type;
            newVertex.GetComponent<VertexController>().ArmyPower = vertexConfig.power;
            newVertex.GetComponent<VertexController>().Level = vertexConfig.level;
            newVertex.GetComponent<VertexController>().Id = vertexConfig.id;
            newVertex.tag = "Vertex";
            newVertex.name = $"vertex{vertexConfig.id}";
        }

        foreach (EdgeConfig connection in levelConfig.levels[0].edges)
        {
            GameObject vertexA = GameObject.Find($"vertex{connection.a}");
            GameObject vertexB = GameObject.Find($"vertex{connection.b}");

            vertexA.GetComponent<VertexController>().Connections.Add(vertexB);
            vertexB.GetComponent<VertexController>().Connections.Add(vertexA);
        }
    }

    public void OnVertexTouch(int id)
    {
        if (_gameplayController.SelectedVertexA == null)
        {
            _gameplayController.SelectedVertexA = GameObject.Find($"vertex{id}").GetComponent<VertexController>();
        }
        else
        {
            _gameplayController.SelectedVertexB = GameObject.Find($"vertex{id}").GetComponent<VertexController>();
        }
    }

    public void FixedUpdate()
    {
        if (_gameplayController.SelectedVertexA != null && _gameplayController.SelectedVertexB == null)
        {
            GameObject selectedVertex = GameObject.Find($"vertex{_gameplayController.SelectedVertexA.Id}");
            selectedVertex.GetComponent<Renderer>().material.color = Color.white;

            foreach (GameObject connectedVertex in selectedVertex.GetComponent<VertexController>().Connections)
            {
                connectedVertex.GetComponent<Renderer>().material.color = Color.yellow;
            }
        }

        if (_gameplayController.SelectedVertexA != null && _gameplayController.SelectedVertexB != null)
        {
            GameObject selectedVertex = GameObject.Find($"vertex{_gameplayController.SelectedVertexA.Id}");

            foreach (GameObject possibleVertex in selectedVertex.GetComponent<VertexController>().Connections)
            {
                if (possibleVertex.GetComponent<VertexController>().Id == _gameplayController.SelectedVertexB.Id)
                {
                    if (selectedVertex.GetComponent<VertexController>().ArmyPower > 1 && _gameplayController.SpellToCast == -1)
                    {
                        int armyPowerToSend = (int)Mathf.Floor(selectedVertex.GetComponent<VertexController>().ArmyPower * _gameplayController.TransportPart);
                        selectedVertex.GetComponent<VertexController>().ArmyPower -= armyPowerToSend;

                        SendArmy(_gameplayController.SelectedVertexA.Id, _gameplayController.SelectedVertexB.Id, armyPowerToSend);
                        Debug.Log($"Sent unit from {_gameplayController.SelectedVertexA.Id} to {_gameplayController.SelectedVertexB.Id}");
                    }
                }
            }

            ClearSelection();
        }
    }

    public void ClearSelection()
    {
        foreach (GameObject vertex in GameObject.FindGameObjectsWithTag("Vertex"))
        {
            vertex.GetComponent<Renderer>().material.color = Color.clear;
        }

        _gameplayController.SelectedVertexA = null;
        _gameplayController.SelectedVertexB = null;
    }

    public void SendArmy(int origin, int target, int amount)
    {
        GameObject vertexA = GameObject.Find($"vertex{origin}");
        GameObject vertexB = GameObject.Find($"vertex{target}");

        if (vertexA.GetComponent<VertexController>().ArmyPower >= amount)
        {
            Vector3 spawnPosition = vertexA.gameObject.transform.position;
            spawnPosition.y = 0.25f;
            GameObject newArmy = GameObject.Instantiate(ArmyObject, spawnPosition, Quaternion.identity);
            newArmy.GetComponent<ArmyController>().Owner = vertexA.GetComponent<VertexController>().Owner;
            newArmy.GetComponent<ArmyController>().ArmyPower = amount;
            newArmy.GetComponent<ArmyController>().Origin = origin;
            newArmy.GetComponent<ArmyController>().Target = target;
        }
        else
        {
            // insufficient army power
        }
    }
}

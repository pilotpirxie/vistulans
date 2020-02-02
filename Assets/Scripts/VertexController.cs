using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Type of vertex, represents what vertex produces
/// </summary>
public enum VertexType
{
    Village,
    Shrine,
    Apiary
};

/// <summary>
/// Owner of vertex
/// </summary>
public enum OwnerType
{
    Wild,
    Player,
    EnemyOne,
    EnemyTwo,
    EnemyThree
};

public class VertexController : MonoBehaviour
{
    /// <summary>
    /// Object of badge used for instantiating
    /// </summary>
    public GameObject BadgeObjectPrefab;

    /// <summary>
    /// White circle object displaying below mesh
    /// </summary>
    public GameObject WhiteCirclePrefab;

    /// <summary>
    /// Meshes used for rendering villages on different levels
    /// </summary>
    public List<GameObject> MeshLevelsVillage;

    /// <summary>
    /// Meshes used for rendering apiary on different levels
    /// </summary>
    public List<GameObject> MeshLevelsApiary;

    /// <summary>
    /// Meshes used for rendering shrines on different levels
    /// </summary>
    public List<GameObject> MeshLevelsShrine;

    /// <summary>
    /// Object actually rendered
    /// </summary>
    private GameObject _viewObject;

    /// <summary>
    /// Reference to badge controller for this vertex
    /// </summary>
    private BadgeController _badgeController;

    /// <summary>
    /// Reference to mechanism object
    /// </summary>
    private GameObject _mechanismObject;

    /// <summary>
    /// Id of vertex, used for comparing and name
    /// </summary>
    public int Id;

    /// <summary>
    /// Type of vertex
    /// </summary>
    public VertexType Type;

    /// <summary>
    /// X coordinate
    /// </summary>
    public int X;

    /// <summary>
    /// Y coordinate
    /// </summary>
    public int Y;

    /// <summary>
    /// How fast vertex produce goods
    /// </summary>
    public int Level;

    /// <summary>
    /// List of connections (edges) to other vertices
    /// </summary>
    public List<GameObject> Connections;

    /// <summary>
    /// How many army units stay in the vertex
    /// </summary>
    public int ArmyPower;

    /// <summary>
    /// Determine who owns the vertex
    /// </summary>
    public OwnerType Owner;

    /// <summary>
    /// Flag, is vertex selected or not
    /// </summary>
    public bool Selected = false;

    /// <summary>
    /// Flag, is vertex highlighted or not
    /// </summary>
    public bool Highlighted = false;

    void Start()
    {
        GameObject newBadge = GameObject.Instantiate(BadgeObjectPrefab, gameObject.transform.position - new Vector3(0.15f, 0.5f, 1.5f), Quaternion.identity);
        _badgeController = newBadge.GetComponent<BadgeController>();

        GameObject circle = GameObject.Instantiate(WhiteCirclePrefab, gameObject.transform.position - new Vector3(0, 0.45f, 0.3f), Quaternion.Euler(90, 0, 0));

        if (_mechanismObject == null)
        {
            _mechanismObject = GameObject.Find("Mechanism");
        }

        SetViewObject();
    }

    void FixedUpdate()
    {
        UpdateBadgeUI();
    }

    /// <summary>
    /// Set new view object
    /// </summary>
    public void SetViewObject()
    {
        if (_viewObject != null)
        {
            Destroy(_viewObject);
        }

        Vector3 currPos = transform.position;

        switch (Type)
        {
            case VertexType.Village:
                _viewObject = Instantiate(MeshLevelsVillage[Level - 1], currPos - new Vector3(0.2f, 0f, 0f), Quaternion.identity);
                break;
            case VertexType.Apiary:
                _viewObject = Instantiate(MeshLevelsApiary[Level - 1], currPos - new Vector3(0.3f, 0f, 0.5f), Quaternion.identity);
                break;
            case VertexType.Shrine:
                _viewObject = Instantiate(MeshLevelsShrine[Level - 1], currPos + new Vector3(0f, -0.2f, -0.1f), Quaternion.identity);
                break;
        }

        if (_viewObject != null)
        {
            _viewObject.transform.parent = gameObject.transform.parent;
        }
    }

    /// <summary>
    /// Set new values for badge
    /// </summary>
    void UpdateBadgeUI()
    {
        _badgeController.Level = Level;
        _badgeController.ArmyPower = ArmyPower;
        _badgeController.Type = Type;
        _badgeController.Owner = Owner;
    }

    /// <summary>
    /// Call graph controller when player touch or click on vertex
    /// </summary>
    private void OnMouseDown()
    {
        _mechanismObject.GetComponent<GraphController>().OnVertexTouch(gameObject.GetComponent<VertexController>().Id);
    }
}

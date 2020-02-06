using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class ArmyController : MonoBehaviour
{
    /// <summary>
    /// Index Id of vertex where army object was spawn
    /// </summary>
    public int OriginVertexIndexId = -1;

    /// <summary>
    /// Index Id where army is going
    /// </summary>
    public int TargetVertexIndexId = -1;

    /// <summary>
    /// Target object to reach
    /// </summary>
    private GameObject _targetObject;

    /// <summary>
    /// Power of army
    /// </summary>
    public int ArmyPower = 0;

    /// <summary>
    /// Owner of army
    /// </summary>
    public OwnerType Owner = OwnerType.Wild;

    /// <summary>
    /// Speed of army
    /// </summary>
    public float MovementSpeed = 1f;

    /// <summary>
    /// Flag used to determine which army triggered first. 
    /// Because two armies can meet each other, somewhere in 
    /// the edge - both may influence on each other.
    /// When this flag is checked, army don't affect,
    /// because "battle" is already started by the second one army.
    /// </summary>
    public bool AlreadyTriggering = false;

    /// <summary>
    /// Text with army power
    /// </summary>
    [SerializeField]
    private TextMeshProUGUI _bannerText;

    /// <summary>
    /// Background of banner to replace
    /// </summary>
    [SerializeField]
    private SpriteRenderer _bannerBackground;

    /// <summary>
    /// Possible banners to use
    /// </summary>
    public List<Sprite> BannerBackgrounds = new List<Sprite>();

    /// <summary>
    /// Set target object based on index id
    /// and color of material for army object mesh
    /// </summary>
    void UpdateTarget()
    {
        if (_targetObject == null)
        {
            _targetObject = GameObject.Find($"vertex{TargetVertexIndexId}");

            switch (Owner)
            {
                case OwnerType.EnemyOne:
                    gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
                    break;
                case OwnerType.EnemyTwo:
                    gameObject.GetComponent<MeshRenderer>().material.color = Color.green;
                    break;
                case OwnerType.EnemyThree:
                    gameObject.GetComponent<MeshRenderer>().material.color = Color.yellow;
                    break;
                case OwnerType.Player:
                    gameObject.GetComponent<MeshRenderer>().material.color = Color.cyan;
                    break;
                default:
                    gameObject.GetComponent<MeshRenderer>().material.color = Color.gray;
                    break;
            }
        }
    }

    void UpdateBanner()
    {
        _bannerText.text = $"{ArmyPower}";
        _bannerBackground.sprite = BannerBackgrounds[(int)Owner];
    }

    /// <summary>
    /// Set position of army object
    /// in target direction
    /// </summary>
    void MoveArmy()
    {
        if (_targetObject != null)
        {
            Vector3 targetDirection = _targetObject.gameObject.transform.position - transform.position;
            targetDirection.y = 0;
            transform.rotation = Quaternion.LookRotation(targetDirection);
            gameObject.transform.position += gameObject.transform.forward * MovementSpeed * Time.deltaTime;
        }
    }

    void FixedUpdate()
    {
        UpdateTarget();
        MoveArmy();
        UpdateBanner();
    }

    /// <summary>
    /// Triggered when two armies stop triggering each other
    /// </summary>
    /// <param name="other">Collider of the second army</param>
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Army")
        { 
            other.gameObject.GetComponent<ArmyController>().AlreadyTriggering = false;
        }
    }

    /// <summary>
    /// Triggered when two armies start triggering each other
    /// </summary>
    /// <param name="other">Collider of the second army</param>
    private void OnTriggerEnter(Collider other)
    {
        // Check if is not already affected by the second army
        if (AlreadyTriggering == false)
        {
            // Depending if the second object is vertex or army
            // call another method
            if (other.gameObject.tag == "Vertex")
            {
                CollideWithVertex(other.gameObject);
            }
            else if (other.gameObject.tag == "Army")
            {
                CollideWithArmy(other.gameObject);
            }
        }
    }

    /// <summary>
    /// Triggered on trigger enter with vertex
    /// </summary>
    /// <param name="vertex">Triggered vertex object</param>
    void CollideWithVertex(GameObject vertex)
    {
        VertexController vertexController = vertex.GetComponent<VertexController>();

        // Check if vertex is different than origin
        if (vertexController.Id != OriginVertexIndexId)
        {
            // Depending on owner of the vertex
            // sum army unit or perform battle
            if (vertexController.Owner == Owner)
            {
                vertexController.ArmyPower += ArmyPower;
            }
            else
            {
                vertexController.ArmyPower -= ArmyPower;

                if (vertexController.ArmyPower <= 0)
                {
                    vertexController.Owner = Owner;
                    vertexController.ArmyPower = Mathf.Abs(vertexController.ArmyPower);
                }
            }

            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Triggered on trigger enter with army
    /// </summary>
    /// <param name="secondArmy">Second army object</param>
    void CollideWithArmy(GameObject secondArmy)
    {
        ArmyController secondArmyController = secondArmy.GetComponent<ArmyController>();

        // Check if second army object is not triggering at this moment
        // and if has different owner
        if (secondArmyController.AlreadyTriggering == false && secondArmyController.Owner != Owner)
        {
            // Set flag on the second army to inform that this army object
            // is going to perform battle between them
            secondArmyController.AlreadyTriggering = true;

            if (secondArmyController.ArmyPower < ArmyPower)
            {
                ArmyPower -= secondArmyController.ArmyPower;
                Destroy(secondArmy);
            }
            else if (secondArmyController.ArmyPower > ArmyPower)
            {
                secondArmyController.ArmyPower -= ArmyPower;
                Destroy(gameObject);
            }
            else
            {
                Destroy(gameObject);
                Destroy(secondArmy);
            }
        }
    }
}

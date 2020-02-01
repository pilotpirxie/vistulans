using UnityEngine;

public class ArmyController : MonoBehaviour
{
    public int OriginVertexIndexId = -1;

    public int TargetVertexIndexId = -1;

    private GameObject _targetObject;

    public int ArmyPower = 0;

    public OwnerType Owner = OwnerType.Wild;

    public float MovementSpeed = 1f;

    public bool AlreadyTriggering = false;

    void UpdateTarget(int newTarget)
    {
        TargetVertexIndexId = newTarget;
        _targetObject = GameObject.Find($"vertex{TargetVertexIndexId}");
    }

    void FixedUpdate()
    {
        if (_targetObject == null)
        {
            UpdateTarget(TargetVertexIndexId);

            switch(Owner)
            {
                case OwnerType.EnemyOne:
                    gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
                    break;
                case OwnerType.EnemyTwo:
                    gameObject.GetComponent<MeshRenderer>().material.color = Color.green;
                    break;
                case OwnerType.EnemyThree:
                    gameObject.GetComponent<MeshRenderer>().material.color = Color.gray;
                    break;
                case OwnerType.Player:
                    gameObject.GetComponent<MeshRenderer>().material.color = Color.cyan;
                    break;
            }
        }
        else
        {
            Vector3 targetDirection = _targetObject.gameObject.transform.position - transform.position;
            targetDirection.y = 0;
            transform.rotation = Quaternion.LookRotation(targetDirection);
            gameObject.transform.position += gameObject.transform.forward * MovementSpeed * Time.deltaTime;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Army")
        { 
            other.gameObject.GetComponent<ArmyController>().AlreadyTriggering = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (AlreadyTriggering == false)
        {
            if (other.gameObject.tag == "Vertex")
            {
                if (other.gameObject.GetComponent<VertexController>().Id != OriginVertexIndexId)
                {
                    if (other.gameObject.GetComponent<VertexController>().Owner == Owner)
                    {
                        other.gameObject.GetComponent<VertexController>().ArmyPower += ArmyPower;
                    }
                    else
                    {
                        other.gameObject.GetComponent<VertexController>().ArmyPower -= ArmyPower;

                        if (other.gameObject.GetComponent<VertexController>().ArmyPower <= 0)
                        {
                            other.gameObject.GetComponent<VertexController>().Owner = Owner;
                            other.gameObject.GetComponent<VertexController>().ArmyPower = Mathf.Abs(other.gameObject.GetComponent<VertexController>().ArmyPower);
                        }
                    }

                    Destroy(gameObject);
                }
            }
            else if (other.gameObject.tag == "Army")
            {
                if (other.gameObject.GetComponent<ArmyController>().AlreadyTriggering == false && other.gameObject.GetComponent<ArmyController>().Owner != Owner)
                {
                    other.gameObject.GetComponent<ArmyController>().AlreadyTriggering = true;

                    if (other.gameObject.GetComponent<ArmyController>().ArmyPower < ArmyPower)
                    {
                        ArmyPower -= other.gameObject.GetComponent<ArmyController>().ArmyPower;

                        Destroy(other.gameObject);

                    }
                    else if (other.gameObject.GetComponent<ArmyController>().ArmyPower > ArmyPower)
                    {
                        other.gameObject.GetComponent<ArmyController>().ArmyPower -= ArmyPower;

                        Destroy(gameObject);
                    }
                    else
                    {
                        Destroy(gameObject);
                        Destroy(other.gameObject);
                    }
                }
            }
        }
    }
}

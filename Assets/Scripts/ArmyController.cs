using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmyController : MonoBehaviour
{
    [SerializeField]
    public int Origin = -1;

    [SerializeField]
    public int Target = -1;

    [SerializeField]
    private GameObject TargetObject;

    [SerializeField]
    public int ArmyPower = 0;

    [SerializeField]
    public OwnerType Owner = OwnerType.Wild;

    [SerializeField]
    private bool AlreadyTriggering = false;

    void UpdateTarget(int newTarget)
    {
        Target = newTarget;
        TargetObject = GameObject.Find($"vertex{Target}");
    }

    void FixedUpdate()
    {
        if (TargetObject == null)
        {
            UpdateTarget(Target);
        }
        else
        {
            Vector3 targetDirection = TargetObject.gameObject.transform.position - transform.position;
            targetDirection.y = 0;
            transform.rotation = Quaternion.LookRotation(targetDirection);
            gameObject.transform.position += gameObject.transform.forward * 0.1f;
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
        Debug.Log(other.tag);

        if (AlreadyTriggering == false)
        {
            if (other.gameObject.tag == "Vertex")
            {
                if (other.gameObject.GetComponent<VertexController>().Id != Origin)
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

                    GameObject.Destroy(gameObject);
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

                        GameObject.Destroy(other.gameObject);

                    }
                    else if (other.gameObject.GetComponent<ArmyController>().ArmyPower > ArmyPower)
                    {
                        other.gameObject.GetComponent<ArmyController>().ArmyPower -= ArmyPower;

                        GameObject.Destroy(gameObject);
                    }
                    else
                    {
                        GameObject.Destroy(gameObject);
                        GameObject.Destroy(other.gameObject);
                    }
                }
            }
        }
    }
}

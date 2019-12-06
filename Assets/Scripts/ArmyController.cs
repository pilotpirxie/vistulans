using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmyController : MonoBehaviour
{
    [SerializeField]
    public int Target = -1;

    [SerializeField]
    private GameObject TargetObject;

    [SerializeField]
    public int ArmyPower = 0;

    [SerializeField]
    public OwnerType Owner = OwnerType.Wild;

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
}

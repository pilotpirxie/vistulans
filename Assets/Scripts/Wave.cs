using UnityEngine;

public class Wave : MonoBehaviour
{
    public Vector3 MaxSize = new Vector3(0.02f, 1f, 0.02f);
    public bool IsWaving = false;

    void FixedUpdate()
    {
        if (IsWaving)
        {
            transform.localScale = new Vector3(MaxSize.x, MaxSize.y, (Mathf.Abs(Mathf.Sin(Time.time) / 5) * MaxSize.z) + MaxSize.z);
        }
    }
}

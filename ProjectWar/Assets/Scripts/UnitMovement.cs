using UnityEngine;

public class UnitMovement : MonoBehaviour
{
    public float speed = 2f;
    public Vector3 direction = Vector3.forward;

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }
}

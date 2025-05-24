using UnityEngine;

public class UnitMovement : MonoBehaviour
{
    public float speed = 2f;
    public Vector3 direction = Vector3.forward;
    private bool canMove = true;

    public void StopMoving()
    {
        canMove = false;
    }

    void Update()
    {
        if (canMove)
        {
            transform.Translate(direction * speed * Time.deltaTime);
        }
    }
}

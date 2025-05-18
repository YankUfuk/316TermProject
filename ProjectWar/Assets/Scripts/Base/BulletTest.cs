using UnityEngine;

public class BulletTest : MonoBehaviour
{
    private Transform target;
    public float speed = 10f;
    public int damage = 1;

    public void SetTarget(Transform t)
    {
        target = t;
    }

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 dir = target.position - transform.position;
        float moveDistance = speed * Time.deltaTime;

        if (dir.magnitude <= moveDistance)
        {
            HitTarget();
            return;
        }

        transform.Translate(dir.normalized * moveDistance, Space.World);
    }

    void HitTarget()
    {
        EnemyHealthTest eh = target.GetComponent<EnemyHealthTest>();
        if (eh != null)
        {
            eh.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}

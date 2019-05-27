using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("References")]
    [SerializeField] LayerMask collisionMask;

    readonly float speed = 10;

    float maxLifetime = 2;
    float currentLifetime = 0;

    public void OnSpawn()
    {
        currentLifetime = 0;
    }

    void Update()
    {
        ControlLifeTime();

        float moveDistance = speed * Time.deltaTime;
        CheckCollisions(moveDistance);
        transform.Translate(Vector3.up * moveDistance);
    }

    void CheckCollisions(float moveDistance)
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, moveDistance, collisionMask, QueryTriggerInteraction.Collide))
        {
            OnHitObject(hit);
        }
    }

    private void OnHitObject(RaycastHit hit)
    {
        Ball ball = hit.collider.GetComponent<Ball>();
        print(ball.name);
        gameObject.SetActive(false);
    }

    void Despawn()
    {
        gameObject.SetActive(false);
    }

    void ControlLifeTime()
    {
        if (currentLifetime >= maxLifetime)
            Despawn();
        else
            currentLifetime += Time.deltaTime;
    }
}

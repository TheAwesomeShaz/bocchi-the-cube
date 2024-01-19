using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float bulletLifetime;
    [SerializeField] private int bulletDamage;
    [SerializeField] private GameObject bulletDestroyVFX;
 
    void Update()
    {
        transform.Translate(Vector3.forward * bulletSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out EnemyController enemy))
        {
            GameObject bulletFX = Instantiate(bulletDestroyVFX, transform.position, Quaternion.identity);
            Destroy(bulletFX, 1f);
            enemy.TakeDamage(bulletDamage);
            Destroy(gameObject);
        }
        else
        {
            GameObject bulletFX = Instantiate(bulletDestroyVFX, transform.position, Quaternion.identity);
            Destroy(bulletFX, 1f);
            Destroy(gameObject);
        }
    }
}

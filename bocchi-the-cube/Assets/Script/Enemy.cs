using System;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private int health = 20;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private GameObject deathVFX;
    [SerializeField] private GameObject hurtVFX;

    private Transform target;
    private Vector3 directionToTarget;

    public void TakeDamage(int bulletDamage)
    {
        if (health <= 0)
        {
            GameController.Instance.PlaySound(AudioNames.Explode);
            var deathFX = Instantiate(deathVFX,transform.position,Quaternion.identity);
            Destroy(deathFX,2f);
            Destroy(gameObject);
            return;
        }
        else
        {
            GameController.Instance.PlaySound(AudioNames.Hurt);
            var hurtFX = Instantiate(hurtVFX, transform.position, Quaternion.identity);
            Destroy(hurtFX, 2f);
            health -= bulletDamage;
        }
    }

    private void Start()
    {
        target = GameController.Instance.playerTransform;
        rb = GetComponent<Rigidbody>(); 
    }

    private void Update()
    {
        RotateTowardsTarget();
        MoveTowardsTargetPosition();
    }

    private void RotateTowardsTarget()
    {
        if (target != null)
        {
            directionToTarget = target.position - transform.position;
            transform.rotation = Quaternion.LookRotation(directionToTarget);
        }
    }

    private void MoveTowardsTargetPosition()
    {
        if (target != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
        }
    }
}

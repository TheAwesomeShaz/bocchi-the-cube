using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private GameObject flashVFX;
    [SerializeField] private GameObject deathVFX;


    private Rigidbody rb;
    private float currentMoveSpeed;
    private Vector3 movementVector;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentMoveSpeed = moveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
        RotateTowardsMouse();
        Shoot();
    }

    private void Shoot()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameController.Instance.PlaySound(AudioNames.Shoot);
            var bullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.LookRotation(transform.forward));
            var flashFX = Instantiate(flashVFX, shootPoint.position, Quaternion.LookRotation(transform.forward));
            Destroy(bullet, 2f);
            Destroy(flashFX, 2f);
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void RotateTowardsMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray,out hit))
        {
            Vector3 targetDirection = new Vector3(hit.point.x, transform.position.y, hit.point.z);
            transform.rotation = Quaternion.LookRotation(targetDirection);
        }
    }

    private void MovePlayer ()
    {
        rb.velocity = movementVector * currentMoveSpeed;
    }

    private void HandleInput()
    {
        movementVector = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            currentMoveSpeed = moveSpeed * 2;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            currentMoveSpeed = moveSpeed;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.TryGetComponent(out EnemyController enemy))
        {
            GameController.Instance.PlaySound(AudioNames.Explode);
            GameController.Instance.isPlayerDead = true;
            var deathFX = Instantiate(deathVFX, transform.position, Quaternion.identity);
            Destroy(deathFX,2f);
            Destroy(gameObject);
        }
    }
}

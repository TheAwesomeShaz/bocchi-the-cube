using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private GameObject muzzleFlashVFX;
    [SerializeField] private GameObject playerDeathVFX;

    private float currentMoveSpeed;
    private Vector3 movementVector;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        mainCamera = Camera.main;
        currentMoveSpeed = moveSpeed;
    }

    private void Update()
    {
        HandleInput();
        RotatePlayerToMouse();
        Shoot();
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        rb.velocity = movementVector * currentMoveSpeed;
    }

    private void HandleInput()
    {
        movementVector = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;
        movementVector.y = 0f;

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            currentMoveSpeed = moveSpeed * 2;
        }
    }
  
    private void Shoot()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameController.Instance.PlaySound(AudioNames.Shoot);
            Bullet bullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.LookRotation(transform.forward));
            GameObject flashFX = Instantiate(muzzleFlashVFX, shootPoint.position, Quaternion.LookRotation(transform.forward));
            Destroy(bullet, 1f);
            Destroy(flashFX, 1f);
        }
    }

    private void RotatePlayerToMouse()
    {   
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Vector3 targetPosition = new Vector3(hit.point.x, transform.position.y, hit.point.z);
            Vector3 directionToLook = targetPosition - transform.position;

            // Rotate the player to look at the mouse point
            if (directionToLook != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(directionToLook, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.TryGetComponent(out EnemyController enemy))
        {
            GameController.Instance.isPlayerDead = true;
            GameController.Instance.PlaySound(AudioNames.Explode);
            GameObject deathVFX = Instantiate(playerDeathVFX, transform.position, Quaternion.identity);
            Destroy(deathVFX,2f);
            Destroy(gameObject);
        }
    }

}


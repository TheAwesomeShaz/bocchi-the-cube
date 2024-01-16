using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float moveSpeed;
    [SerializeField] private CharacterController characterController;

    void Start()
    {
        //rb = GetComponent<Rigidbody>();
        characterController = GetComponent<CharacterController>();
    }

    private void FixedUpdate()
    {
        MovePlayer();
        RotatePlayerToMouse();
    }

    private void MovePlayer()
    {
        // Get input axes
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        // Calculate movement direction
        Vector3 moveDirection = new Vector3(horizontalInput, 0f, verticalInput).normalized;

        // Check if there is any input (non-zero direction)
        if (moveDirection.magnitude >= 0.1f)
        {
            // Rotate move direction to be relative to the camera's orientation
            moveDirection = Camera.main.transform.TransformDirection(moveDirection);

            // Ensure the y component of the movement direction is 0 to prevent flying or falling through the ground
            moveDirection.y = 0f;

            // Normalize the move vector after transformation to avoid faster diagonal movement
            moveDirection = moveDirection.normalized;

            // Calculate movement vector
            Vector3 moveVector = moveDirection * moveSpeed * Time.deltaTime;

            // Move the player
            characterController.Move(moveVector);
        }
    }

    private void RotatePlayerToMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
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

    //private void OnTriggerEnter(Collider other)
    //{
    //    if(other.tag == "Wall")
    //    {
    //        // spawn particle fx
    //        Destroy(gameObject);
    //    }
    //}

}


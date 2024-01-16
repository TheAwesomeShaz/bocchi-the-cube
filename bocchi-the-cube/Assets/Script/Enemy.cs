using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 0.5f;
    private PlayerController target;
    // Start is called before the first frame update
    void Start()
    {
        target = GameController.Instance.playerController;
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, GameController.Instance.playerController.transform.position, moveSpeed);
    }
}

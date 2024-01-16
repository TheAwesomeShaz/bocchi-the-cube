using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    [SerializeField] private EnemyController EnemyPrefab;
    [SerializeField] private float spawnInterval;

    void Start()
    {
        StartCoroutine(StartSpawningEnemyAfterTime(spawnInterval));       
    }

    private void Update()
    {
        if (GameController.Instance.isPlayerDead)
        {
            StopAllCoroutines();
        }
    }

    private IEnumerator StartSpawningEnemyAfterTime(float time)
    {
        while (!GameController.Instance.isPlayerDead)
        {
            yield return new WaitForSeconds(time);
            Instantiate(EnemyPrefab,transform.position,Quaternion.identity);
        }
    }
}

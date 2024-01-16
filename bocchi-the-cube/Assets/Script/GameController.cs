using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public Transform playerTransform;
    public static GameController Instance;
    public bool isPlayerDead;

    private AudioSource audioSource;
    [SerializeField] private AudioClip shootSFX;
    [SerializeField] private AudioClip explodeSFX;
    [SerializeField] private AudioClip hurtSFX;



    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        if (playerTransform == null)
        {
            playerTransform = FindObjectOfType<PlayerController>().transform;
        }


        if (Instance == null)
        {
            Instance = this;
        }
        else 
        { 
            Destroy(gameObject);        
        }
    }

    public void PlaySound(AudioNames audioName)
    {
        switch (audioName)
        {
            case AudioNames.Shoot:
                audioSource.PlayOneShot(shootSFX, 0.5f);        
                break;
            case AudioNames.Hurt:
                audioSource.PlayOneShot(hurtSFX, 0.5f);        
                break;
            case AudioNames.Explode:
                audioSource.PlayOneShot(explodeSFX, 0.5f);        
                break;
            default:
                break;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

}
public enum AudioNames
{
    Shoot,
    Hurt,
    Explode,
}
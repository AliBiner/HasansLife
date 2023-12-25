using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats instance;

    public float health;
    public Vector3 playerPosition;
    public GameObject gameManager;
    public int nextLevelIndex;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            DontDestroyOnLoad(gameManager);
        }
        else
        {
            Destroy(gameObject);
            Destroy(gameManager);
        }
    }
}

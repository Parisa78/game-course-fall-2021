using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public EnemyConfig config;
    private float health;

    void Awake()
    {
        health = config.maxHealth;
    }

    void Update()
    {
        
    }

    public void Damage(float hitpoints)
    {
        health -= hitpoints;
        Debug.Log("ENEMEY DAMAGE; HEALTH: " + health);

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {

    }
}

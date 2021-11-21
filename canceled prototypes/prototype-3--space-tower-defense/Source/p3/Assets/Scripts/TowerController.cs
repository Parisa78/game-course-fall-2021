using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerController : MonoBehaviour
{
    public TowerConfig config;
    public List<EnemyController> enemiesInRange;

    private void Awake()
    {
        enemiesInRange = new List<EnemyController>();
    }

    void Start()
    {
        GetComponent<SphereCollider>().radius = config.radius;
    }

    private void Update()
    {
        // make this a coroutine
        HitEnemies();    
    }

    private void HitEnemies()
    {
        foreach (EnemyController e in enemiesInRange)
        {
            float hitpoint = (config.maxHitPoint + config.minHitPoint) / 2;
            e.Damage(hitpoint);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("ENEMY");

            // keep the track of the enemey and add them to a list
            enemiesInRange.Add(other.GetComponent<EnemyController>());
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // keep defensing from the seen target
    }

    private void OnTriggerExit(Collider other)
    {
        // delete the ones who are dead
        
        // delete the one who escaped from the radius
    }
}

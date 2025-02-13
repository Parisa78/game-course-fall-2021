﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodInstanceController : MonoBehaviour
{
    public FoodItemConfig config;
    private Rigidbody rigidBody;
    private UiManager uiManager;
    private PlayerController player;
    private void Start()
    {
        // change mass based on config
        rigidBody = GetComponent<Rigidbody>();
        player = FindObjectOfType<PlayerController>();
        rigidBody.mass = config.weight;

        // rotate randomly when instantiating
        transform.Rotate(0, Random.Range(-45, 45), 0);

        uiManager = FindObjectOfType<UiManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Kill"))
        {
            uiManager.ScoreCount(-1*config.score);
            player.playerScore -= config.score;
            Destroy(this.gameObject);
        }
    }
}

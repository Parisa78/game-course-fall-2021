﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Range(0f, 1f)] public float moveAmount;

    public int playerScore;
    public int playerHeartsCount;
    public EventSystemCustom eventSystem;
    private UiManager uiManager;
    private bool canmoveleft;
    private bool canmoveright;

    private void Start()
    {
        playerScore = 0;
        playerHeartsCount=3;
        uiManager = FindObjectOfType<UiManager>();
        canmoveleft = true;
        canmoveright = true;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.D) && canmoveright)
        {
            transform.position += new Vector3(moveAmount, 0, 0);
        }
        if (Input.GetKey(KeyCode.A) && canmoveleft)
        {
            transform.position += new Vector3(-moveAmount, 0, 0);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(TagNames.Food.ToString()))
        {
            // access the food object config
            FoodItemConfig conf = collision.gameObject.GetComponent<FoodInstanceController>().config;

            // increase the player's score
            playerScore += conf.score;
            uiManager.ScoreCount(conf.score);

            Debug.Log("SCORE: " + playerScore);

            // destroy the food object
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.CompareTag(TagNames.Combo.ToString()))
        {
            // polymorphism!
            // for example, the object of type "TimeFreezerComboController" which is the child of "ComboInstanceController", is put inside the "comboController" object below.
            ComboInstanceController comboController =  collision.gameObject.GetComponent<ComboInstanceController>();

            // the CONTENT of OnConsume method inside "TimeFreezerComboController" is available inside the "comboController"
            comboController.OnConsume();
            eventSystem.GetHeartsCount.Invoke();

            Debug.Log("COMBO!!! " + comboController.config.comboName);

            // destroy the combo object
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.CompareTag(TagNames.leftCube.ToString()))
        {
            Debug.Log("left waalll");
            canmoveleft = false;

        }

        if (collision.gameObject.CompareTag(TagNames.rightCube.ToString()))
        {
            canmoveright = false;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag(TagNames.leftCube.ToString()))
        {
            canmoveleft = true;
        }

        if (collision.gameObject.CompareTag(TagNames.rightCube.ToString()))
        {
            canmoveright = true;
        }
    }

    public void HeartsCounts(int amount)
    {
        playerHeartsCount += amount;
        Debug.Log(playerHeartsCount);
        if (playerHeartsCount <= 0)
        {
            eventSystem.GameOver.Invoke();
        }
    }
}

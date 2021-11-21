using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public float timeDelayBetweenEachMove;
    public float timerBoostValue;

    public float gridUnitSize = 0.641f;
    public int width;
    public int height;
    public float heightOffset;
    public float widthOffset;

    public GameObject[] blockViewTypes;
    public GameObject boardObjectsParent;

    public enum GameState
    {
        BlockIsBeingPlaced,
        NextBlockInQueue,
        BossIsPassing,
        None
    };

    public GameState state;


    private float timer;
    private BoardManager boardManager;
    private float defaultTimeDelayBetweenEachMove;

    private int lastChosenColor;

    private void Awake()
    {
        boardManager = new BoardManager(width, height);
        state = GameState.NextBlockInQueue;

        defaultTimeDelayBetweenEachMove = timeDelayBetweenEachMove;
        timer = Time.fixedTime + timeDelayBetweenEachMove;
    }

    private void FixedUpdate()
    {
        if (Time.fixedTime >= timer)
        {
            if (state == GameState.BlockIsBeingPlaced)
            {
                if (boardManager.CanKeepMovingThePreset())
                {
                    boardManager.MovePresetDownwards();
                }
                else
                {
                    // check if a full row is complete and shift all rows by one

                    // place the preset on board permentantly
                    // change state
                    state = GameState.NextBlockInQueue;
                }
            }

            timer = Time.fixedTime + timeDelayBetweenEachMove;
        }
    }

    void Update()
    {
        if (state == GameState.BossIsPassing)
        {
            // pass the boss


            // after boss has passed
            state = GameState.NextBlockInQueue;
        }

        if (state == GameState.NextBlockInQueue)
        {
            // choose the next preset
            //boardManager.ChooseNextPreset();
            boardManager.ChooseNextPreset();

            ChooseNewColor();


            // place it on top of the board


            state = GameState.BlockIsBeingPlaced;
        }

        if (state == GameState.BlockIsBeingPlaced)
        {
            if (boardManager.CanKeepMovingThePreset())
            {
                HandlePresetControlByPlayer();
            }

            boardManager.PreviewNextPresetPosition();
            //boardManager.CheckAndEraseFullRows();

            RenderBoard();
        }
    }

    private void HandlePresetControlByPlayer()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            boardManager.RotateNextPreset();

            //boardManager.PreviewNextPresetPosition();

            //RenderBoard();
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            boardManager.MovePresetToRightAndLeft(true);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            boardManager.MovePresetToRightAndLeft(false);
        }
        
        if (Input.GetKey(KeyCode.DownArrow))
        {
            timeDelayBetweenEachMove -= timerBoostValue;
        }
        else
        {
            timeDelayBetweenEachMove = defaultTimeDelayBetweenEachMove;
        }
    }
    void RenderBoard()
    {
        //boardManager.DrawBoardLogically();

        WipeBoardRender();

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (boardManager.gameBoard[i, j] != 0)
                {
                    var block = Instantiate(blockViewTypes[boardManager.gameBoard[i, j] - 1], boardObjectsParent.transform);
                    
                    block.transform.position = new Vector3(i * gridUnitSize - widthOffset, (height - j) * gridUnitSize - heightOffset, 0);
                }
            }
        }
    }
    
    private void WipeBoardRender()
    {
        for (int i = 0; i < boardObjectsParent.transform.childCount; i++)
            Destroy(boardObjectsParent.transform.GetChild(i).gameObject);
    }

    private void ChooseNewColor()
    {
        int newColor;
        do
        {
            newColor = UnityEngine.Random.Range(1, blockViewTypes.Length + 1);
        } while (lastChosenColor == newColor);

        boardManager.SetColorForCurrentPreset(newColor);
        lastChosenColor = newColor;
    }
}

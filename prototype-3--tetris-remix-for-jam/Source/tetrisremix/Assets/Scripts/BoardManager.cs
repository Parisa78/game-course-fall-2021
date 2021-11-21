using System;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager
{
    private int height;
    private int width;

    private List<TetrisPreset> presets;
    public int[,] gameBoard;
    private TetrisPreset nextPresetToPlace;

    private Vector2 nextPresetCurrentPosition;
    private Vector2 defaultInitialPosition;
    private List<Vector2> nextPresetVisitedPositions;

    public BoardManager(int width, int height)
    {
        this.height = height;
        this.width = width;
        defaultInitialPosition = new Vector2(width / 2, 0);
        nextPresetCurrentPosition = defaultInitialPosition;

        gameBoard = new int[width, height];

        nextPresetVisitedPositions = new List<Vector2>();

        _InitPresets();
    }
    private void _InitPresets()
    {
        presets = new List<TetrisPreset>();
        
        presets.Add(TetrisPresets.GetPreset_L());
        presets.Add(TetrisPresets.GetPreset_O());
        presets.Add(TetrisPresets.GetPreset_I());
        presets.Add(TetrisPresets.GetPreset_T());
        presets.Add(TetrisPresets.GetPreset_S());
    }
    
    public void DrawBoardLogically()
    {
        string board = "";
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                board += gameBoard[j, i];
            }
            board += "\n";
        }

        Debug.Log(board);
    }
    public void ChooseNextPreset(int manualIndex = -1)
    {

        int index;
        if (manualIndex == -1)
            index = UnityEngine.Random.Range(0, presets.Count);
        else
            index = manualIndex;
        
        nextPresetToPlace = presets[index];
        
        nextPresetCurrentPosition = defaultInitialPosition;
        
        nextPresetVisitedPositions.Clear();

    }
    
    public void PreviewNextPresetPosition()
    {
        int length = nextPresetToPlace.GetLength();

        int x = (int)nextPresetCurrentPosition.x;
        int y = (int)nextPresetCurrentPosition.y;

        WipePreviosulyVisitedPlacesByNextPresetLogically();

        for (int i = x; i < x + length; i++)
        {
            for (int j = y; j < y + length; j++)
            {
                if (nextPresetToPlace.rotations[nextPresetToPlace.GetCurrentRotationIndex()][i-x,j - y] != 0)
                { 
                    gameBoard[i, j] = nextPresetToPlace.colorCode;
                    nextPresetVisitedPositions.Add(new Vector2(i, j));
                }
            }
        }
    }
    
    public void MovePresetDownwards()
    {
        nextPresetCurrentPosition.y++;

        // check collision witht previoulsy placed presets
        if (nextPresetCurrentPosition.y + nextPresetToPlace.GetLength() >= height)
            nextPresetCurrentPosition.y = height - 1 - nextPresetToPlace.GetLength();
    }


    public void MovePresetToRightAndLeft(bool isRight)
    {
        Debug.Log(".");
        
        if (isRight)
        {
            if (nextPresetCurrentPosition.x + nextPresetToPlace.GetHighestBoundRight() + 1 >= width)
                return;

            nextPresetCurrentPosition.x++;
        }
        else
        {
            if (nextPresetCurrentPosition.x - 1 < 0)
                return;

            nextPresetCurrentPosition.x--;
        }
        
        Debug.Log("X => " + nextPresetCurrentPosition.x);
        Debug.Log("B => " + nextPresetToPlace.GetHighestBoundRight());
    }

    public void RotateNextPreset()
    {
        nextPresetToPlace.Rotate();
    }

    public void WipePreviosulyVisitedPlacesByNextPresetLogically()
    {
        // clean previous locations
        foreach (Vector2 pos in nextPresetVisitedPositions)
        {
            gameBoard[(int)pos.x, (int)pos.y] = 0;
        }
        nextPresetVisitedPositions.Clear();
    }

    public bool CanKeepMovingThePreset()
    {
        
        int length = nextPresetToPlace.GetLength();
        int x = (int)nextPresetCurrentPosition.x;
        int y = (int)nextPresetCurrentPosition.y;
        int[] lowerBounds = nextPresetToPlace.FindLowerBoundsOfPreset();

        if (nextPresetCurrentPosition.y + nextPresetToPlace.GetHighestBoundY(lowerBounds) + 1 >= height - 1)
            return false;

        for (int i = x; i < x + length; i++)
        {
            if (lowerBounds[i-x] != -1 && 
                gameBoard[i, y + lowerBounds[i-x]] != 0 && 
                gameBoard[i, y + lowerBounds[i-x] + 1] != 0)
                return false;
        }

        return true;
    }

    public void SetColorForCurrentPreset(int colorIndex)
    {
        nextPresetToPlace.colorCode = colorIndex;
    }

    public void CheckAndEraseFullRows()
    {
        bool[] rowBlocksAreFull = new bool[height];
        for (int j = 0; j < height; j++)
        {
            rowBlocksAreFull[j] = true;

            for (int i = 0; i < width; i++)
            {
                if (gameBoard[i, j] == 0)
                {
                    rowBlocksAreFull[j] = false;
                    break;
                }
            }
        }
        for (int j = 0; j < height; j++)
        {

        }
    }
}

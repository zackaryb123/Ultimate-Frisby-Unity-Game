using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelManager : MonoBehaviour
{
    public int columns = 9;
    public int rows = 105;
    
    public int coinCount = 20;
    public int enemyCount = 60;

    public GameObject FloorTiles; 
    public GameObject CoinTiles;
    public GameObject EnemyTiles;
    public GameObject OuterWallTiles;

    Transform boardHolder;
    List<Vector3> gridPosition = new List<Vector3>();
    List<Vector3> spawnPositionList = new List<Vector3>();

    void InitailizeList()
    {
        gridPosition.Clear();
        spawnPositionList.Clear();

        for (int x = 0; x < columns; x++)
        {
            for (int y = 1; y < rows - 2; y++)
                gridPosition.Add(new Vector3(x, y, 0f));
        }

        for (int x = 0; x < columns; x++)
            spawnPositionList.Add(new Vector3(x, rows - 2, 0f));
    }

    void BoardSetup()
    {
        boardHolder = new GameObject("Board").transform;
        boardHolder.gameObject.tag = "Board";

        for (int x = -1; x < columns + 1; x++)
        {

            for (int y = -1; y < rows + 1; y++)
            {
                GameObject toInstantiate = FloorTiles;

                if (x == -1 || x == columns  || y == -1 || y == rows)
                {
                    toInstantiate = OuterWallTiles;
                }
                GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;

                instance.transform.localScale = new Vector3(2f, 2f, 2f);

                instance.transform.SetParent(boardHolder);
            }
        }
    }

    public Vector3 RandomPosition(List<Vector3> grid)
    {
        int randomIndex = Random.Range(0, grid.Count);
        Vector3 randomPosition = grid[randomIndex];
        grid.RemoveAt(randomIndex);

        return randomPosition;
    }

    public void LayoutObjectAtRandom(GameObject tile, List<Vector3> grid, int count)
    {
        int objectCount = count;

        for (int i = 0; i < objectCount; i++)
        {
            Vector3 randomPosition = RandomPosition(grid);
            GameObject tileChoice = tile;

            Instantiate(tileChoice, randomPosition, Quaternion.identity);
        }
    }

    public void SetupScene (int level)
    {
        BoardSetup();
        InitailizeList();

        CoinTiles.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        EnemyTiles.transform.localScale = new Vector3(2, 2, 2);

        LayoutObjectAtRandom(CoinTiles, gridPosition, coinCount);
        //int enemyCount = (int)Mathf.Log(level, 2f); // Determines number of enemies basesd on level
        LayoutObjectAtRandom(EnemyTiles, gridPosition, enemyCount);
    }
}

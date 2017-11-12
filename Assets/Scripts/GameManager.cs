using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public int SpeedOfEnemy = 50;
    public int PlayerCoinPoints = 0;
    public float PlayerYardDist = 0;

    Text GameText;
    GameObject GameImage;
    Animator ScreenFaderAnim;

    public bool doingSetup;

    public GameObject canvas;
    public GameObject FrisbyTile;
    public GameObject PlayerTile;
    public GameObject TargetTile;

    public static GameManager instance = null;
    public LevelManager boardScript;

    int goals = 1;
    int speedOfEnemy = 0;
    List<Enemy> enemies;
    List<Vector3> RespawnPositionList;
    List<GameObject> PrevSpawnedEnemies;

    void Awake()
    {
        if (instance == null)
            instance = this;

        else if (instance != this)
            Destroy(gameObject);

        //DontDestroyOnLoad(gameObject);

        enemies = new List<Enemy>();
        RespawnPositionList = new List<Vector3>();
        PrevSpawnedEnemies = new List<GameObject>();
        boardScript = GetComponent<LevelManager>();

        InitGame();
    }

    private void Start()
    {
        PlayerTile = GameObject.FindGameObjectWithTag("Player");
        FrisbyTile = GameObject.FindGameObjectWithTag("Frisby");
        TargetTile = GameObject.FindGameObjectWithTag("Target");
    }

    void Update()
    {
        if (doingSetup) { return; }

        if (speedOfEnemy == SpeedOfEnemy)
        {
            StartCoroutine(ManageEnemies());
            speedOfEnemy = 0;
        }
        else
        {
            speedOfEnemy++;
        }
        CheckGameOver();
        CheckRoundWin();
        CheckGameWon();
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        goals++;
        InitGame();
    }

    void InitGame()
    {
        doingSetup = true;

        ScreenFaderAnim = GameObject.Find("Canvas").GetComponent<Animator>();
        GameImage = GameObject.Find("ScreenFader");
        GameText = GameObject.Find("GameText").GetComponent<Text>();

        GameImage.SetActive(true);
        GameText.text = "Choose a Pass";

        ScreenFaderAnim.SetTrigger("RoundWon");

        enemies.Clear();
        boardScript.SetupScene(goals);
    }

    public void HideGameImage()
    {
        GameImage.SetActive(false);
        ScreenFaderAnim.SetTrigger("ResetAnim");
        doingSetup = false;
    }

    public void CheckGameOver()
    {
        if (FrisbyLanded() && PlayerNotInPosition())
        {
            GameImage.SetActive(true);
            GameText.text = "Game Over!";
            ScreenFaderAnim.SetTrigger("GameOver");
            doingSetup = true;
        }
    }

    public void CheckRoundWin()
    {
        if (FrisbyLanded() && !PlayerNotInPosition() && !PlayerInEndzone())
        {
            GameImage.SetActive(true);
            GameText.text = "Good Catch!";
            ScreenFaderAnim.SetTrigger("RoundWon");
            doingSetup = true;
        }
    }

    public void CheckGameWon()
    {
        if (FrisbyLanded() && !PlayerNotInPosition() && PlayerInEndzone())
        {
            GameImage.SetActive(true);
            GameText.text = "Score!";
            ScreenFaderAnim.SetTrigger("GameOver");
            doingSetup = true;
        }
    }

    public bool FrisbyLanded()
    {
        Vector3 frisbyPos = FrisbyTile.transform.position;
        Vector3 targetPos = TargetTile.transform.position;

        if (frisbyPos == targetPos)
            return true;

        return false;
    }

    public bool PlayerNotInPosition()
    {
        Vector3 playerPos = PlayerTile.transform.position;
        Vector3 targetPos = TargetTile.transform.position;

        if (playerPos != targetPos)
            return true;

        return false;
    }

    public bool PlayerInEndzone()
    {
        float playerYPos = PlayerTile.transform.position.y;

        if (playerYPos > 100)
            return true;

        return false;
    }

    public void AddEenemyToList(Enemy script)
    {
        enemies.Add(script);
    }

    IEnumerator ManageEnemies()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].MoveEnemy();
            yield return new WaitForSeconds(enemies[i].Movetime);
        }

        InitializeRespawnList();

        CheckSpawnForPlayer(RespawnPositionList);
        CheckSpawnForEnemies(PrevSpawnedEnemies);

        ReSpawnEnemies();;
    }

    void InitializeRespawnList()
    {
        RespawnPositionList.Clear();

        for (int x = 0; x < boardScript.columns; x++)
        {
            RespawnPositionList.Add(new Vector3(x, boardScript.rows - 5, 0f));
        }
    }

    void ReSpawnEnemies()
    {
        PrevSpawnedEnemies.Clear();

        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i].transform.position.y == 0)
            {
                Vector3 respawnV = boardScript.RandomPosition(RespawnPositionList);
                enemies[i].gameObject.transform.position = new Vector3(respawnV.x, respawnV.y, respawnV.z);

                PrevSpawnedEnemies.Add(enemies[i].gameObject);
            }
        }
    }

    void CheckSpawnForPlayer(List<Vector3> grid)
    {
        int playerIndex = 0;
        if (grid.Contains(PlayerTile.transform.position))
        {
            playerIndex = GetIndexInGrid(grid, PlayerTile);
            grid.RemoveAt(playerIndex);
        }
    }

    void CheckSpawnForEnemies(List<GameObject> prevSpawnedEnemies)
    {
        int enemyIndexInRespawnList = 0;

        for (int i = 0; i < prevSpawnedEnemies.Count; i++)
        {
            if (prevSpawnedEnemies[i].transform.position.x == boardScript.rows - 5)
                enemyIndexInRespawnList =  GetIndexInGrid(RespawnPositionList, prevSpawnedEnemies[i].gameObject);
                RespawnPositionList.RemoveAt(enemyIndexInRespawnList);
        }
    }

    public int GetIndexInGrid(List<Vector3> grid, GameObject sprite)
    {
        for (int i = 0; i < grid.Count; i++)
        {
            if (sprite.transform.position == grid[i])
                return i;
        }
        return 0;
    }
}

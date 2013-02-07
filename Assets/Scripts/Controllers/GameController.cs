using UnityEngine;
using System;
using System.Collections;

public enum GameStates { Play, Pause, GameOver }

public class GameController : MonoBehaviour
{
    #region Variables

    public Transform enemyPrefab; 

    private static GameController instance;

    private GameStates gameState = GameStates.Play;
    private Vector3[] spawnPositions = null;
    private SpawnPool enemiesPool = null;

    #endregion

    #region Properties

    public static GameController Instance
    {
        get
        {
            if (instance == null)
                Debug.LogError("\'GameController\' instance isn't assigned");
            return instance;
        }
        private set { instance = value; }
    }

    public GameStates CurrentGameState
    {
        get { return gameState; }
        private set { gameState = value; }
    }

    #endregion

    #region Unity functions

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        spawnPositions = FindSpawnPositions();

        this.StartCoroutine(SpawnEnemy("EnemiesPool"));
    }

    private void OnDestroy()
    {
        Instance = null;
    }

    #endregion

    #region Game functions

    public void ResumeGame()
    {
        CurrentGameState = GameStates.Play;
    }

    public void StopGame()
    {
        CurrentGameState = GameStates.Pause;
    }   

    private IEnumerator GameOver()
    {
        yield return null;
    }

    private Vector3[] FindSpawnPositions()
    {
        GameObject[] positionsGO = GameObject.FindGameObjectsWithTag("spawnPosition");
        Vector3[] curPositions = null;
        if (positionsGO == null)
        {
            curPositions = new Vector3[1];
            curPositions[0] = new Vector3(0f, 1f, 0f);
            Debug.LogWarning("GameController.FindSpawnPositions() - on this level SpawnPosition aren't found. Enemies will spawned in (0,1,0)");
        }
        else
        {
            curPositions = new Vector3[positionsGO.Length];

            for (int i = 0; i < positionsGO.Length; i++)
            {
                if (positionsGO[i] == null)
                {
                    curPositions[i] = new Vector3(0f, 1f, 0f);
                    Debug.LogWarning("GameController.FindSpawnPositions(). GameObject positionsGO[" + i.ToString() + "] in null! Used value (0,1,0)");
                }
                else
                    curPositions[i] = new Vector3(positionsGO[i].transform.position.x, 1f, positionsGO[i].transform.position.z);
            }            
        }

        return curPositions;
    }

    #endregion

    #region Enemies functions

    private IEnumerator SpawnEnemy(string poolName)
    {
        Debug.Log(PoolManager.Pools[poolName].Count);
        PoolManager.Pools[poolName].Spawn(enemyPrefab, spawnPositions[0], Quaternion.identity);
        Debug.Log(PoolManager.Pools[poolName].Count);
        yield return new WaitForSeconds(0.5f);
    }

    #endregion

    #region Player's functions

    #endregion
}

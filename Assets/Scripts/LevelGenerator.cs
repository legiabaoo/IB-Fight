using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelGenerator : MonoBehaviour
{
    public static LevelGenerator Instance;

    public int currentLevelIndex = 0;
    public List<LevelData> levels = new List<LevelData>();
    public List<GameObject> enemies; // Gán enemy1, enemy2 trong Inspector
    public TMP_Text txtLevel;
    public Button btnNextLV;
    public GameManager gameManager;
    //public Transform enemyTransform;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // 🔥 Xóa bản trùng
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Giữ lại duy nhất một bản
    }
    void Start()
    {
        
        GeneratorLevels(10);
        StartLevel();
    }

    private void GeneratorLevels(int totalLevels)
    {
        for (int i = 0; i < totalLevels; i++)
        {
            LevelData level = new LevelData();
            level.levelNumber = i + 1;

            level.enemyHP = 110 + i * 20;
            level.enemyDamage = 60 + i * 20;

            levels.Add(level);
        }
    }
    public void NextLevel()
    {
        currentLevelIndex++;
        SceneManager.sceneLoaded += OnSceneLoaded; // Đăng ký sự kiện khi scene load xong
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void RestartLevel()
    {
        SceneManager.sceneLoaded += OnSceneLoaded; // Đăng ký sự kiện khi scene load xong
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // Hủy đăng ký để không bị gọi nhiều lần
        StartLevel(); // Gọi lại khi scene đã được load hoàn tất
    }
    public void StartLevel()
    {
        if (currentLevelIndex >= levels.Count)
        {
            Debug.Log("da hoan thanh tat ca cac level");
            return;
        }
        LevelData currentLevel = levels[currentLevelIndex];
        if (txtLevel == null)
        {
            txtLevel = GameObject.Find("txtLevel").GetComponent<TMP_Text>();
        }
        txtLevel.text = "Level: " + currentLevel.levelNumber;
        Debug.Log("Bắt đầu Level: " + currentLevel.levelNumber);

        SpawnEnemy(currentLevel);
    }
    //private void SpawnEnemy(LevelData data)
    //{
    //    GameObject enemy = Instantiate(enemyPrefab, enemyTransform.position, enemyTransform.rotation);
    //    EnemyHealth eH = enemy.GetComponent<EnemyHealth>();
    //    Enemy e = enemy.GetComponent<Enemy>();
    //    eH.maxHealth = data.enemyHP;
    //    e.dameEnemy = data.enemyDamage;
    //    Debug.Log("da spawn");
    //}
    private void SpawnEnemy(LevelData data)
    {
        enemies.Clear();
        if (enemies.Count == 0)
        {
            if (GameObject.Find("Enemy1") != null)
            {
                enemies.Add(GameObject.Find("Enemy1"));
            }
            if (GameObject.Find("Enemy2") != null)
            {
                enemies.Add(GameObject.Find("Enemy2"));
            }
            if (GameObject.Find("Enemy") != null)
            {
                enemies.Add(GameObject.Find("Enemy"));
            }
        }
        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].GetComponent<EnemyHealth>().maxHealth = data.enemyHP;
            enemies[i].GetComponent<Enemy>().dameEnemy = data.enemyDamage;
            enemies[i].GetComponent<EnemyHealth>().Sync();
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (gameManager == null)
        {
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        }
    }
}

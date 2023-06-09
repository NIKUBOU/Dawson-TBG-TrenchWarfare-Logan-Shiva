using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.EventSystems.EventTrigger;

public class GameManager : MonoBehaviour
{
    private int previousScore = 0;

    private float currentTimer = 0;

    private static GameManager instance;

    private Menu menu;

    private int deaths;

    private int score = 0;

    private bool gameStart = false;

    private bool gameWon = false;

    private bool gameStop = false;

    public int Deaths { get { return deaths; } set { deaths = value; } }

    public int Score { get { return score; } set { score = value; } }

    public bool GameStart { get { return gameStart; } set { gameStart = value; } }

    public bool GameWon { get { return gameWon; } set { gameWon = value; } }

    public bool GameStop { get { return gameStop; } set { gameStop = value; } }

    public float CurrentTimer
    {
        get { return currentTimer; }
    }

    public static GameManager Instance
    {
        get
        {
            return instance;
        }
    }

    private void Start()
    {
        FindMenu();
        deaths = 0;
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (gameStart)
        {
            Timer();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TriggerGameWon();
        }

        if (menu == null)
        {
            FindMenu();
        }
    }

    void Timer()
    {
        currentTimer += Time.deltaTime;
    }

    //Scene management stuff
    public void LoadNext()
    {
        previousScore = score;
        if (SceneManager.GetActiveScene().buildIndex >= SceneManager.sceneCount)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            
        }
    }

    public void ReloadScene()
    {
        score = previousScore;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoBackToMainMenu()
    {
        SceneManager.LoadScene(0);
        gameStart = false;
        score = 0;
        deaths = 0;
        currentTimer = 0;
    }

    public void TriggerGameOver()
    {
        gameStart = false;
        menu.OpenDeathUI();
        score = previousScore;
    }

    public void TriggerGameWon()
    {
        gameStart = false;
        Invoke("OpenWonUI", 2f);
    }

    private void OpenWonUI()
    {
        menu.OpenWonUI();
    }

    public void TriggerGameStop()
    {
        Time.timeScale = 0;
    }

    public void TriggerGameContinue()
    {
        Time.timeScale = 1;
    }

    private void FindMenu()
    {
        menu = FindObjectOfType<Menu>();
    }
}

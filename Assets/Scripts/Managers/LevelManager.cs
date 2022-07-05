using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : Singleton<LevelManager>
{
    private GameManager gameManager;
    [HideInInspector]public Level level;

    public GameObject Player;
    public GameObject GK;
    public GameObject Ball;

    // Gameplay
    private Player _player;
    public bool PowerUP;
    public bool isGoal;
    public bool isGameActive;
    public float shootPower;
    public float ExplosionForce;
    public float ExplosionRadius;

    //Menus
    public GameObject DeathPanel;
    public GameObject VictoryPanel;
    public GameObject InGamePanel;

    //Canvas
    public Text curreny;
    public Text levelText;
    [SerializeField] private Image Fade;
    [SerializeField] private Animator anim;


    private void Awake()
    {
        gameManager = GameManager.Instance;
        int mapInt = gameManager.currentLevel % 5;
        level = Resources.LoadAll<Level>("levels")[mapInt];
        //Debug.Log(level.name);
        _player = Player.GetComponent<Player>();
        GK.transform.Find("Trigger").GetComponent<GoalPost>().ExplosionForce = ExplosionForce;
        GK.transform.Find("Trigger").GetComponent<GoalPost>().ExplosionRadius = ExplosionRadius;
    }

    public void Start()
    {
        levelText.text = "Level " + (1 + gameManager.currentLevel);
    }

    public void StartGame()
    {
        isGameActive = true;
    }

    public void PowerUpDown(bool state)
    {
        PowerUP = state;
    }

    public void SetPlayerAndBall(Vector3 playerPos)
    {
        Player.transform.position = playerPos;
        Ball.transform.position = playerPos + Vector3.forward;
    }

    private void FixedUpdate()
    {
        if (PowerUP)
        {
            _player.shootPower = shootPower * 2;
        }
        else
        {
            _player.shootPower = shootPower;
        }

        if (!isGameActive)
        {
            if (Player.GetComponent<Animator>().GetBool("Run"))
                Player.GetComponent<Animator>().SetBool("Run", false);

            if(InGamePanel.activeSelf && (DeathPanel.activeSelf || VictoryPanel.activeSelf))
                InGamePanel.SetActive(false);
        }

        curreny.text = gameManager.currency.ToString();
    }
    public void NextLevel()
    {
        //gameManager.currentLevel += 1;
        gameManager.LevelUp();
        StartCoroutine("ReloadScene");
    }

    public void TryAgain()
    {
        StartCoroutine("ReloadScene");
    }

    IEnumerator ReloadScene()
    {
        anim.SetBool("Fade", true);
        yield return new WaitUntil(() => Fade.color.a == 1);
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }
}

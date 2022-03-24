using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public FrogController frog;
    public Text levelText;
    public Text scoreText;
    public Text gameOverText;
    public float difficultyMultiplier = 1.2f;

    private int level = 1;
    private int score = 0;
    private float highestPosition;
    private float restartTimer = 3f;
    private GameObject[] enemyRows;

    // Start is called before the first frame update
    void Start()
    {
        frog.OnFrogMoved += OnFrogMoved;
        frog.OnFrogEscaped += OnFrogEscaped;

        highestPosition = frog.transform.position.y;

        levelText.text = level.ToString();
        scoreText.text = score.ToString();
        gameOverText.gameObject.SetActive(false);
        enemyRows = GameObject.FindGameObjectsWithTag("EnemyRow");
        foreach (GameObject row in enemyRows)
            if (row.name != "EnemyRow1")
                row.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        if (frog == null)
        {
            gameOverText.gameObject.SetActive(true);
            restartTimer -= Time.deltaTime;
            if (restartTimer <= 0f)
                SceneManager.LoadScene("Main");
        }
    }

    void OnFrogMoved()
    {
        float frogPosition = frog.transform.position.y;

        if (frogPosition > highestPosition)
        {
            RaiseScore();
            highestPosition = frogPosition;
        }
    }

    void OnFrogEscaped()
    {
        RaiseScore();
        highestPosition = frog.transform.position.y;
        levelText.text = (++level).ToString();
        RaiseDifficulty();
    }

    private void RaiseDifficulty()
    {
        if (level <= 5)
        {
            foreach (GameObject row in enemyRows)
                if (row.name.Equals("EnemyRow" + level))
                {
                    row.SetActive(true);
                    return;
                }
        }
        else
        {
            foreach (EnemyController enemy in GetComponentsInChildren<EnemyController>())
                enemy.RaiseSpeed(difficultyMultiplier);
        }
    }

    void RaiseScore()
    {
        score += level;
        scoreText.text = score.ToString();
    }
}

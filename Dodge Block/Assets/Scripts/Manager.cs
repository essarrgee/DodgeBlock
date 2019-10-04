using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
	//Public Properties
	public GameObject[] spawnerList;
	public GameObject enemyPrefab;
	
	//UI
	private GameObject HUDUI;
	private GameObject GameOverUI;
	private GameObject TitleUI;
	private GameObject FadeUI;
	private GameObject startButton;
	private GameObject gameOverButton;
	private string CurrentUI;
	
	//Player
	private GameObject player;
	private Rigidbody2D playerRB;
	private int playerHealth;
	
	//Enemy Properties
	private List<GameObject> enemyList; //Generic list to store enemies
	
	private float tick;
	
	private int level;
	private float overallEnemySpeed;
	private float spawnEnemyInterval;
	private Vector2 spawnEnemyPosition;
	private Vector2 spawnEnemyForce;
	private Vector2 spawnEnemyScale;
	
    void Awake()
    {
		tick = 0;
		
		HUDUI = GameObject.Find("HUD");
		GameOverUI = GameObject.Find("GameOver");
		TitleUI = GameObject.Find("Title");
		FadeUI = GameObject.Find("Fade");
		
		startButton = TitleUI.transform.Find("Button").gameObject;
		gameOverButton = GameOverUI.transform.Find("Button").gameObject;
		
		HUDUI.SetActive(false);
		GameOverUI.SetActive(false);
		TitleUI.SetActive(true);
		FadeUI.SetActive(false);
		CurrentUI = "Title";
		
		player = GameObject.FindWithTag("Player");
		playerRB = player.GetComponent<Rigidbody2D>();
        playerHealth = 3;
		player.SetActive(false);
		playerRB.position = new Vector2(0,0);
		
		enemyList = new List<GameObject>();
		
		level = 0;
		overallEnemySpeed = 0.1f;
		spawnEnemyInterval = 3f;
		spawnEnemyPosition = new Vector2(-6.5f,0);
		spawnEnemyForce = new Vector2(0,0);
    }

    // Update is called once per frame
    void Update()
    {
        if (CurrentUI == "Title") {
			HUDUI.SetActive(false);
			GameOverUI.SetActive(false);
			TitleUI.SetActive(true);
			FadeUI.SetActive(false);
		}
		else if (CurrentUI == "Play") {
			HUDUI.SetActive(true);
			GameOverUI.SetActive(false);
			TitleUI.SetActive(false);
			FadeUI.SetActive(false);
		}
		else if (CurrentUI == "GameOver") {
			HUDUI.SetActive(true);
			GameOverUI.SetActive(true);
			TitleUI.SetActive(false);
			FadeUI.SetActive(false);
		}
    }
	
	void Start() {
		startButton.GetComponent<Button>().onClick.AddListener(PlayGame);
		gameOverButton.GetComponent<Button>().onClick.AddListener(ReturnTitle);
	}
	
	void FixedUpdate()
    {
		if (CurrentUI == "Play") {
			if (tick >= spawnEnemyInterval) {
				tick = 0;
				int spawnerNo = Random.Range(1,4);
				spawnEnemyPosition = spawnerList[spawnerNo].transform.position;
				if (spawnEnemyPosition.x > 0.5f || spawnEnemyPosition.x < -0.5f) {
					spawnEnemyPosition = spawnEnemyPosition + new Vector2(0, Random.Range(-1f,1f)*5f);
					spawnEnemyForce = (spawnEnemyPosition * new Vector2(-1f,1f)) - spawnEnemyPosition;
				} else if (spawnEnemyPosition.y > 0.5f || spawnEnemyPosition.y < -0.5f) {
					spawnEnemyPosition = spawnEnemyPosition + new Vector2(Random.Range(-1f,1f)*5.5f, 0);
					spawnEnemyForce = (spawnEnemyPosition * new Vector2(1f,-1f)) - spawnEnemyPosition;
				}
				//spawnEnemyForce = spawnEnemyForce.normalized;
				GameObject newEnemy = Instantiate(enemyPrefab, spawnEnemyPosition, Quaternion.identity);
				enemyList.Add(newEnemy);
				//newEnemy.transform.localScale = spawnEnemyScale;
				Rigidbody2D rb = newEnemy.GetComponent<Rigidbody2D>();
				BoxCollider2D bc = newEnemy.GetComponent<BoxCollider2D>();
				rb.AddForce(spawnEnemyForce*overallEnemySpeed, ForceMode2D.Impulse);
				//bc.size = newEnemy.transform.localScale;
				Destroy(newEnemy, 1/overallEnemySpeed);
			}
			else {
				tick += Time.fixedDeltaTime;
			}
		}
    }
	
	void ClearEnemies() {
		for (int i=0; i<enemyList.Count; i++) {
			if (enemyList[i] != null) {
				Destroy(enemyList[i]);
			}
			enemyList.RemoveAt(i);
		}
	}
	
	void PlayGame() {
		ClearEnemies();
		player.SetActive(true);
		playerHealth = 3;
		playerRB.position = new Vector2(0,0);
		CurrentUI = "Play";
	}
	
	public void EndGame() {
		player.SetActive(false);
		ClearEnemies();
		CurrentUI = "GameOver";
	}
	
	void ReturnTitle() {
		player.SetActive(false);
		ClearEnemies();
		CurrentUI = "Title";
	}
}

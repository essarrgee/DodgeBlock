using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Manager : MonoBehaviour
{
	//Public Properties
	public GameObject[] spawnerList;
	public GameObject enemyPrefab;
	
	//Camera
	private Camera cam;
	
	//UI
	private GameObject HUDUI;
	private GameObject GameOverUI;
	private GameObject TitleUI;
	private GameObject FadeUI;
	private TMP_Text lvlCounter;
	private TMP_Text hpCounter;
	private TMP_Text gameOverText;
	private GameObject startButton;
	private GameObject gameOverButton;
	private string CurrentUI;
	
	//Player
	private GameObject player;
	private Rigidbody2D playerRB;
	private SpriteRenderer playerSR;
	private int playerHealth;
	private float playerIFrame;
	
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
		
		cam = Camera.main;
		
		HUDUI = GameObject.Find("HUD");
		GameOverUI = GameObject.Find("GameOver");
		TitleUI = GameObject.Find("Title");
		FadeUI = GameObject.Find("Fade");
		
		lvlCounter = HUDUI.transform.Find("LevelCounter").gameObject.GetComponent<TMP_Text>();
		hpCounter = HUDUI.transform.Find("Health").gameObject.GetComponent<TMP_Text>();
		gameOverText = GameOverUI.transform.Find("Text").gameObject.GetComponent<TMP_Text>();
		
		startButton = TitleUI.transform.Find("Button").gameObject;
		gameOverButton = GameOverUI.transform.Find("Button").gameObject;
		
		HUDUI.SetActive(false);
		GameOverUI.SetActive(false);
		TitleUI.SetActive(true);
		FadeUI.SetActive(false);
		CurrentUI = "Title";
		
		player = GameObject.FindWithTag("Player");
		playerRB = player.GetComponent<Rigidbody2D>();
		playerSR = player.GetComponent<SpriteRenderer>();
        playerHealth = 3;
		playerIFrame = 3f;
		player.SetActive(false);
		playerRB.position = new Vector2(0,0);
		
		enemyList = new List<GameObject>();
		
		level = 0;
		overallEnemySpeed = 0.1f;
		spawnEnemyInterval = 3f;
		spawnEnemyPosition = new Vector2(-6.5f,0);
		spawnEnemyForce = new Vector2(0,0);
    }
	
	void Start() {
		startButton.GetComponent<Button>().onClick.AddListener(PlayGame);
		gameOverButton.GetComponent<Button>().onClick.AddListener(ReturnTitle);
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
			lvlCounter.text = "Level: " + level;
			hpCounter.text = "Health: " + playerHealth;
		}
		else if (CurrentUI == "GameOver") {
			HUDUI.SetActive(true);
			GameOverUI.SetActive(true);
			TitleUI.SetActive(false);
			FadeUI.SetActive(false);
			lvlCounter.text = "Level: " + level;
			hpCounter.text = "Health: " + playerHealth;
		}
		//I-Frame blinking
		if (player.activeSelf) {
			if (playerIFrame > 0f) {
				playerIFrame -= Time.deltaTime;
				playerSR.color = new Color(1f,0f,0f,Mathf.Round((playerIFrame*10)%2));
			} 
			else {
				playerIFrame = 0f;
				playerSR.color = new Color(1f,0f,0f,1f);
			}
		}
    }
	
	void FixedUpdate()
    {
		if (CurrentUI == "Play") {
			int amount = enemyList.Count;
			switch (amount) {
				case 10:
					level = 1;
					overallEnemySpeed = 0.1f;
					spawnEnemyInterval = 3f;
					break;
				case 20:
					level = 2;
					overallEnemySpeed = 0.2f;
					spawnEnemyInterval = 2.5f;
					break;
				case 30:
					level = 3;
					overallEnemySpeed = 0.4f;
					spawnEnemyInterval = 2f;
					break;
				case 50:
					level = 4;
					overallEnemySpeed = 0.7f;
					spawnEnemyInterval = 1.5f;
					break;
				case 80:
					level = 5;
					overallEnemySpeed = 1f;
					spawnEnemyInterval = 1f;
					break;
				default:
					break;
			}
		}
		else {
			level = 0;
			overallEnemySpeed = 0.1f;
			spawnEnemyInterval = 3f;
		}
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
			spawnEnemyScale = new Vector2(1.5f,1.5f);
			if (level > 1 && level <= 3) {
				float randomSize = Random.value;
				spawnEnemyScale += new Vector2(randomSize, randomSize);
			} 
			else if (level > 3) {
				float randomSizeX = Random.Range(2f, 2f+level);
				float randomSizeY = Random.Range(2f, 2f+level);
				spawnEnemyScale += new Vector2(randomSizeX, randomSizeY);
			}
			//spawnEnemyForce = spawnEnemyForce.normalized;
			GameObject newEnemy = Instantiate(enemyPrefab, spawnEnemyPosition, Quaternion.identity);
			enemyList.Add(newEnemy);
			Rigidbody2D rb = newEnemy.GetComponent<Rigidbody2D>();
			BoxCollider2D bc = newEnemy.GetComponent<BoxCollider2D>();
			rb.AddForce(spawnEnemyForce*overallEnemySpeed, ForceMode2D.Impulse);
			newEnemy.transform.localScale = spawnEnemyScale;
			bc.size = newEnemy.transform.localScale*2/10;
			Destroy(newEnemy, 1/overallEnemySpeed);
		}
		else {
			tick += Time.fixedDeltaTime;
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
	
	public bool HurtPlayer() {
		bool canHurt = false;
		if (playerIFrame <= 0f) {
			canHurt = true;
			playerHealth -= 1;
			if (playerHealth <= 0) {
				playerHealth = 0;
				EndGame();
			}
			playerIFrame = 3f;
			StartCoroutine("ShakeScreen");
		}
		return canHurt;
	}
	
	IEnumerator ShakeScreen() {
		for (int i=0; i<10; i++) {
			int shake = (i%2 == 0) ? 1 : -1;
			cam.transform.eulerAngles += new Vector3(0f,0f,Random.Range(1f-(i/10),3f-(i/10))*shake);
			yield return new WaitForSeconds(0.02f);
		}
		cam.transform.eulerAngles = new Vector3(0f,0f,0f);
	}
	
	void PlayGame() {
		level = 0;
		overallEnemySpeed = 0.1f;
		spawnEnemyInterval = 3f;
		ClearEnemies();
		player.SetActive(true);
		playerHealth = 3;
		playerIFrame = 3f;
		playerRB.position = new Vector2(0,0);
		CurrentUI = "Play";
	}
	
	void EndGame() {
		player.SetActive(false);
		gameOverText.text = "GAME OVER";
		gameOverText.text += "\n" + "\n" + "Level: " + level;
		gameOverText.text += "\n" + "Enemies: " + enemyList.Count;
		CurrentUI = "GameOver";
	}
	
	void ReturnTitle() {
		ClearEnemies();
		player.SetActive(false);
		CurrentUI = "Title";
	}
}

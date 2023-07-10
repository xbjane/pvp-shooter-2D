﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour 
{
	public List<Wave> waves = new List<Wave>();
	public List<GameObject> curEnemies = new List<GameObject>();
	public bool waveActive;
	public int waveCount;
	public float waveTimer;
	public float timeEachWave;
	private bool canUpgrade;
	public bool gameDone;

	public Transform[] spawnPoints;

	public Transform cam;

	//Enemy Prefabs
	public GameObject knightPrefab;
	public GameObject archerPrefab;
	public GameObject magePrefab;
	public GameObject kingPrefab;

	public static Game game;
	public UI ui;
	public Player player;

	void Awake ()
	{
		game = this;
	}

	void Start ()
	{
		StartCoroutine(StartGameTimer());
	}

	//Called at the start of the gmae to count down on screen.
	IEnumerator StartGameTimer ()
	{
		ui.startText.gameObject.active = true;
		yield return new WaitForSeconds(1);
		ui.startText.text = "BEGINS IN\n<size=150>4</size>"; ui.startText.rectTransform.localScale += new Vector3(0.15f, 0.15f, 0);
		Game.game.Shake(0.1f, 0.1f, 30.0f); Camera.main.orthographicSize -= 0.2f;
		yield return new WaitForSeconds(1);
		ui.startText.text = "BEGINS IN\n<size=150>3</size>"; ui.startText.rectTransform.localScale += new Vector3(0.15f, 0.15f, 0);
		Game.game.Shake(0.1f, 0.1f, 30.0f); Camera.main.orthographicSize -= 0.2f;
		yield return new WaitForSeconds(1);
		ui.startText.text = "BEGINS IN\n<size=150>2</size>"; ui.startText.rectTransform.localScale += new Vector3(0.15f, 0.15f, 0);
		Game.game.Shake(0.1f, 0.1f, 30.0f); Camera.main.orthographicSize -= 0.2f;
		yield return new WaitForSeconds(1);
		ui.startText.text = "BEGINS IN\n<size=150>1</size>"; ui.startText.rectTransform.localScale += new Vector3(0.15f, 0.15f, 0);
		Game.game.Shake(0.1f, 0.1f, 30.0f); Camera.main.orthographicSize -= 0.2f;
		yield return new WaitForSeconds(1);
		ui.startText.gameObject.active = false;
		Camera.main.orthographicSize = 7;

		NextWave();
	}

	void Update ()
	{
		//If the wave is currently going but there are no enemies, then end the wave.
		if(waveActive)
		{
			if(curEnemies.Count == 0)
			{
				waveActive = false;
				StartCoroutine(WaveEndTimer());
			}
		}

		//If the player can upgrade, then allow the required keyboard inputs.
		if(canUpgrade)
		{
			if(Input.GetKeyDown(KeyCode.Q))
			{
				player.damage += 5;
				canUpgrade = false;
				ui.upgradeText.gameObject.active = false;
				Game.game.Shake(0.15f, 0.15f, 30.0f);
			}
			if(Input.GetKeyDown(KeyCode.E))
			{
				player.attackRate = 0.05f;
				canUpgrade = false;
				ui.upgradeText.gameObject.active = false;
				Game.game.Shake(0.15f, 0.15f, 30.0f);
			}
		}

		//If the player presses ESCAPE, quit the game.
		if(Input.GetKeyDown(KeyCode.Escape)){
			Application.Quit();
		}
	}

	//Called when the next wave needs to be spawned.
	void NextWave ()
	{
		curEnemies.Clear();
		waveCount++;
		Wave wave = waves[waveCount - 1];
		StartCoroutine(EnemySpawnLoop(wave));
		ui.StartCoroutine("NextWaveAnim");
	}

	//Spawns enemies overtime in random positions.
	IEnumerator EnemySpawnLoop (Wave wave)
	{
		//If we're not spawning a boss, then loop through and spawn all the enemies.
		if(!wave.spawnBoss)
		{
			//Called for each enemy spawned.
			for(int x = 0; x < wave.enemies.Length; x++)
			{
				yield return new WaitForSeconds(wave.spawnRates[x]);

				GameObject enemyToSpawn = GetEnemyToSpawn(wave.enemies[x]);
				Vector3 randomOffset = new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), 0);
				GameObject enemy = Instantiate(enemyToSpawn, spawnPoints[Random.Range(0, spawnPoints.Length)].position + randomOffset, Quaternion.identity);

				enemy.GetComponent<Enemy>().target = player.gameObject;
				curEnemies.Add(enemy);
			}
		}
		//Otherwise, spawn the boss.
		else
		{
			GameObject enemy = Instantiate(kingPrefab, spawnPoints[Random.Range(0, spawnPoints.Length)].position, Quaternion.identity);
			enemy.GetComponent<King>().target = player.gameObject;
			curEnemies.Add(enemy);
		}

		waveActive = true;
	}

	//Called at the end of a wave. Has a delay between the wave ending, and the next wave starting.
	IEnumerator WaveEndTimer ()
	{
		if(!gameDone)
		{
			yield return new WaitForSeconds(2);

			//If the player needs to choose and upgrade, display that...
			if(waveCount == 4)
			{
				ui.upgradeText.gameObject.active = true;
				canUpgrade = true;
			}

			//but don't start the next round.
			while(canUpgrade)
			{
				yield return null;
			}

			NextWave();
		}
	}

	//Returns an enemy prefab based on the EnemyType enum sent.
	GameObject GetEnemyToSpawn (EnemyType e)
	{
		switch(e)
		{
			case EnemyType.Knight:
				return knightPrefab;
				break;
			case EnemyType.Archer:
				return archerPrefab;
				break;
			case EnemyType.Mage:
				return magePrefab;
				break;
		}

		return null;
	}

	//Called when the player dies.
	public void EndGame ()
	{
		StartCoroutine(EndGameTimer());
	}

	//When the player dies, delay showing the end game screen for 2 seconds.
	IEnumerator EndGameTimer ()
	{
		yield return new WaitForSeconds(2);
		ui.endGameScreen.active = true;
		Game.game.Shake(0.1f, 0.1f, 50.0f);
	}

	//Called when the player defeats the boss.
	public void WinGame ()
	{
		gameDone = true;
		player.GetComponent<CircleCollider2D>().enabled = false;
		player.rig.simulated = false;
		StartCoroutine(WinGameTimer());
	}

	//Delay when the boss is killed, before showing the win screen.
	IEnumerator WinGameTimer ()
	{
		gameDone = true;
		yield return new WaitForSeconds(2);
		ui.winScreen.active = true;
	}

	//Takes in dur, amount and intensity to create a camera shake.
	public void Shake (float duration, float amount, float intensity)
	{
		StartCoroutine(ShakeCam(duration, amount, intensity));
	}

	//Shakes the camera over time.
	IEnumerator ShakeCam (float dur, float amount, float intensity)
	{
		float t = dur;
		Vector3 originalPos = cam.position;
		Vector3 targetPos = Vector3.zero;

		while(t > 0.0f)
		{
			if(targetPos == Vector3.zero)
			{
				targetPos = Random.insideUnitCircle * amount;
				targetPos = new Vector3(targetPos.x, targetPos.y, -10);
			}

			cam.position = Vector3.Lerp(cam.position, targetPos, intensity * Time.deltaTime);

			if(Vector3.Distance(cam.position, targetPos) < 0.02f)
			{
				targetPos = Vector3.zero;
			}

			t -= Time.deltaTime;
			yield return null;
		}

		cam.position = originalPos;
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour {

	public enum SpawnType { Random, Wall, TowardsShip };
	public SpawnType type = SpawnType.Random;

	public float startDelay = 0;
	public int waveCount = 1;
	public float waveDelay = 1f;
	public int spawnCount = 1;
	public float spawnDelay = 1f;
	public float speed = 100;

	public GameObject warningSquare;
	public GameObject asteroid;

	private enum SpawnState { Idle, Spawning };
	private SpawnState state = SpawnState.Idle;

	private Transform ship;
	private Grid grid;

	void Start () {
		ship = GameObject.FindWithTag ("Player").transform;
		grid = GameObject.Find ("grid").GetComponent<Grid>();
	}

	void Update () {
		if (state == SpawnState.Idle)
			return;

		switch (type) {
		case SpawnType.Random:
			break;
		case SpawnType.Wall:
			break;
		case SpawnType.TowardsShip:
			break;
		}
	}

	void OnTriggerEnter(Collider collider)
	{	
		if ((collider.tag == "Player") && (state != SpawnState.Spawning)) {
			state = SpawnState.Spawning;
			StartCoroutine ("SpawnWaves");
		}
	}

	void OnTriggerExit(Collider collider)
	{	
		if (collider.tag == "Player") {
			state = SpawnState.Idle;
			StopAllCoroutines ();
		}
	}

	IEnumerator SpawnWaves()
	{
		yield return new WaitForSeconds(startDelay);

		while (waveCount-- > 0) {
			for (int i = 0; i < spawnCount; i++) {
				Vector3 e = GetComponent<BoxCollider> ().bounds.extents;
				Vector3 p = grid.GetGridPosition(new Vector3(transform.position.x + Random.Range(-e.x, e.x), 0, Random.Range(ship.position.z, transform.position.z + e.z)));
				Quaternion r = Quaternion.identity;
				GameObject g = (GameObject)Instantiate (warningSquare, p + 10 * Vector3.down, Quaternion.AngleAxis (90, Vector3.right));
				Destroy (g, 3);
				g = (GameObject) Instantiate(asteroid, p + 100*Vector3.up, r);
				g.transform.localScale = 8 * Vector3.one;
				g.GetComponent<Rigidbody> ().velocity = speed * Vector3.down;
				Destroy (g, 10);

				yield return new WaitForSeconds(spawnDelay);
			}
			yield return new WaitForSeconds(waveDelay);
		}
	}

}

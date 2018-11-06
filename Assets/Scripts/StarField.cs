using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarField : MonoBehaviour {

	public Transform cam;

	public GameObject wallRock;
	public int numWallRocks;
	public float wallDistance;
	public float wallWidth;
	public float wallHeight;

	List<GameObject> wallRocks;

	public GameObject star;
	public int numStars;
	public Bounds starVolume;

	List<GameObject> stars;

	void Start () {
		stars = new List<GameObject> ();
		wallRocks = new List<GameObject> ();
		starVolume.center = cam.position.z*Vector3.forward + 0.5f*starVolume.size.z*Vector3.forward;

		// initialize the starfield and walls
		for (int i = 0; i < numStars; i++) {
			GameObject g = (GameObject)Instantiate (star, getRandomStarPosition(false), Quaternion.identity);
			g.transform.parent = transform;
			g.GetComponent<Rigidbody>().angularVelocity = Random.Range (-10, 10) * Random.onUnitSphere; 
			stars.Add (g);
		}
		for (int i = 0; i < numWallRocks; i++) {
			GameObject g = (GameObject)Instantiate (wallRock, getRandomWallRockPosition(false), Quaternion.identity);
			g.transform.parent = transform;
			g.layer = LayerMask.NameToLayer ("WallRock");
			g.transform.localScale = Random.Range (1.5f, 4)*Vector3.one;
			g.GetComponent<Rigidbody>().angularVelocity = Random.Range (-10, 10) * Random.onUnitSphere; 
			wallRocks.Add (g);
		}
	}
	
	void Update () {
		starVolume.center = cam.position.z*Vector3.forward + 0.5f*starVolume.size.z*Vector3.forward;

		// move stars inside the star volume
		for (int i = 0; i < stars.Count; i++) {
			if (!starVolume.Contains(stars[i].transform.position))
				stars[i].transform.position = getRandomStarPosition(false);
		}
		// move wall rocks inside the star volume
		for (int i = 0; i < wallRocks.Count; i++) {
			if (!starVolume.Contains(wallRocks[i].transform.position))
				wallRocks[i].transform.position = getRandomWallRockPosition(true);
		}
	}

	Vector3 getRandomStarPosition(bool spawnToHorizon) {
		Vector3 s = starVolume.size;
		return (starVolume.center + (new Vector3(Random.Range (-s.x/2, s.x/2), Random.Range (-s.y/2, s.y/2), spawnToHorizon ? s.z/2 : Random.Range (-s.z/2, s.z/2))));
	}

	Vector3 getRandomWallRockPosition(bool spawnToHorizon) {
		Vector3 s = starVolume.size;
		Vector3 p;
		if ((Random.Range(0, 2) & 1) == 0)
			p = (starVolume.center + (new Vector3(Random.Range (wallDistance, wallDistance + wallWidth), Random.Range (-wallHeight, wallHeight), spawnToHorizon ? s.z/2 : Random.Range (-s.z/2, s.z/2))));
		else
			p = (starVolume.center + (new Vector3(Random.Range (-wallDistance - wallWidth, -wallDistance), Random.Range (-wallHeight, wallHeight), spawnToHorizon ? s.z/2 : Random.Range (-s.z/2, s.z/2))));
		return p;
	}


	void OnDrawGizmos() {
		Gizmos.color = Color.green;
		Gizmos.DrawLine(wallDistance*Vector3.left + (starVolume.center.z - starVolume.size.z/2)*Vector3.forward, wallDistance*Vector3.left + (starVolume.center.z + starVolume.size.z/2)*Vector3.forward);
		Gizmos.DrawLine(wallDistance*Vector3.right + (starVolume.center.z - starVolume.size.z/2)*Vector3.forward, wallDistance*Vector3.right + (starVolume.center.z + starVolume.size.z/2)*Vector3.forward);
	}

}


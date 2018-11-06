using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	public enum EnemyType { Asteroid, Meteorite1, Meteorite2, IceAsteroid, Fuel };
	public enum EnemySpawnType { Random, Wall };

	public static Dictionary<EnemyType, string> enemyToString = new Dictionary<EnemyType, string> {
		{ EnemyType.Asteroid, 		"asteroid" },
		{ EnemyType.IceAsteroid, 	"iceasteroid" },
		{ EnemyType.Meteorite1, 	"meteorite1" },
		{ EnemyType.Meteorite2, 	"meteorite2" },
		{ EnemyType.Fuel, 			"fuel" }
	};

}

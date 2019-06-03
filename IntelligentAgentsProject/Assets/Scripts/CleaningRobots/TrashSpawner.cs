using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashSpawner : MonoBehaviour {

	public GameObject trashPrefab;
	public float secondsBetweenSpawns;
	public int trashPerSpawn;

	float lastSpawn = Mathf.NegativeInfinity;

	public float spawnRange;

	public bool spawnEnabled = false;

	void Update() {
		if (spawnEnabled && lastSpawn + secondsBetweenSpawns < Time.time) {
			lastSpawn = Time.time;
			for (int i = 0; i < trashPerSpawn; i++) {
				float x = Random.Range(-spawnRange, spawnRange);
				float z = Random.Range(-spawnRange, spawnRange);
				GameObject spawnedTrash = Instantiate(trashPrefab, new Vector3(x, 0, z), Quaternion.identity, transform);
				spawnedTrash.name = "Trash";
			}
		}
	}
}

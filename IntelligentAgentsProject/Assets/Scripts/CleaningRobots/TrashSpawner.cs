using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashSpawner : MonoBehaviour {

	public GameObject trashPrefab;
	public float secondsBetweenSpawns;
	public int trashPerSpawn;

	float lastSpawn = Mathf.NegativeInfinity;

	public Vector2 xMinMax = new Vector2(-1f, 1f);
	public Vector2 zMinMax = new Vector2(-1f, 1f);

	void Update() {
		if (lastSpawn + secondsBetweenSpawns < Time.time) {
			lastSpawn = Time.time;
			for (int i = 0; i < trashPerSpawn; i++) {
				float x = Random.Range(xMinMax.x, xMinMax.y);
				float z = Random.Range(zMinMax.x, zMinMax.y);
				GameObject spawnedTrash = Instantiate(trashPrefab, new Vector3(x, 0, z), Quaternion.identity, transform);
				spawnedTrash.name = "Trash";
			}
		}
	}
}

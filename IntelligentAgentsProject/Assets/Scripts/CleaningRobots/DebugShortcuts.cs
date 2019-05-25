using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugShortcuts : MonoBehaviour {

	[SerializeField]
	TrashSpawner spawner;

    void Update() {
		if (Input.GetKeyDown(KeyCode.Q)) { // Start trash spawning
			spawner.spawnEnabled = !spawner.spawnEnabled;
		} else if (Input.GetKeyDown(KeyCode.W)) { // Reload scene
			Scene scene = SceneManager.GetActiveScene();
			SceneManager.LoadScene(scene.name);
		} else if (Input.GetKeyDown(KeyCode.Escape)) { // Exit program
			Application.Quit();
		}

	}
}

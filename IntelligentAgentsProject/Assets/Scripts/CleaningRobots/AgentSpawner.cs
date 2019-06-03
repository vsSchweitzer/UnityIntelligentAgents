using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentSpawner : MonoBehaviour
{
	[SerializeField]
	string baseName;

	[SerializeField]
	GameObject agentPrefab;

	[SerializeField]
	Transform trashCan;

	[SerializeField]
	int agentCount;

	[SerializeField]
	float spawnRange;

	void Start() {
		for (int i = 0; i < agentCount; i++) {
			float x = Random.Range(-spawnRange, spawnRange);
			float z = Random.Range(-spawnRange, spawnRange);
			GameObject spawnedTrash = Instantiate(agentPrefab, new Vector3(x, 0, z), Quaternion.identity, transform);
			spawnedTrash.name = "Cleaner";
			CleaningRobotAgent agent = spawnedTrash.GetComponent<CleaningRobotAgent>();
			agent.SetTrashCan(trashCan);
			agent.SetIdentifier(baseName + (i+1));
		}
	}
}

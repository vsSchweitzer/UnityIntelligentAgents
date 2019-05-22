using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleaningRobotAgent : IntelligentAgent {

	private static readonly string agentTrashPercept = "Trash";
	private static readonly string agentCarryingPercept = "CarryingTrash";

	public float moveSpeed = 5f;
	public float moveStopDistance = 1f;

	public float scanDuration = 2f;
	public float scanRadius= 10f;

	public float rotateSpeed = 250f;

	public float snapThreshold = 0.01f;

	public float pickupDistance = 1.2f;
	public float pickupCheckRadius = 1.2f;

	public float pickupDuration = 1f;

	GameObject heldTrash;

	public GameObject scanPrefab;

	public Transform trashcan;

	AgentModelController myModel;

	void Start() {
		myModel = GetComponentInChildren<AgentModelController>();
	}

	public IEnumerator TurnTowards(Vector3 targetPosition) {
		Vector3 targetDirection = targetPosition - transform.position;
		float angleToTarget = Vector3.SignedAngle(transform.forward, targetDirection, Vector3.up);
		while (angleToTarget > snapThreshold) {
			transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime, Space.Self);
			yield return null;
			angleToTarget = Vector3.SignedAngle(transform.forward, targetDirection, Vector3.up);
		}
		transform.Rotate(Vector3.up, angleToTarget, Space.Self);
	}

	[AgentAction]
	public IEnumerator ScanSurroundings() {
		List<Percept> trashPercepts = new List<Percept>();

		GameObject instatiatedEffect = Instantiate(scanPrefab, transform.position, Quaternion.identity, transform);
		yield return instatiatedEffect.GetComponent<ScannerField>().ApplyEffect(scanDuration, scanRadius);
		Destroy(instatiatedEffect);

		Collider[] trashFound = Physics.OverlapSphere(transform.position, scanRadius, LayerMask.GetMask("Trash"));
		foreach (Collider trashCollider in trashFound) {
			string x = trashCollider.transform.position.x.ToString();
			string z = trashCollider.transform.position.z.ToString();
			Percept trashPercept = new Percept(agentTrashPercept, new List<string> { x, z });

			trashPercepts.Add(trashPercept);
		}

		yield return trashPercepts;
	}

	[AgentAction]
	public IEnumerator MoveTo(float x, float z) {
		Vector3 destination = new Vector3(x, 0, z);
		yield return TurnTowards(destination);

		while (Vector3.Distance(transform.position, destination) > moveStopDistance) {
			transform.position = Vector3.MoveTowards(transform.position, destination, moveSpeed * Time.deltaTime);
		}
	}

	[AgentAction]
	public IEnumerator PickupTrashAt(float x, float z) {
		Vector3 trashPosition = new Vector3(x, 0, z);
		if (Vector3.Distance(transform.position, trashPosition) <= pickupDistance) {
			Collider[] trashFound = Physics.OverlapSphere(trashPosition, pickupCheckRadius, LayerMask.GetMask("Trash"));
			if (trashFound.Length > 0) {
				GameObject trash;
				float closestTrashDistance = Mathf.Infinity;
				trash = trashFound[0].gameObject;
				foreach (Collider trashCollider in trashFound) {
					if (Vector3.Distance(trashPosition, trashCollider.transform.position) <= closestTrashDistance) {
						trash = trashCollider.gameObject;
					}
				}
				myModel.PickupEvent += () => {
					Debug.Log(trash);
					myModel.SetToHand(trash);
					heldTrash = trash;
				};
				yield return myModel.AnimatePickup(pickupDuration);
				yield return new List<Percept>() {
					new Percept(agentTrashPercept, new List<string> { x.ToString(), z.ToString() }, PerceptAction.REMOVE),
					new Percept(agentCarryingPercept)
				};
			} else {
				Debug.Log("There was no trash in that position");
				yield return new List<Percept>() {
					new Percept(agentTrashPercept, new List<string> { x.ToString(), z.ToString() }, PerceptAction.REMOVE)
				};
			}
		} else {
			Debug.Log("Can't pickup trash, too far away");
		}
	}

	[AgentAction]
	public IEnumerator DisposeTrash() {
		if (Vector3.Distance(transform.position, trashcan.position) <= pickupDistance
			&& heldTrash != null) {
			Destroy(heldTrash);
			yield return new List<Percept> {
				new Percept(agentCarryingPercept, PerceptAction.REMOVE)
			};
		} else {
			Debug.Log("Trash can too far away");
		}
	}

}

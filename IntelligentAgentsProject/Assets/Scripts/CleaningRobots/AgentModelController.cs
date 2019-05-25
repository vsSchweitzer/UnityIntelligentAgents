using System;
using System.Collections;
using UnityEngine;

public class AgentModelController : MonoBehaviour {

	public Transform armsPivot;
	public Transform rArmPivot;
	public Transform lArmPivot;
	public Transform heldItemPivot;

	public float startFrontAngle = 0f;
	public float finalFrontAngle = 15f;

	public float normalSideAngle = 0f;
	public float openSideAngle = 15f;

	public event Action PickupEvent;

	Vector3 originalRotationArms;
	Vector3 originalRotationLArm;
	Vector3 originalRotationRArm;

	void Start() {
		originalRotationArms = armsPivot.localEulerAngles;
		originalRotationLArm = lArmPivot.localEulerAngles;
		originalRotationRArm = rArmPivot.localEulerAngles;
	}

	public IEnumerator AnimatePickup(float animationDuration) {
		float downFraction = 0.6f; // What fraction of the duration is dedicated to making the down animation;

		float frontSpeedDown = Mathf.Abs(normalSideAngle - finalFrontAngle) / (animationDuration * downFraction);
		float frontSpeedUp = -(Mathf.Abs(normalSideAngle - finalFrontAngle) / (animationDuration * (1- downFraction) ));
		float sideSpeed = Mathf.Abs(normalSideAngle - openSideAngle) / (animationDuration * downFraction);

		float startTime = Time.time;
		float finishTime = startTime + animationDuration;
		float elapsedTime = 0f;
		bool eventTriggered = false;
		while (Time.time < finishTime) {
			if (elapsedTime < animationDuration * downFraction) {
				armsPivot.Rotate(Vector3.right, frontSpeedDown * Time.deltaTime, Space.Self);
			} else {
				if (!eventTriggered) {
					PickupEvent?.Invoke();
					eventTriggered = true;
				}
				armsPivot.Rotate(Vector3.right, frontSpeedUp * Time.deltaTime, Space.Self);
			}

			if (elapsedTime < animationDuration * downFraction / 2) {
				rArmPivot.Rotate(Vector3.up, sideSpeed * Time.deltaTime, Space.Self);
				lArmPivot.Rotate(Vector3.up, -sideSpeed * Time.deltaTime, Space.Self);
			} else if (elapsedTime >= animationDuration * downFraction / 2
						&& elapsedTime < animationDuration * downFraction) {
				rArmPivot.Rotate(Vector3.up, -sideSpeed * Time.deltaTime, Space.Self);
				lArmPivot.Rotate(Vector3.up, sideSpeed * Time.deltaTime, Space.Self);
			}

			yield return null;
			elapsedTime = Time.time - startTime;
		}
		returnArmsPosition();
		yield return null;
	}

	private void returnArmsPosition() {
		armsPivot.localEulerAngles = originalRotationArms;
		lArmPivot.localEulerAngles = originalRotationLArm;
		rArmPivot.localEulerAngles = originalRotationRArm;
	}

	public void SetToHand(GameObject trash) {
		trash.transform.SetParent(heldItemPivot, true);
	}
}

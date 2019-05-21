using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScannerField : MonoBehaviour {

	float diameter = 0f;
	public float initialAlpha = 1f;
	public float finalAlpha = 0f;

	public IEnumerator ApplyEffect(float duration, float radius) {
		MeshRenderer myRenderer = GetComponent<MeshRenderer>();
		float startTime = Time.time;
		float finalDiamater = 2 * radius;
		float speed = finalDiamater / duration;
		Color originalColor = myRenderer.material.color;
		while (diameter <= finalDiamater) {
			float elapsedTime = Time.time - startTime;
			float alphaToApply = Mathf.Lerp(initialAlpha, finalAlpha, elapsedTime / duration);
			myRenderer.material.color = new Color(originalColor.r, originalColor.g, originalColor.b, alphaToApply);
			diameter += speed * Time.deltaTime;
			transform.localScale = Vector3.one * diameter;
			yield return null;
		}
	}
}

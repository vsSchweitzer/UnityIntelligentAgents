using System.Collections;
using UnityEngine;

// Code by Ted-Bigham
// from: https://answers.unity.com/questions/24640/how-do-i-return-a-value-from-a-coroutine.html
public class CoroutineWithData<T> {
	public Coroutine coroutine { get; private set; }
	public T result;
	private IEnumerator target;
	public CoroutineWithData(MonoBehaviour owner, IEnumerator target) {
		this.target = target;
		this.coroutine = owner.StartCoroutine(Run());
	}

	private IEnumerator Run() {
		while (target.MoveNext()) {
			result = (T) target.Current;
			yield return result;
		}
	}
}
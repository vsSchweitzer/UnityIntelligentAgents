using System.Collections;
using UnityEngine;

// Code by Ted-Bigham
// from: https://answers.unity.com/questions/24640/how-do-i-return-a-value-from-a-coroutine.html
// last accessed in: 2019-05-19
public class CoroutineWithData<T> {

	public Coroutine coroutine { get; private set; }
	private object result;
	private IEnumerator target;

	public CoroutineWithData(MonoBehaviour owner, IEnumerator target) {
		this.target = target;
		coroutine = owner.StartCoroutine(Run());
	}

	private IEnumerator Run() {
		while (target.MoveNext()) {
			result = target.Current;
			yield return result;
		}
	}

	public T GetResult() {
		return (T)result;
	}
}

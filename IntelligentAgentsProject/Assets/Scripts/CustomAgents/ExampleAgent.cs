using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleAgent : IntelligentAgent {

	private List<Percept> TestPercepts() {
		return new List<Percept> {
			new Percept("Percept1"),
			new Percept("Percept2",
				new List<string> { "v1", "v2" }
			)
		};
	}

	[AgentAction]
	public void VoidPublic() {
		Debug.Log("VoidPublic");
	}

	[AgentAction]
	private void VoidPrivate() {
		Debug.Log("VoidPrivate");
	}

	[AgentAction]
	public List<Percept> List() {
		Debug.Log("List");
		return TestPercepts();
	}

	[AgentAction]
	public IEnumerator TenSecondsWait() {
		Debug.Log("TenSecondsWait: Begin");
		yield return new WaitForSeconds(10f);
		Debug.Log("TenSecondsWait: End");
		yield return TestPercepts();
	}

	[AgentAction]
	public IEnumerator MultipleTenSecondsWait() {
		int i = 0;
		Debug.Log("MultipleTenSecondsWait: Begin");
		Debug.Log("MultipleTenSecondsWait: " + i++);
		yield return new WaitForSeconds(10f);
		Debug.Log("MultipleTenSecondsWait: " + i++);
		yield return new WaitForSeconds(10f);
		Debug.Log("MultipleTenSecondsWait: " + i++);
		yield return new WaitForSeconds(10f);
		Debug.Log("MultipleTenSecondsWait: " + i++);
		yield return new WaitForSeconds(10f);
		Debug.Log("MultipleTenSecondsWait: " + i++);
		Debug.Log("MultipleTenSecondsWait: End");
		yield return TestPercepts();
	}

	[AgentAction]
	public IEnumerator Parameterized(string param1, string param2, float secondsToWait) {
		Debug.Log("Parameterized: v1=" + param1 + ", v2=" + param2);
		//yield return new WaitForSeconds(float.Parse(secondsToWait));
		yield return new WaitForSeconds(secondsToWait);
		yield return TestPercepts();
	}

}

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
		Debug.Log("voidPublic");
	}

	[AgentAction]
	private void VoidPrivate() {
		Debug.Log("voidPrivate");
	}

	[AgentAction]
	public List<Percept> List() {
		Debug.Log("listPublic");
		return TestPercepts();
	}

	[AgentAction]
	public IEnumerable TenSecondsWait() {
		Debug.Log("tenSecondsWait: Begin");
		yield return new WaitForSeconds(10f);
		Debug.Log("tenSecondsWait: End");
		yield return TestPercepts();
	}

	[AgentAction]
	public IEnumerable MultipleTenSecondsWait() {
		int i = 0;
		Debug.Log("multipleTenSecondsWait: Begin");
		Debug.Log("multipleTenSecondsWait: " + i++);
		yield return new WaitForSeconds(10f);
		Debug.Log("multipleTenSecondsWait: " + i++);
		yield return new WaitForSeconds(10f);
		Debug.Log("multipleTenSecondsWait: " + i++);
		yield return new WaitForSeconds(10f);
		Debug.Log("multipleTenSecondsWait: " + i++);
		yield return new WaitForSeconds(10f);
		Debug.Log("multipleTenSecondsWait: " + i++);
		Debug.Log("multipleTenSecondsWait: End");
		yield return TestPercepts();
	}

}

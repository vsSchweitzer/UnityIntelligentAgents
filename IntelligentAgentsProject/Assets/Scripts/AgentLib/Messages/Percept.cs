using System;
using System.Collections.Generic;

[System.Serializable]
public class Percept {

	public string percept;
	public List<string> perceptValues;
	public string action;

	public Percept(string percept) {
		this.percept = percept;
		perceptValues = new List<string>();
		SetAction(PerceptAction.ADD);
	}

	public Percept(string percept, PerceptAction action) {
		this.percept = percept;
		perceptValues = new List<string>();
		SetAction(action);
	}

	public Percept(string percept, List<string> perceptValues) {
		this.percept = percept;
		this.perceptValues = perceptValues;
		SetAction(PerceptAction.ADD);
	}

	public Percept(string percept, List<string> perceptValues, PerceptAction action) {
		this.percept = percept;
		this.perceptValues = perceptValues;
		SetAction(action);
	}

	public PerceptAction GetActionEnum() {
		return (PerceptAction)Enum.Parse(typeof(PerceptAction), action);
	}

	public void SetAction(PerceptAction action) {
		this.action = action.ToString().ToUpper();
	}

}

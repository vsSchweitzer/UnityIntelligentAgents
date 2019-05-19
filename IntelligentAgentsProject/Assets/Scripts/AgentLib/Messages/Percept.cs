﻿using System.Collections.Generic;

[System.Serializable]
public class Percept {

	public Percept(string percept) {
		this.percept = percept;
		this.perceptValues = new List<string>();
	}

	public Percept(string percept, List<string> perceptValues) {
		this.percept = percept;
		this.perceptValues = perceptValues;
	}

	public string percept;
	public List<string> perceptValues;

}

using UnityEngine;
using System.Collections;

[NodeDescription("Boolean", 1, 2, NodeDescription.Type.BOOLEAN)]
public class BooleanNode : MarrowNode {
	void OnEnable() {
		outputLabels = new string[]{"True", "False"};
	}
	
	override public void OnInput(int input) {
		if (IsTrue()) {
			SendOutput(0);
		}
		else {
			SendOutput(1);
		}
	}
	
	override public bool IsTrue() {
		if (GetInput(0).IsTrue())
			return true;
		else
			return false;
	}
}

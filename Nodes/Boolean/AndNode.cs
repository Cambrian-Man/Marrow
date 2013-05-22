using UnityEngine;
using System.Collections;

[NodeDescription("And", 2, 2, NodeDescription.Type.BOOLEAN)]
public class AndNode : BooleanNode {
	void OnEnable () {
		inputLabels = new string[] {"Input 1", "Input 2"};
		outputLabels = new string[] {"True", "False"};
	}
	
	override public bool IsTrue() {
		if (GetInput(0).IsTrue() && GetInput(1).IsTrue())
			return true;
		else
			return false;
	}
}

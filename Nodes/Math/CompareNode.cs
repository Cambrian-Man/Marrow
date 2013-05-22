using UnityEngine;
using System.Collections;

[NodeDescription("Compare A to B", 3, 2, NodeDescription.Type.MATH)]
public class CompareNode : MarrowNode {
	public enum Operations {
		GREATERTHAN,
		GREATERTHANOREQUALTO,
		EQUALS,
		LESSTHAN,
		LESSTHANOREQUALTO
	}
	
	[MarrowProperty]
	public Operations operation;
	
	void OnEnable () {
		if ((GetInput(0) != null && !(GetInput(1) is IValueNode)) ||
			(GetInput(1) != null && !(GetInput(2) is IValueNode))) {
			Debug.Log("Node is not a value node.");
		}
	}
	
	override public string[] InputLabels {
		get {
			return new string[]{"Input", "A", "B"};
		}
	}
	
	override public string[] OutputLabels {
		get {
			return new string[]{"True", "False"};
		}
	}
	
	public override void OnInput (int input) {
		if (IsTrue())
			SendOutput(0);
		else
			SendOutput(1);
	}
	
	override public bool IsTrue() {
		float a = ((IValueNode) GetInput(1)).GetFloat();
		float b = ((IValueNode) GetInput(2)).GetFloat();
		
		switch (operation) {
			case Operations.GREATERTHAN:
				return a > b;
			case Operations.GREATERTHANOREQUALTO:
				return a >= b;
			case Operations.EQUALS:
				return a == b;
			case Operations.LESSTHAN:
				return a < b;
			case Operations.LESSTHANOREQUALTO:
				return a <= b;
			default:
				return false;
		}
	}
}

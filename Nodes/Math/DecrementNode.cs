using UnityEngine;
using System.Collections;

[NodeDescription("Decrement", 3, 1, NodeDescription.Type.MATH)]
public class DecrementNode : IncrementNode {	
	public override void OnInput (int input) {
		if (!valuesSet)
			SetValues();
		
		if (value > end) {
			value--;
		}
		else if (value == end && repeat) {
			value = start;
		}
		
		base.OnInput (0);
	}
}

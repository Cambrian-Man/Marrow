using UnityEngine;
using System.Collections;

[NodeDescription("Cycle", 1, 2, NodeDescription.Type.CONTROL)]
public class CycleNode : SplitNode {
	private int output;
	
	[MarrowProperty]
	public bool repeat = true;
	
	void OnEnable() {
		output = 0;
	}
	
	override public void OnInput(int input) {
		SendOutput(output);
		
		if (output < GetOutputSize() - 1) {
			output++;
		}
		else {
			if (repeat)
				output = 0;
		}
	}
}

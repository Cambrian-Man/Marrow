using UnityEngine;
using System.Collections;

[NodeDescription("Basic Output", 1, 0)]
public abstract class OutputNode : MarrowNode {	
	override public void OnInput(int input) {
		SendOutput(0);
	}
	
	public override void SendOutput (int output)
	{
		Output();
	}
	
	protected virtual void Output () {

	}
}

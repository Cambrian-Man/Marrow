using UnityEngine;
using System.Collections;

[NodeDescription("Basic Input", 0, 1)]
public abstract class InputNode : MarrowNode, IActiveNode {
	private bool interacted = false;
	
	public void OnInteract () {
		interacted = true;
		SendOutput(0);
	}
	
	public void Update () {
		interacted = false;
	}
	
	override public bool IsTrue() {
		return interacted;
	}
}

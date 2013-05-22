using UnityEngine;
using System.Collections;

[NodeDescription("Location", 0, 1, NodeDescription.Type.INPUT)]
public class LocationNode : MarrowNode, IActiveNode {
	[MarrowProperty]
	public bool isTrigger;
	
	[MarrowProperty]
	public Vector3 location;
	
	[MarrowProperty]
	public float proximity = 10;
	
	protected bool triggered;
	
	public void Update() {
		if (isTrigger) {
			if (IsTrue() && !triggered) {
				SendOutput(0);
				triggered = true;
			}
			else if (triggered && !IsTrue()) {
				triggered = false;
			}
		}
	}
	
	override public bool IsTrue() {
		return (Vector3.Distance(gameObject.transform.position, location) < proximity);
	}
}

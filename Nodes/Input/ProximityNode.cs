using UnityEngine;
using System.Collections;

[NodeDescription("Proximity", 0, 1, NodeDescription.Type.INPUT)]
public class ProximityNode : MarrowNode, IActiveNode {
	[MarrowProperty]
	public bool isTrigger;
	
	[MarrowProperty]
	public GameObject target;
	
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
		return (Vector3.Distance(gameObject.transform.position, target.transform.position) < proximity);
	}
}


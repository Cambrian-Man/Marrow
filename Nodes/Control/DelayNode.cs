using UnityEngine;
using System.Collections;

[NodeDescription("Delay", 2, 1, NodeDescription.Type.CONTROL)]
public class DelayNode : MarrowNode, IActiveNode {
	[MarrowProperty]
	public float delay = 0;
	
	private float timer;
	
	public bool IsRunning;
	
	void OnEnable () {
		inputLabels = new string[]{"Input", "Delay"};
	}
	
	public override void OnInput (int input) {
		if (delay == 0) {
			GeneratorNode node = GetInput(1) as GeneratorNode;
			if (node != null) {
				timer = node.GetFloat();
				IsRunning = true;
			}
		}
		else {
			timer = delay;
			IsRunning = true;
		}
	}
	
	/// <summary>
	/// Determines whether this instance is true.
	/// </summary>
	/// <returns>
	/// <c>true</c> if the timer is currently done; otherwise, <c>false</c>.
	/// </returns>
	public override bool IsTrue () {
		return !IsRunning;
	}
	
	public void Update () {
		if (IsRunning) {
			timer -= Time.deltaTime;
			if (timer <= 0) {
				IsRunning = false;
				SendOutput(0);
			}
		}
	}
}

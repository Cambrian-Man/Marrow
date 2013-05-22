using UnityEngine;
using System.Collections;

[NodeDescription("Random", 0, 1, NodeDescription.Type.GENERATOR)]
public class RandomNode : GeneratorNode {
	[MarrowProperty]
	public float minimum;
	
	[MarrowProperty]
	public float maximum;
	
	public override float GetFloat () {
		return Random.Range(minimum, maximum);
	}
}

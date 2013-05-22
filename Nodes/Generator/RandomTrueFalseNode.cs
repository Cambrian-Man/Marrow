using UnityEngine;
using System.Collections;

[NodeDescription("Random T/F", 0, 1, NodeDescription.Type.GENERATOR)]
public class RandomTrueFalseNode : MarrowNode {
	[MarrowProperty]
	public float chanceTrue;
	
	public override bool IsTrue ()	{
		return (Random.value < chanceTrue);
	}
}

using UnityEngine;
using System.Collections;

[NodeDescription("Arithmetic", 2, 1, NodeDescription.Type.MATH)]
public class ArithmeticNode : MarrowNode, IValueNode, IStringNode {
	public enum Operations {
		ADDITION,
		SUBTRACTION,
		DIVISION,
		MULTIPLICATION		
	}
	
	public enum RoundType {
		FLOAT,
		ROUND,
		CEILING,
		FLOOR
	}
	
	[MarrowProperty]
	public ArithmeticNode.Operations operation;
	
	[MarrowProperty]
	public ArithmeticNode.RoundType rounding = RoundType.FLOAT; 
	
	override public string[] InputLabels {
		get {
			return new string[]{"Input 1", "Input 2"};
		}
	}
	
	private float value {
		get {
			if (!(GetInput(0) is IValueNode) || !(GetInput(1) is IValueNode)) {
				throw new UnityException("Input nodes are not value nodes");
			}
			else {
				switch (operation) {
					case Operations.ADDITION:
						return (((IValueNode) GetInput(0)).GetFloat() + ((IValueNode) GetInput(1)).GetFloat());
					case Operations.SUBTRACTION:
						return (((IValueNode) GetInput(0)).GetFloat() - ((IValueNode) GetInput(1)).GetFloat());
					case Operations.MULTIPLICATION:
						return (((IValueNode) GetInput(0)).GetFloat() * ((IValueNode) GetInput(1)).GetFloat());
					case Operations.DIVISION:
						return (((IValueNode) GetInput(0)).GetFloat() / ((IValueNode) GetInput(1)).GetFloat());
					default:
						return 0;
				}
			}
		}
	}
	
	public float GetFloat() {
		switch (rounding) {
			case RoundType.CEILING:
				return Mathf.Ceil(value);
			case RoundType.FLOOR:
				return Mathf.Floor(value);
			case RoundType.ROUND:
				return Mathf.Round(value);
			default:
				return value;
		}
	}
	
	public int GetInt() {
		return Mathf.RoundToInt(GetFloat());
	}
	
	public string GetString() {
		return GetFloat().ToString();
	}
}

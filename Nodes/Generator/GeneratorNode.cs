using UnityEngine;
using System.Collections;

public abstract class GeneratorNode : MarrowNode, IValueNode {
	virtual public float GetFloat() {
		return 0f;
	}
	
	virtual public int GetInt() {
		return Mathf.RoundToInt(GetFloat());
	}
	
	virtual public string GetString() {
		return GetFloat().ToString();
	}
}

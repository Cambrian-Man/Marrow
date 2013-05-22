using UnityEngine;
using System.Collections;

public interface IValueNode : IStringNode {
	float GetFloat();
	
	int GetInt();
}

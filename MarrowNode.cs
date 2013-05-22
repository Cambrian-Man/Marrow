using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Base Marrow node.
///	Inherit from this class (or more likely one of its subclasses
/// to create custom nodes.
/// </summary>
/// <exception cref='UnityException'>
/// Is thrown when the unity exception.
/// </exception>
[NodeDescription("Base Marrow Node", 1, 1)]
public abstract class MarrowNode : ScriptableObject {
	[SerializeField]
	private MarrowNode[] inputs;
	
	[SerializeField]
	private MarrowNode[] outputs;
	
	[SerializeField]
	private int[] outputConnections;
	
	[SerializeField]
	private GameObject thisObject;

	protected string[] inputLabels;
	protected string[] outputLabels;
	
	public Rect editorPosition;
	
	/// <summary>
	/// Raises the create event.
	/// </summary>
	public virtual void OnCreate() {
	}

	/// <summary>
	/// Sets the number of inputs.
	/// </summary>
	/// <param name='size'>
	/// Size.
	/// </param>
	public virtual void SetInputSize(int size) {
		if (inputs == null) {
			inputs = new MarrowNode[size];
		}
		else {
			MarrowNode[] newInputs = new MarrowNode[size];
			if (size > inputs.Length)
				inputs.CopyTo(newInputs, 0);
			else
				System.Array.Copy(inputs, 0, newInputs, 0, size);
			
			inputs = newInputs;
		}
	}
	
	/// <summary>
	/// Sets the number of outputs.
	/// </summary>
	/// <param name='size'>
	/// Size.
	/// </param>
	public virtual void SetOutputSize(int size) {
		if (outputs == null) {
			outputs = new MarrowNode[size];
			outputConnections = new int[size];
		}
		else {			
			MarrowNode[] newOutputs = new MarrowNode[size];
			if (size > outputs.Length)
				outputs.CopyTo(newOutputs, 0);
			else
				System.Array.Copy(outputs, 0, newOutputs, 0, size);
			outputs = newOutputs;
			
			int[] newConnections = new int[size];
			if (size > outputConnections.Length)
				outputConnections.CopyTo(newConnections, 0);
			else
				System.Array.Copy(outputConnections, 0, newConnections, 0, size);
			
			outputConnections = newConnections;
		}
	}
	
	/// <summary>
	/// Gets the output labels. Override to provide generated labels.
	/// </summary>
	/// <value>
	/// The output labels.
	/// </value>
	public virtual string[] OutputLabels {
		get {
			if (outputLabels == null || outputLabels.Length != GetOutputSize()) {
				string[] labels = new string[GetOutputSize()];
				for (int i = 0; i < GetOutputSize(); i++) {
					labels[i] = "Output " + i;
				}
				
				outputLabels = labels;
			}
			return outputLabels;
		}
	}
	
	/// <summary>
	/// Gets the input labels. Override to provide generated labels.
	/// </summary>
	/// <value>
	/// The input labels.
	/// </value>
	public virtual string[] InputLabels {
		get {
			if (inputLabels == null || inputLabels.Length != GetInputSize()) {
				string[] labels = new string[GetInputSize()];
				for (int i = 0; i < GetInputSize(); i++) {
					labels[i] = "Input " + i;
				}
				
				inputLabels = labels;
			}
			return inputLabels;
		}
	}
	
	/// <summary>
	/// Gets the number of inputs.
	/// </summary>
	/// <returns>
	/// The input size.
	/// </returns>
	public int GetInputSize() {
		return inputs.Length;
	}
	
	/// <summary>
	/// Gets the number of the outputs.
	/// </summary>
	/// <returns>
	/// The output size.
	/// </returns>
	public int GetOutputSize() {
		return outputs.Length;
	}
	
	/// <summary>
	/// Gets the output node.
	/// </summary>
	/// <returns>
	/// The node connected to the given output.
	/// </returns>
	/// <param name='number'>
	/// Number of the desired output.
	/// </param>
	public MarrowNode GetOutput(int number) {
		if (outputs == null)
			return null;
		else
			return outputs[number];
	}
	
	/// <summary>
	/// Gets the corresponding input that this output connects to.
	/// </summary>
	/// <returns>
	/// The number of the receiving input.
	/// </returns>
	/// <param name='number'>
	/// Number.
	/// </param>
	public int GetOutputConnection(int number) {
		if (outputConnections == null)
			return -1;
		else
			return outputConnections[number];
	}
	
	/// <summary>
	/// Gets the input node.
	/// </summary>
	/// <returns>
	/// The node connected to the given input.
	/// </returns>
	/// <param name='number'>
	/// Number of the desired input.
	/// </param>
	public MarrowNode GetInput(int number) {
		if (inputs == null)
			return null;
		else
			return inputs[number];
	}
	
	/// <summary>
	/// Determines whether this instance is true.
	/// </summary>
	/// <returns>
	/// <c>true</c> if this instance is true; otherwise, <c>false</c>.
	/// </returns>
	public virtual bool IsTrue() {
		return false;
	}
	
	/// <summary>
	/// Raises the input event.
	/// </summary>
	/// <param name='input'>
	/// Input.
	/// </param>
	public virtual void OnInput(int input) {
		
	}
	
	/// <summary>
	/// Sends the output.
	/// </summary>
	/// <param name='output'>
	/// Output.
	/// </param>
	public virtual void SendOutput(int output) {
		if (outputs[output] != null) {
			outputs[output].OnInput(outputConnections[output]);
		}
	}
	
	/// <summary>
	/// Sets the output.
	/// </summary>
	/// <param name='number'>
	/// Number of the output.
	/// </param>
	/// <param name='item'>
	/// Node that this output will connect to.
	/// </param>
	/// <param name='input'>
	/// The receiving input.
	/// </param>
	public void SetOutput(int number, MarrowNode item, int input) {
		outputs[number] = item;
		outputConnections[number] = input;
		item.SetInput(input, this);
	}
	
	/// <summary>
	/// Sets the input. Usually, you won't need to use this.
	/// Instead just set the output on the other side of the 
	/// connection.
	/// </summary>
	/// <param name='number'>
	/// Number of the input.
	/// </param>
	/// <param name='item'>
	/// Node to connect.
	/// </param>
	public void SetInput(int number, MarrowNode item) {
		inputs[number] = item;
	}
	
	/// <summary>
	/// Gets the name of this node. Define the name using a
	/// NodeDescription.
	/// </summary>
	/// <returns>
	/// The name.
	/// </returns>
	public string GetName() {
		System.Attribute[] attrs = System.Attribute.GetCustomAttributes(this.GetType());
		
		foreach (System.Attribute attr in attrs) {
			if (attr is NodeDescription) {
				return ((NodeDescription) attr).GetName();
			}
		}
		
		return null;
	}
	
	/// <summary>
	/// Gets the default outputs, as defined in the NodeDescription.
	/// </summary>
	/// <returns>
	/// The default outputs.
	/// </returns>
	public int GetDefaultOutputs() {
		System.Attribute[] attrs = System.Attribute.GetCustomAttributes(this.GetType());
		
		foreach (System.Attribute attr in attrs) {
			if (attr is NodeDescription) {
				return ((NodeDescription) attr).GetOutputs();
			}
		}
		
		return 0;
	}
	
	/// <summary>
	/// Gets the default inputs, as defined in the NodeDescription.
	/// </summary>
	/// <returns>
	/// The default inputs.
	/// </returns>
	public int GetDefaultInputs() {
		System.Attribute[] attrs = System.Attribute.GetCustomAttributes(this.GetType());
		
		foreach (System.Attribute attr in attrs) {
			if (attr is NodeDescription) {
				return ((NodeDescription) attr).GetInputs();
			}
		}
		
		return 0;
	}
	
	/// <summary>
	/// Gets the type of the node, as defined in the NodeDescription.
	/// </summary>
	/// <returns>
	/// The node type.
	/// </returns>
	public NodeDescription.Type GetNodeType() {
		System.Attribute[] attrs = System.Attribute.GetCustomAttributes(this.GetType());
		
		foreach (System.Attribute attr in attrs) {
			if (attr is NodeDescription) {
				return ((NodeDescription) attr).GetNodeType();
			}
		}
		
		return NodeDescription.Type.CUSTOM;
	}
	
	/// <summary>
	/// Gets or sets the game object. Marrow will automatically set this.
	/// Do not try to set it again or it will throw an exception.
	/// </summary>
	/// <value>
	/// The game object.
	/// </value>
	/// <exception cref='UnityException'>
	/// Is thrown when you try to define a game object that is already set.
	/// </exception>
	public GameObject gameObject {
		get {
			return thisObject;
		}
		set {
			if (thisObject != null)
				throw new UnityException("GameObject already set");
			else 
				thisObject = value;
		}
	}
	
	/// <summary>
	/// Sever the specified node.
	/// </summary>
	/// <param name='node'>
	/// Node.
	/// </param>
	public void Sever(MarrowNode node) {
		for (int i = 0; i < outputs.Length; i++) {
			if (outputs[i] == node) {
				outputs[i].SetInput(outputConnections[i], null);
				outputs[i] = null;
				outputConnections[i] = 0;
			}
		}
	}
	
	/// <summary>
	/// Raises the property change event. Usually
	/// called by the editor. Override to provide
	/// custom behaviors if a property changes.
	/// See the Split node for an example.
	/// </summary>
	public virtual void OnPropertyChange() {
	}
}

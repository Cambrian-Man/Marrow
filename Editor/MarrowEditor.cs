using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public class MarrowEditor : EditorWindow {
	public static Texture connectionTex;
	
	[SerializeField]
	Rect toolbarRect = new Rect(0, 0, 150, 300);
	List<MarrowNode> nodes;
	
	[SerializeField]
	int gridSize = 20;
	
	[SerializeField]
	bool snap = true;
	
	Vector2 nodeTypeScrollPosition;
	
	Vector2 layoutScrollPosition = Vector2.zero;
	
	Dictionary<NodeDescription.Type, List<System.Type>> NodeTypes;
	
	bool[] NodeFoldouts;
	
	MarrowNode inspected;
	Dictionary<string, System.Reflection.FieldInfo> inspectorFields;
	
	private struct Link {
		public MarrowNode input;
		public MarrowNode output;
		
		public int inputNumber;
		public int outputNumber;
	}
	
	private Link link;
	
	[MenuItem("Window/Marrow Editor")]
	static void Init() {
		System.Type[] dockNextTo = new System.Type[1];
		dockNextTo[0] = typeof(SceneView);
		
		MarrowEditor window = EditorWindow.GetWindow<MarrowEditor>(dockNextTo);
		window.Show();
		
	}
	
	private Dictionary<NodeDescription.Type, List<System.Type>> GetNodeTypes() {
		Dictionary<NodeDescription.Type, List<System.Type>> types = new Dictionary<NodeDescription.Type, List<System.Type>>();
		
		List<System.Type> marrowTypes = new List<System.Type>();
		System.Reflection.Assembly assembly = System.Reflection.Assembly.GetAssembly(typeof(MarrowNode));
		foreach (System.Type type in assembly.GetTypes()) {
			if (type.IsClass && !type.IsAbstract && type.IsSubclassOf(typeof(MarrowNode)))
				marrowTypes.Add(type);
		}
		
		foreach (var v in System.Enum.GetValues(typeof(NodeDescription.Type))) {
			NodeDescription.Type t = (NodeDescription.Type) v;
			types.Add(t, new List<System.Type>());
		}
		
		foreach (System.Type type in marrowTypes) {
			NodeDescription attr = (NodeDescription) System.Attribute.GetCustomAttribute(type, typeof(NodeDescription));
			types[attr.GetNodeType()].Add(type);
		}
		
		return types;
	}
			
	void OnEnable() {
		NodeTypes = GetNodeTypes();
		NodeFoldouts = new bool[System.Enum.GetNames(typeof(NodeDescription.Type)).Length];
		inspected = null;
		OnSelectionChange();
	}
	
	void OnGUI() {
		GUILayout.Label("Marrow Editor", EditorStyles.boldLabel);
		Vector2 scrollBarSize = GetLayoutSize();
		layoutScrollPosition = GUI.BeginScrollView(new Rect(0, 0, position.width - 300, position.height), layoutScrollPosition,
			new Rect(0, 0, scrollBarSize.x, scrollBarSize.y));
		
		GUI.Box(new Rect(0, 0, scrollBarSize.x, scrollBarSize.y), "");
		
		BeginWindows();
		if (nodes != null) {
			toolbarRect = GUI.Window(nodes.Count, toolbarRect, DrawToolbar, "Tools");
			DrawNodes();
		}
		else {
			GUI.Label(new Rect((position.width - 300) / 2, position.height / 2, 300, 20), "No Marrow object selected.");
		}
		
		EndWindows();
		GUI.EndScrollView();
		
		GUILayout.BeginArea(new Rect(position.width - 295, 5, 290, position.height - 5));

		DrawInspector();
		
		GUILayout.EndArea();
	}
	
	Vector2 GetLayoutSize() {
		Vector2 size = new Vector2(position.width - 300, position.height);
		
		if (nodes == null)
			return size;
		
		foreach (MarrowNode node in nodes) {
			if (node.editorPosition.xMax > size.x)
				size.x = node.editorPosition.xMax + 100;
			
			if (node.editorPosition.yMax > size.y)
				size.y = node.editorPosition.yMax + 100;
		}
		
		return size;
	}
			
	void OnSelectionChange() {
		if (Selection.activeGameObject != null && Selection.activeGameObject.GetComponent<Marrow>() != null) {
			nodes = Selection.activeGameObject.GetComponent<Marrow>().GetNodes();
		}
		else {
			nodes = null;
		}
		
		inspected = null;
		
		Repaint();
	}
		
	void DrawToolbar (int windowID) {
		GUILayout.Label("Add Node");
		nodeTypeScrollPosition = GUILayout.BeginScrollView(
			nodeTypeScrollPosition);

		foreach (KeyValuePair<NodeDescription.Type, List<System.Type>> kvp in NodeTypes) {
			NodeFoldouts[(int) kvp.Key] = EditorGUILayout.Foldout(NodeFoldouts[(int) kvp.Key], kvp.Key.ToString());
			
			if (NodeFoldouts[(int) kvp.Key]) {
				foreach (System.Type t in kvp.Value) {
					string label = "Undefined Name";
					System.Attribute[] attrs = (System.Attribute[]) t.GetCustomAttributes(false);
					
					foreach (System.Attribute attr in attrs) {
						if (attr is NodeDescription) {
							label = ((NodeDescription) attr).GetName();
						}
					}
					
					if(GUILayout.Button(label)) {
						System.Reflection.MethodInfo method = this.GetType().GetMethod("AddNode", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
						System.Reflection.MethodInfo generic = method.MakeGenericMethod(new System.Type[]{t});
						generic.Invoke(this, null);
					}	
				}
			}
		}
		

		GUILayout.EndScrollView();
				
		if(GUILayout.Button("Clear All")) {
			if (nodes != null) {
				foreach (MarrowNode node in nodes) {
					DestroyImmediate(node);
				}
				inspected = null;
				nodes.Clear();
			}
		}
		
		GUI.DragWindow();
	}
	
	void DrawNodes() {
		MarrowNode node;
		for (int i = 0; i < nodes.Count; i++) {
			node = nodes[i];
			
			node.editorPosition = GUI.Window(i, node.editorPosition, DrawNode, node.GetName());
			node.editorPosition.x = Mathf.Clamp(node.editorPosition.x, 0, 10000);
			node.editorPosition.y = Mathf.Clamp(node.editorPosition.y, 0, 10000);
		}
		
		for (int i = 0; i < nodes.Count; i++) {
			node = nodes[i];
			DrawConnections(node);
		}
	}
	
	void DrawNode(int nodeID) {
		MarrowNode node = nodes[nodeID];
				
		for (int i = 0; i < node.GetInputSize(); i++) {
			Rect connectionBox = GetConnectionBox(true, i, node);
			
			if (ClickBox(new Rect(0, connectionBox.y + 3, 10, 10), node.editorPosition)) {
				link.input = node;
				link.inputNumber = i;
				OnLink();
			}
			GUI.Label(new Rect(12, connectionBox.y, connectionBox.width, connectionBox.height),
				node.InputLabels[i]);
		}
		
		for (int i = 0; i < node.GetOutputSize(); i++) {
			Rect connectionBox = GetConnectionBox(false, i, node);
			
			if (ClickBox(new Rect(connectionBox.xMax - 10, connectionBox.y + 3, 10, 10), node.editorPosition)) {
				link.output = node;
				link.outputNumber = i;
				OnLink();
			}
			GUI.Label(new Rect(0, connectionBox.y, connectionBox.width, connectionBox.height),
				node.OutputLabels[i]);
		}
		
		NodeSelect(node);
		
		GUI.DragWindow();
		if (snap) {
			node.editorPosition.x = Mathf.Floor(node.editorPosition.x / gridSize) * gridSize;
			node.editorPosition.y = Mathf.Floor(node.editorPosition.y / gridSize) * gridSize;
		}
		node.editorPosition.width = 100;
		node.editorPosition.height = 30 + ((node.GetOutputSize() + node.GetInputSize()) * 18);
	}

	void NodeSelect(MarrowNode node) {
		if (Event.current.type == EventType.MouseDown) {
			Rect window = new Rect(0, 0, node.editorPosition.width, node.editorPosition.height);
			if (window.Contains(Event.current.mousePosition)) {
				if (Event.current.button == 0) {
					inspected = node;
					inspectorFields = GetNodeProperties(inspected);
				}
				else if (Event.current.button == 1 && Event.current.modifiers == EventModifiers.Control) {
					RemoveNode(node);
				}
			}
		}
	}
	
	Rect GetConnectionBox(bool isInput, int num, MarrowNode node) {
		if (isInput)
			return new Rect(0, (num * 18) + 20, node.editorPosition.width, 24);
		else
			return new Rect(0, (num * 18) + 20 + (node.GetInputSize() * 18), node.editorPosition.width, 24);
	}
	
	bool ClickBox(Rect rect, Rect window) {
		GUI.Box(rect, "");
		
		if (Event.current.type == EventType.MouseUp) {
			if (rect.Contains(Event.current.mousePosition) && Event.current.button == 0)
				return true;
		}
		
		return false;
	}
	
	void DrawConnections(MarrowNode node) {
		for (int i = 0; i < node.GetOutputSize(); i++) {
			MarrowNode connectTo = node.GetOutput(i);
			if (connectTo == null) {
				break;
			}
			
			Handles.BeginGUI();
			Handles.color = Color.black;
			
			Rect startBox = GetConnectionBox(false, i, node);
			Vector3 startPos = new Vector3(node.editorPosition.xMax, 
				node.editorPosition.y + startBox.center.y - 5, 0);
			
			Rect endBox = GetConnectionBox(true, node.GetOutputConnection(i), node);
			Vector3 endPos = new Vector3(connectTo.editorPosition.x, 
				connectTo.editorPosition.y + endBox.center.y - 5, 0);
			
			Vector3 startTangent = startPos + Vector3.right * (Vector3.Distance(startPos, endPos) / 3F);
			Vector3 endTangent = endPos + Vector3.left * (Vector3.Distance(startPos, endPos) / 3F);			
			
			Handles.DrawBezier(startPos, endPos, startTangent, endTangent, Color.black, null, 2);
			Handles.EndGUI();
		}
	}
	
	void OnLink() {
		if (link.input == null || link.output == null)
			return;
				
		link.output.SetOutput(link.outputNumber, link.input, link.inputNumber);
		
		link.input = null;
		link.output = null;
		EditorUtility.SetDirty(Selection.activeGameObject);
	}
	
	void AddNode<T>() where T : MarrowNode {
		MarrowNode node = CreateInstance<T>();
		node.editorPosition = new Rect(position.width / 3, position.height / 2, 100, 100);
		node.SetInputSize(node.GetDefaultInputs());
		node.SetOutputSize(node.GetDefaultOutputs());
		node.OnCreate();
		node.gameObject = Selection.activeGameObject;
		nodes.Add(node);
		Repaint ();
		EditorUtility.SetDirty(Selection.activeGameObject);
	}
	
	void RemoveNode (MarrowNode toRemove) {
		foreach(MarrowNode node in nodes) {
			node.Sever(toRemove);
		}
		
		nodes.Remove(toRemove);
		DestroyImmediate(toRemove);
	}
	
	Dictionary<string, System.Reflection.FieldInfo> GetNodeProperties(MarrowNode node) {
		Dictionary<string, System.Reflection.FieldInfo> nodeProperties = new Dictionary<string, System.Reflection.FieldInfo>();
		
		System.Reflection.FieldInfo[] fields = node.GetType().GetFields();
		foreach (System.Reflection.FieldInfo field in fields) {
			foreach (System.Attribute attr in field.GetCustomAttributes(false)) {
				if (attr is  MarrowProperty) {
					nodeProperties.Add(field.ToString(), field);
				}
			}
		}
		
		return nodeProperties;
	}
	
	void DrawInspector() {
		if (inspected == null)
			return;
		
		foreach (string key in inspectorFields.Keys) {
			System.Type fieldType = inspectorFields[key].FieldType;
			if (fieldType == typeof(string)) {
				string editorText =	EditorGUILayout.TextField(CleanName(key), (string) inspectorFields[key].GetValue(inspected));
				UpdateProperty<string>(key, editorText);
			}
			else if (fieldType == typeof(int)) {
				int editorInt =	EditorGUILayout.IntField(CleanName(key), (int) inspectorFields[key].GetValue(inspected));
				UpdateProperty<int>(key, editorInt);
			}
			else if (fieldType == typeof(float)) {
				float editorFloat = EditorGUILayout.FloatField(CleanName(key), (float) inspectorFields[key].GetValue(inspected));
				UpdateProperty<float>(key, editorFloat);
			}
			else if (fieldType == typeof(bool)) {
				bool editorBool = EditorGUILayout.Toggle(CleanName(key), (bool) inspectorFields[key].GetValue(inspected));
				UpdateProperty<bool>(key, editorBool);
			}
			else if (fieldType == typeof(Vector3)) {
				Vector3 editorVec3 = EditorGUILayout.Vector3Field(CleanName(key), (Vector3) inspectorFields[key].GetValue(inspected));
				UpdateProperty<Vector3>(key, editorVec3);
			}
			else if (fieldType.IsEnum) {
				System.Enum editorEnum = EditorGUILayout.EnumPopup(CleanName(key), (System.Enum) inspectorFields[key].GetValue(inspected));
				UpdateProperty<System.Enum>(key, editorEnum);
			}
			else if (fieldType == typeof(GameObject)) {
				GameObject editorObj = EditorGUILayout.ObjectField(CleanName(key), (GameObject) inspectorFields[key].GetValue(inspected),
					typeof(GameObject), true) as GameObject;
				
				UpdateProperty<GameObject>(key, editorObj);
			}
			
		}
	}
	
	void UpdateProperty<T>(string key, T val) {
		if (inspectorFields[key].GetValue(inspected) == null && val == null)
			return;
		else if (inspectorFields[key].GetValue(inspected) != null &&
			inspectorFields[key].GetValue(inspected).Equals(val))
			return;
			

		inspectorFields[key].SetValue(inspected, val);
		inspected.OnPropertyChange();
		EditorUtility.SetDirty(Selection.activeGameObject);
		Repaint();
	}
	
	string CleanName(string name) {
		string[] n = name.Split(null);
		name = n[n.Length - 1];
		
		char[] charArray = name.ToCharArray();
		charArray[0] = char.ToUpper(charArray[0]);
		
		List<char> charList = new List<char>();
		
		charList.Add(charArray[0]);
		for (int i = 1; i < charArray.Length; i++) {
			if (char.IsUpper(charArray[i]) && char.IsLower(charArray[i - 1])) {
				charList.Add(' ');
				charList.Add(charArray[i]);
			}
			else
				charList.Add(charArray[i]);
		}
		
		return new string(charList.ToArray());
	}
}

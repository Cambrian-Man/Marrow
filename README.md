Marrow
===

What Is Marrow?
---

Marrow is an extensible, node-based behavior framework for Unity. It is meant to provide a quick and easy way to script content for game jams and larger projects. It comes with a simple inventory system and a set of common nodes that you can use to create interaction in your Unity games.

Using Marrow
---

The core of Marrow consists of two components: the Marrow component itself and the editor. To get started, attach a Marrow component to a game object and go to Window> Marrow Editor to create a new editor window. Select a Marrow object to begin editing it.

To create a new node, choose it in the toolbox. You may move it around and delete it by holding Ctrl and right-clicking it. To connect a node output to an input, click the box next to an output or input and then click the corresponding box on the other end.

The bottom of the toolbar has a button which lets you clear all nodes on an object.

Types of Nodes
---

*Input Nodes*

Input nodes take no inputs themselves, but rather respond to stimulus from the environment. Input nodes have an Update method that is called when the Marrow object updates.

*String Nodes*

String nodes implement the IStringNode interface which means they contain a string, which can be accessed with GetString()

*Value Nodes*

Value nodes implement the IValueNode interface and return a float and int value. They also return strings, since IValueNodes inherit IStringNode.


Writing Custom Nodes
---

Marrow only provides basic functionality by default, if you want to use it in your own project you’ll have to write your own nodes. Custom nodes must inherit from MarrowNode, though other classes, such as InputNode, provide some additional functionality. All nodes need a NodeDescription attribute that provides a name number of inputs and outputs, and a node type. Node types are primarily used for organizing nodes in the editor. Feel free to slot custom nodes wherever you need them. The custom type is provided in case there are nodes that just don’t fit anywhere else.

If you wish to provide user-editable properties, they must be public and flagged with the MarrowProperty attribute. Currently, int, float, string, enum, and GameObject types are supported.

You will also wish to override the InputLabels and OutputLabels properties if you wish to provide custom labels.
# ActionGraph

<p align="center">
  <img src="Images/Logo.svg" width="250">
</p>

**ActionGraph** is a visual scripting engine for the Godot game engine, written in C#. It's intended as a generic back-end for visual scripting tools and provides:
- A graph-based visual scripting editor.
- An importer plugin that can load graphs as executable programs.
- A runtime node that can execute imported programs.

The bare editor does not ship with any instantiable nodes - instead, a developer must add their own **node definitions**, which can then be instantiated in the graph editor.

## Architecture
ActionGraph is built around a set of core concepts, grouped into three layers.

### The Runtime
- **Programs**: programs that can be executed by an `InstructionProcess` node. They contain:
  - A list of **instruction definitions**, which define the executable units of the program. Each contains an opcode, a list of parameter IDs and the name of an execution handler.
  - A list of **instruction instances**. Each carries a list of arguments, which correspond to the parameter IDs from the definition.
- **Execution handlers**: small scripts that contain the implementation of an instruction.
- **Processes**: scene nodes that can run a program. They are also responsible for locating execution handlers.

### The Editor
- **Graphs**: The editor representation of a program. It supports four types of elements:
  - **Nodes**: The executable graph elements, defined by a list of **node definitions**. Each definition tells the editor what the node's contents are, and how they are drawn in the inspector and what instructions the node will compile to. A single node can compile to many runtime instructions. Each instruction is represented by a **form**, which contains a **field** for each instruction argument. Forms can be arranged into structures using **options**, **choices**, **tuples**, and **lists**.
	- A special type of parameter are **outputs**. These do not drawn as a field in the inspector, but instead add an output port to the node.
  - **Edges**: The connections between nodes. Can be visually segmented using **joints**.
  - **Frames**: A visual grouping of graph elements.
  - **Memos**: An editor sticky note.

### The Importer
Takes the `.agp` files created by the graph editor, and converts them into executable programs.

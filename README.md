# ActionGraph
**ActionGraph** is a visual scripting framework for the Godot game engine, written in C#. It provides the following:
- A graph-based editor for writing game behavior graphs.
- An importer for graph files, which are loaded as `GraphProgram` resources.
- A runtime `GraphProcess` node that can execute loaded programs.

It is designed primarily for cutscenes, but can be used for any kind of game logic.

## Concepts
ActionGraph is built around a small set of core concepts.

The **graph** is the core of the module. It consists of a set of nodes and connections between them, and represents a program of some kind. Each node can be marked as a **start point** from which execution can be started; multiple start points can exist.

A **graph element** is an object on the graph. Four types exist:
- A **node** represents an action of some kind.
- A **frame** is a visual grouping of elements.
- A **memo** is an editor comment or sticky note.
- A **joint** is a way to break up straight lines in the editor.

The **inspector** shows the editable contents of a selected element.

Nodes essentially act as a container for **instructions**, which are the core executable units of the module. Nodes can expose various **structures** of instructions, such as optional toggles, choice dropdowns, tuple groups and lists.

By default, no instruction and nodes exist in the module. A user must supply their own **instruction definitions** and **node definitions** before programs can be created.

Graphs are saved as `.isa` **action graph files**, which is based on the XML format.

### Programs
A **program** is the compiled result of a `.isa` graph file: a flat list of **instructions** paired with an **instruction set** that defines what those instructions mean. Programs are stored as `GraphProgram` resources.

### Execution Handlers
An **execution handler** is the code that actually runs an instruction at runtime. Handlers inherit from `ExecutionHandler` and are collected by the `GraphProcess` node upon initialization.

Because handlers are resolved by name rather than by direct reference, new instructions can be added to a project simply by writing a handler and registering it in an instruction set - no changes to the core runtime are required.

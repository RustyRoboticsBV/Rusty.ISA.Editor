# Virtual ISA
A generic *instruction set architecture* (ISA) module, written for the Godot game engine in C#. It allows you to:
- Define an instruction set.
- Write programs with this instruction set, using a graph-based editor.
- Execute these programs from within a Godot game.

It is mainly intended for cutscenes, but can be used for any kind of in-game behavior.

## Terminology
In this module, we define a *process* as a node that can run a *program*, which consist of *instructions*. Each instruction has an *opcode*, *parameters* and an *execution handler*.

A process can start executing a program at explicitly-defined *start points*, and stops when it reaches an *end point*. Programs may have multiple start and end points.

## Graph Editor
This tab allows for the creation of ISA programs, using a graph-based interface.

[TODO: screenshot]

The editor has three components:
- The graph. It can be used to create, delete, select, move, connect and disconnect nodes. Multiple nodes can be selected at the same time.
- The inspector. It displays the contents of the selected nodes, and allows you to edit them. Each node comes with an optional start field, which allows you to define entry points from which the program can be executed.
- The console. It displays messages, warnings and errors.

[TODO: elaborate on usage]

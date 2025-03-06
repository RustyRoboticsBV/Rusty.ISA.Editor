# Rusty.CutsceneEditor
A cutscene editor app for the Rusty.Custcene module.

Related repositories:
- [Rusty.Cutscenes](https://github.com/RustyRoboticsBV/Rusty.Cutscenes): The core module.
- [Rusty.CutsceneImporter](https://github.com/RustyRoboticsBV/Rusty.CutsceneImporter): A cutscene resource importer tool.
- [Rusty.CutscenePackage](https://github.com/RustyRoboticsBV/Rusty.CutscenePackage): A combination of the core module, the importer, an implemented instruction set and various other relevant modules.

## Summary
This app allows for the creation of cutscene programs, using a graph-based interface.

[TODO: screenshot]

The editor has three components:
- The graph. It can be used to create, delete, select, move, connect and disconnect nodes. Multiple nodes can be selected at the same time.
- The inspector. It displays the contents of the selected nodes, and allows you to edit them. Each node comes with an optional start field, which allows you to define entry points from which the cutscene program can be played.
- File buttons. You can save and load programs to and from a file or the clipboard.

[TODO: elaborate on usage]

The app saves cutscenes in a CSV file format.

## Adding Instructions
You can add instruction definitions to the editor by selecting the *Instruction Set* tab. Here, you can add, remove and edit instruction definitions. The definitions are stored in the InstructionSet folder, should you wish to inspect the files directly.

If you want the editor to be able to create nodes for an instruction, be sure to tick the *Has Editor Node* checkbox. If you leave this blank, then the instruction can only appear as a pre-instruction of other nodes (see the [instruction definition manual](https://github.com/RustyRoboticsBV/Rusty.Cutscenes/Documentation/InstructionDefinitions.md) for more details).

Make sure to export your instruction set and add it to your Godot game project whenever you make any changes!

## Built-in Instructions
In addition to the [built-in instruction from the core module](https://github.com/RustyRoboticsBV/Rusty.Cutscenes?tab=readme-ov-file#built-in-instructions), the editor compiles additional *marker instructions* into cutscene program files. These instructions have no in-game meaning, but are required to preserve editability. They include the following:
- `NOD(x, y)`: this notifies the editor that the next instructions are part of the same node. The last member of the group represents the 'main' instruction of the node. The first two members may optionally be a `STA` and `LAB` instruction. Everything in-between represents the node's pre-instructions.
- Pre-instruction markers: these notify the editor that the next instructions are part of a pre-instruction group.
  - `OPT()`: notifies the editor that the following instruction is part of an option structure. If the group has no member, it is disabled.
  - `CHO(selected)`: notifies the editor that the following instruction is part of a choice structure, and that this instruction represents the nth selection.
  - `TPL()`: notifies the editor that the following instructions are part of a tuple structure.
  - `LST()`: notifies the editor that the following instructions are part of a list structure.
- `EOG()`: Ends a node or pre-instruction group.

All editor-only instructions are stripped out when a program is loaded into Godot as a CutsceneProgram resource.

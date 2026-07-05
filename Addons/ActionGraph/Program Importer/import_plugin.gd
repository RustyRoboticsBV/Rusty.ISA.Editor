@tool
extends EditorImportPlugin;

func _get_importer_name():
	return "rusty_robotics_bv.action_graph.program_importer";

func _get_visible_name():
	return "InstructionProgram Importer";

func _get_recognized_extensions():
	return ["agxp"];

func _get_save_extension():
	return "res";

func _get_resource_type():
	return "Resource";

func _get_preset_count():
	return 0;

func _get_preset_name(_preset_index):
	return "";

func _get_import_options(_path, _preset_index):
	return [];

func _get_option_visibility(_path, _option_name, _options):
	return true;

func _import(source_file, save_path, _options, _r_platform_variants, _r_gen_files):
	var file := FileAccess.open(source_file, FileAccess.READ);
	if file == null:
		return FileAccess.get_open_error();
	
	var text := file.get_as_text();
	var resource := XmlLoader.LoadAsProgram(text);
	var error = ResourceSaver.save(resource, "%s.res" % save_path);
	return error;

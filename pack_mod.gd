extends SceneTree

# Raw source formats Godot compiles into .ctex / other cache files.
# These are normally loaded via their .import remap, but we keep the
# source as a fallback when the compiled cache is missing.
const SKIP_EXTENSIONS: Array[String] = [
	".png", ".jpg", ".jpeg", ".webp", ".bmp", ".svg", ".tga",
	".ogg", ".mp3", ".wav",  # audio is compiled to cache just like images
]

# Change this order if you're targeting mobile/web instead.
const REMAP_PATH_KEYS: Array[String] = [
	"path.s3tc_bptc",   # modern desktop (DX11+ / Vulkan)
	"path.etc2_astc",   # mobile fallback
	"path",             # generic / uncompressed fallback
]

var _fallback_count: int = 0
var _missing_count: int = 0

func _init() -> void:
	var args: PackedStringArray = OS.get_cmdline_user_args()
	if args.size() < 2:
		printerr("Error: Missing arguments. Usage: godot -s script.gd -- <mod_folder> <output_path>")
		quit(1)
		return

	var mod_folder: String = args[0].trim_suffix("/").trim_prefix("res://")
	var output_path: String = args[1]

	# Resolve to the actual filesystem directory name to fix path casing.
	# MSBuild or the command line may pass a different casing than what
	# exists on disk.  Godot resource paths are case-sensitive, so the
	# .pck directory must match the C# ModId casing exactly.
	# We list the project root to find the directory's real name.
	var res_base: String = "res://" + mod_folder
	var dir_check: DirAccess = DirAccess.open(res_base)
	if dir_check == null:
		printerr("Error: Cannot open mod folder: ", res_base)
		quit(1)
		return
	var real_folder: String = _resolve_dir_casing(mod_folder)
	if real_folder != mod_folder:
		print("Normalized folder casing: \"", mod_folder, "\" -> \"", real_folder, "\"")
		mod_folder = real_folder
		res_base = "res://" + mod_folder

	var packer: PCKPacker = PCKPacker.new()
	var err: int = packer.pck_start(output_path, 16)

	if err != OK:
		printerr("Failed to start PCK packer. Error code: ", err)
		quit(1)
		return

	print("Packing folder: " + res_base)
	_pack_folder_recursive(packer, res_base)

	err = packer.flush(true)
	if err == OK:
		print("Successfully packed: " + output_path)
		if _fallback_count > 0:
			push_warning("Packed ", _fallback_count, " source-file fallback(s) because compiled cache was missing.")
		if _missing_count > 0:
			push_warning("Skipped ", _missing_count, " import(s) — no compiled cache or source found.")
		quit(0)
	else:
		printerr("Failed to flush PCK file. Error code: ", err)
		quit(1)


func _pack_folder_recursive(packer: PCKPacker, path: String) -> void:
	var dir: DirAccess = DirAccess.open(path)
	if dir == null:
		return

	dir.list_dir_begin()
	var file_name: String = dir.get_next()

	while file_name != "":
		# Skip filesystem noise and the editor's internal cache folder.
		if file_name == "." or file_name == ".." or file_name == ".godot":
			file_name = dir.get_next()
			continue

		var full_path: String = path + "/" + file_name

		if dir.current_is_dir():
			_pack_folder_recursive(packer, full_path)
		else:
			# For import metadata files, also pack the compiled cache asset.
			# If the compiled cache is missing, the source file will be
			# packed as a fallback so the game can still load the resource.
			var is_import_file: bool = file_name.ends_with(".import")
			if is_import_file:
				var cache_packed: bool = _pack_imported_dependency(packer, full_path)
				# If we successfully packed either the cache or the source
				# fallback, skip the source file below to avoid duplication.
				if cache_packed:
					# Still need to pack the .import file itself.
					var err: int = packer.add_file(full_path, full_path)
					if err != OK:
						printerr("Failed to pack import file: ", full_path)
					file_name = dir.get_next()
					continue

			# Skip raw source formats — the compiled .ctex (or fallback
			# packed above) is what Godot loads at runtime.
			var is_raw_source: bool = SKIP_EXTENSIONS.any(
				func(ext: String) -> bool: return file_name.ends_with(ext)
			)
			if not is_raw_source:
				var err: int = packer.add_file(full_path, full_path)
				if err != OK:
					printerr("Failed to pack file: ", full_path)

		file_name = dir.get_next()


func _pack_imported_dependency(packer: PCKPacker, import_file_path: String) -> bool:
	var config: ConfigFile = ConfigFile.new()
	if config.load(import_file_path) != OK:
		push_warning("Cannot parse import file: ", import_file_path)
		return false

	if not config.has_section("remap"):
		return false

	# Try each compiled-cache variant in priority order.
	for key in REMAP_PATH_KEYS:
		var cache_path = config.get_value("remap", key, "")
		if cache_path is String and cache_path != "" and FileAccess.file_exists(cache_path):
			var err: int = packer.add_file(cache_path, cache_path)
			if err != OK:
				printerr("Failed to pack imported cache: ", cache_path)
			return true

	# Compiled cache not found — fall back to packing the raw source file
	# so Godot can still resolve the resource at runtime.
	var source_file: String = config.get_value("deps", "source_file", "")
	if source_file != "" and FileAccess.file_exists(source_file):
		push_warning("Compiled cache missing, falling back to source: ", source_file)
		var err: int = packer.add_file(source_file, source_file)
		if err != OK:
			printerr("Failed to pack source fallback: ", source_file)
		_fallback_count += 1
		return true

	push_warning("No compiled cache or source found for: ", import_file_path)
	_missing_count += 1
	return false


func _resolve_dir_casing(name_hint: String) -> String:
	var root: DirAccess = DirAccess.open("res://")
	if root == null:
		return name_hint

	var lower: String = name_hint.to_lower()
	root.list_dir_begin()
	var entry: String = root.get_next()
	while entry != "":
		if entry.to_lower() == lower and root.current_is_dir():
			return entry
		entry = root.get_next()

	return name_hint

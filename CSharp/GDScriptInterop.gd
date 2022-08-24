extends Node2D

onready var csharp_interop = get_node("../CSharpInterop");
var MessageSerializer = preload("res://addons/godot-rollback-netcode/MessageSerializer.gd");
var CSharpMessageSerializer = preload("res://addons/godot-rollback-netcode/mono/MessageSerializer.cs");
# Called when the node enters the scene tree for the first time.
func _ready():
#	print("From Buffer: GDScript 2_340_234_023 -> C# " + str(csharp_interop.CastMeFromBuffer(2_340_234_023)))
#	print("From Buffer to Dict: GDScript 2_340_234_023 -> C# " + str(csharp_interop.CastMeFromBufferToDict(2_340_234_023)) + " = " + str(csharp_interop.CastMeFromBufferToDict(2_340_234_023)['number']))

	var only_print_failures := true

	var test_serialize_messages := [
#		{
#			0:1, 
#			1:{
#				1: PoolByteArray([18, 0, 0, 0, 1, 0, 0, 0, 4, 0, 0, 0, 1, 0, 0, 0, 36, 0, 0, 0, 2, 0, 0, 0, 183, 181, 2, 0])
#			}, 
#			2:1, 
#			3:{}
#		},
		{
			0:121, 
			1:{
				119: PoolByteArray([18, 0, 0, 0, 2, 0, 0, 0, 4, 0, 0, 0, 23, 0, 0, 0, 47, 114, 111, 111, 116, 47, 77, 97, 105, 110, 47, 83, 101, 114, 118, 101, 114, 80, 108, 97, 121, 101, 114, 0, 18, 0, 0, 0, 1, 0, 0, 0, 4, 0, 0, 0, 12, 0, 0, 0, 105, 110, 112, 117, 116, 95, 118, 101, 99, 116, 111, 114, 5, 0, 0, 0, 0, 0, 128, 63, 0, 0, 0, 0, 4, 0, 0, 0, 1, 0, 0, 0, 36, 0, 0, 0, 2, 0, 1, 0, 94, 120, 6, 226, 0, 0, 0, 0]), 
				120: PoolByteArray([18, 0, 0, 0, 2, 0, 0, 0, 4, 0, 0, 0, 23, 0, 0, 0, 47, 114, 111, 111, 116, 47, 77, 97, 105, 110, 47, 83, 101, 114, 118, 101, 114, 80, 108, 97, 121, 101, 114, 0, 18, 0, 0, 0, 1, 0, 0, 0, 4, 0, 0, 0, 12, 0, 0, 0, 105, 110, 112, 117, 116, 95, 118, 101, 99, 116, 111, 114, 5, 0, 0, 0, 0, 0, 128, 63, 0, 0, 0, 0, 4, 0, 0, 0, 1, 0, 0, 0, 36, 0, 0, 0, 2, 0, 1, 0, 94, 120, 6, 226, 0, 0, 0, 0]), 
				121: PoolByteArray([18, 0, 0, 0, 2, 0, 0, 0, 4, 0, 0, 0, 23, 0, 0, 0, 47, 114, 111, 111, 116, 47, 77, 97, 105, 110, 47, 83, 101, 114, 118, 101, 114, 80, 108, 97, 121, 101, 114, 0, 18, 0, 0, 0, 1, 0, 0, 0, 4, 0, 0, 0, 12, 0, 0, 0, 105, 110, 112, 117, 116, 95, 118, 101, 99, 116, 111, 114, 5, 0, 0, 0, 0, 0, 128, 63, 0, 0, 0, 0, 4, 0, 0, 0, 1, 0, 0, 0, 36, 0, 0, 0, 2, 0, 1, 0, 94, 120, 6, 226, 0, 0, 0, 0])
			}, 
			2:118, 
			3:{116:1403911166, 117:654315295, 118:3755780987}
		}
	];

	var serializer = MessageSerializer.new()
	var csharp_serializer = CSharpMessageSerializer.new()
	
	print("********TEST SERIALIZE MESSAGE***********")
	for test_serialize_message in test_serialize_messages:
	
		var test_header = ""
		test_header += "---------------------------------------------\n"
		test_header += "Testing " + str(test_serialize_message) + "\n"
		
		var string = ""
		
		var gd_serialized_message = serializer.serialize_message(test_serialize_message)
		var cs_serialized_message = csharp_serializer.serialize_message(test_serialize_message)
		var serialize_equal = str(gd_serialized_message) == str(cs_serialized_message)
		
		string += "GDScript Serialize Message\n"
		string += "\t" + str(JSON.print(gd_serialized_message)) + "\n";
		string += "CSharp Serialize Message\n"
		string += "\t" + str(JSON.print(cs_serialized_message)) + "\n";
		string += "Equal? " + str(serialize_equal) + "\n"
		string += "\n"
		
		var second_string = ""
		
		var gd_unserialized_message = serializer.unserialize_message(gd_serialized_message)
		var cs_unserialized_message = csharp_serializer.unserialize_message(cs_serialized_message)
		var unserialize_equal = str(gd_unserialized_message) == str(cs_unserialized_message)
		
		second_string += "GDScript Unserialize Message\n"
		second_string += "\t" + str(JSON.print(gd_unserialized_message)) + "\n"
		second_string += "CSharp Unserialize Message\n"
		second_string += "\t" + str(JSON.print(cs_unserialized_message)) + "\n"
		second_string += "Equal? " + str(unserialize_equal) + "\n"
		
		if only_print_failures:
			if not unserialize_equal or not serialize_equal:
				print(test_header)
			if not serialize_equal:
				print(string)
			if not unserialize_equal:
				print(second_string)
		else:
			print(test_header)
			print(string)
			print(second_string)
		
	
	var test_serialize_inputs := [
		{"$":3_524_683_015, "/root/Main/ServerPlayer":{"input_vector": Vector2(-1, 0)}},
		{"$":3792074846, "/root/Main/ServerPlayer":{"input_vector": Vector2(1, 0)}}
	]
	
	return
	
	print()
	print()
	print()
	
	print("********TEST SERIALIZE INPUT***********")
	for test_serialize_input in test_serialize_inputs:
		print("---------------------------------------------")
		print("Testing " + str(test_serialize_input))
		print("")
		var gd_serialized_input = serializer.serialize_input(test_serialize_input)
		var cs_serialized_input = csharp_serializer.serialize_input(test_serialize_input)
		
		print("GDScript Serialize Input")
		print("\t" + str(gd_serialized_input));
		print("CSharp Serialize Input")
		print("\t" + str(cs_serialized_input));
		print("Equal? " + str(str(gd_serialized_input) == str(cs_serialized_input)))
		print("")

		var gd_unserialized_input = serializer.unserialize_input(gd_serialized_input)
		var cs_unserialized_input = csharp_serializer.unserialize_input(cs_serialized_input)

		print("GDScript Unserialize Input")
		print("\t" + str(gd_unserialized_input));
		print("CSharp Unserialize Input")
		print("\t" + str(cs_unserialized_input));
		print("Equal? " + str(str(gd_unserialized_input) == str(cs_unserialized_input)))


# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
#	pass

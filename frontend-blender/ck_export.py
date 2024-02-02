import bpy
import os
import json

blend_file_path = bpy.data.filepath

collection_name = "Territories"
collection = bpy.data.collections.get(collection_name)

positions = {}

if collection is not None:
    for obj in collection.objects:

        # Stash the territory position in AFrame xyz
        positions[obj.name] = {
            "x": obj.location.x,
            "y": obj.location.z,
            "z": (-1 * obj.location.y)
        }

        # Set the GLB file name as the object name with .glb extension
        glb_file_name = f"../frontend-blazor/CKBlazor/wwwroot/assets/territories/{obj.name}.glb"

        # Combine the blend file directory and GLB file name
        output_path = os.path.join(os.path.dirname(blend_file_path), glb_file_name)

        print(f"Exporting {obj.name} to {output_path}")

        # Deselect all objects
        bpy.ops.object.select_all(action='DESELECT')

        # Select the specific object
        obj.select_set(True)

        # Set the selected object as the active object
        bpy.context.view_layer.objects.active = obj

        # Export the selected object as GLB
        bpy.ops.export_scene.gltf(
            filepath=output_path,
            export_format='GLB',
            use_selection=True
        )

        print(f"Exported {obj.name} as GLB to {output_path}")
else:
    print(f"Collection '{collection_name}' not found.")

# Write the positions to json file
json_file_path = os.path.join(os.path.dirname(blend_file_path), "../frontend-blazor/CKBlazor/wwwroot/assets/territories/positions.json")

with open(json_file_path, 'w') as json_file:
    json.dump(positions, json_file, indent=2)

print(f"Object positions saved to {json_file_path}")
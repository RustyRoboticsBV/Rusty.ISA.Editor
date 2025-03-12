shader_type canvas_item;

uniform vec3 color : source_color = vec3(1);

void fragment()
{
	COLOR = vec4(color, texture(TEXTURE, UV).r);
}
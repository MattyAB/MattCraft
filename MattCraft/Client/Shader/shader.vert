#version 330 core
layout (location = 0) in vec3 aPosition;
layout (location = 1) in vec2 aTexPos;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

//out float colval;
out vec2 texPos;

void main()
{
	gl_Position = projection * view * model * vec4(aPosition.xyz, 1.0);
	//Render_Position = projection * view * model * vec4(aTexPos.xy, 0.0, 1.0);
	texPos = aTexPos;
}
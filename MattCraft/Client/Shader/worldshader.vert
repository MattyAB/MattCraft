#version 330 core
layout (location = 0) in vec3 aPosition;
layout (location = 1) in vec2 aTexPos;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

out vec2 texPos;
out vec4 col;

void main()
{
	gl_Position = projection * view * model * vec4(aPosition.xyz, 1.0);
	texPos = aTexPos;
	col = vec4(0.5, 1.0, 1.0, 1.0);
}
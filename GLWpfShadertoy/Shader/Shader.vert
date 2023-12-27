#version 330 core

layout (location = 0) in vec2 screenPos;
out vec2 texCoord;

void main()
{
	texCoord = screenPos;
	gl_Position = vec4(screenPos, 0.0, 1.0);
}
// 460 -- OpenGL 版本 4.6
#version 460 core

layout(location = 0) in vec3 aPos;	// 输入属性 0.  坐标
layout(location = 1) in vec3 aNormal;
layout(location = 2) in vec2 aTexCoord;

out vec2 TexCoord; // 纹理坐标

uniform mat4 model;
uniform mat4 view;
uniform mat4 proj;

void main() 
{
	gl_Position = proj * view * model * vec4(aPos, 1.0); // 顶点着色器 位置输出
	TexCoord = aTexCoord;
}
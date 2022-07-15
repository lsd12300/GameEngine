using System;
using OpenTK.Graphics.OpenGL4;
using System.IO;
using OpenTK.Mathematics;


namespace GameEngine.Engine
{
    public class Shader
    {

        public int m_shaderProgramID;


        /// <summary>
        ///  从文本加载Shader
        /// </summary>
        /// <param name="vsPath"></param>
        /// <param name="fsPath"></param>
        public unsafe Shader(string vsPath, string fsPath)
        {
            var infoStr = string.Empty;

            // 顶点着色器
            var vs = File.ReadAllText(vsPath);
            var vsID = GL.CreateShader(ShaderType.VertexShader);
            BindShaderSource(vsID, vs);
            GL.CompileShader(vsID);
            infoStr = GL.GetShaderInfoLog(vsID);
            if (!string.IsNullOrEmpty(infoStr))
            {
                Console.WriteLine($"vertexShader:  {infoStr}");
            }

            // 片段着色器
            var fs = File.ReadAllText(fsPath);
            int fsID = GL.CreateShader(ShaderType.FragmentShader);
            BindShaderSource(fsID, fs);
            GL.CompileShader(fsID);
            infoStr = GL.GetShaderInfoLog(fsID);
            if (!string.IsNullOrEmpty(infoStr))
            {
                Console.WriteLine($"FragmentShader:  {infoStr}");
            }

            // 着色器程序
            m_shaderProgramID = GL.CreateProgram();
            GL.AttachShader(m_shaderProgramID, vsID);
            GL.AttachShader(m_shaderProgramID, fsID);
            GL.LinkProgram(m_shaderProgramID);
            infoStr = GL.GetShaderInfoLog(m_shaderProgramID);
            if (!string.IsNullOrEmpty(infoStr))
            {
                Console.WriteLine($"ShaderProgram:  {infoStr}");
            }

            // 链接完, 清理资源
            GL.DeleteShader(vsID);
            GL.DeleteShader(fsID);

        }


        /// <summary>
        ///  激活 Shader
        /// </summary>
        public void Use()
        {
            GL.UseProgram(m_shaderProgramID);
        }

        public void SetInt(string name, int val)
        {
            GL.Uniform1(GL.GetUniformLocation(m_shaderProgramID, name), val);
        }

        public void SetMatrix4(string name, int count, bool traspose, float[] val)
        {
            GL.UniformMatrix4(GL.GetUniformLocation(m_shaderProgramID, name), count, traspose, val);
        }

        public void SetMatrix4(string name, bool traspose, Matrix4 val)
        {
            GL.UniformMatrix4(GL.GetUniformLocation(m_shaderProgramID, name), traspose, ref val);
        }


        public unsafe static void BindShaderSource(int shaderID, string shader)
        {
            int length = System.Text.Encoding.UTF8.GetByteCount(shader);
            GL.ShaderSource((uint)shaderID, 1, new string[1]
            {
                shader
            }, &length);
        }
    }
}

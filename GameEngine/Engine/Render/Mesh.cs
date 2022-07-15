using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;


namespace GameEngine.Engine
{
    /// <summary>
    ///  顶点
    ///     struct 结构体内存连续
    /// </summary>
    public struct Vertex
    {
        public Vector3 position;
        public Vector3 normal;
        public Vector2 texCoord;
    }


    public struct Texture
    {
        public uint id;
        public string type;
        public string path;
    }


    public class Mesh
    {

        public Vertex[] m_vertices;
        public uint[] m_indices;
        public Texture[] m_textures;

        private uint m_vao;
        private uint m_vbo;
        private uint m_ibo;


        public Mesh(Vertex[] verts, uint[] indices, Texture[] texs)
        {
            m_vertices = verts;
            m_indices = indices;
            m_textures = texs;

            SetupMesh();
        }

        /// <summary>
        ///  绘制网格
        /// </summary>
        public void Draw(Shader shader)
        {
            int diffIndex = 1;
            int speIndex = 1;
            for (int i = 0; i < m_textures.Length; i++)
            {
                GL.ActiveTexture(TextureUnit.Texture0 + i); // 激活纹理单元

                int number = 0;
                string name = m_textures[i].type;
                if (name == "texture_diffuse")
                    number = diffIndex++;
                else if (name == "texture_specular")
                    number = speIndex++;

                shader.SetInt($"{name}{number}", i); // 设置纹理单元
                GL.BindTexture(TextureTarget.Texture2D, m_textures[i].id);
            }
            GL.ActiveTexture(TextureUnit.Texture0); // 还原.  默认激活 0

            // 绘制网格
            GL.BindVertexArray(m_vao);
            GL.DrawElements(BeginMode.Triangles, m_indices.Length, DrawElementsType.UnsignedInt, 0);
            GL.BindVertexArray(0);
        }

        /// <summary>
        ///  配置渲染数据
        /// </summary>
        private unsafe void SetupMesh()
        {
            GL.GenVertexArrays(1, out m_vao);
            GL.GenBuffers(1, out m_vbo);
            GL.GenBuffers(1, out m_ibo);


            GL.BindVertexArray(m_vao);

            int sizeVertex = sizeof(float) * 8;
            GL.BindBuffer(BufferTarget.ArrayBuffer, m_vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, m_vertices.Length * sizeVertex, m_vertices, BufferUsageHint.StaticDraw);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, m_ibo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, m_indices.Length * sizeof(uint), m_indices, BufferUsageHint.StaticDraw);

            // 顶点位置
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, sizeVertex, 0);

            // 法线
            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, sizeVertex, sizeof(float) * 3);

            // 纹理坐标
            GL.EnableVertexAttribArray(2);
            GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, sizeVertex, sizeof(float) * 6);

            GL.BindVertexArray(0);      // 解绑
        }
    }
}

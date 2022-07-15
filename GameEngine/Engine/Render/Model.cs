using System;
using System.Collections.Generic;
using Assimp;
using System.IO;
using OTKTexWrapMode = OpenTK.Graphics.OpenGL4.TextureWrapMode;
using OTKTexMinFilter = OpenTK.Graphics.OpenGL4.TextureMinFilter;
using AiMesh = Assimp.Mesh;
using AiScene = Assimp.Scene;
using AiMaterial = Assimp.Material;



namespace GameEngine.Engine
{
    public class Model
    {

        private List<Mesh> m_meshes;
        private string m_directory;

        public HashSet<Texture> m_LoadedTextures = new HashSet<Texture>();



        public Model(string path)
        {
            m_meshes = new List<Mesh>();

            LoadModel(path);
        }

        public void Draw(Shader shader)
        {
            foreach (var m in m_meshes)
            {
                m.Draw(shader);
            }
        }


        /// <summary>
        ///  加载模型
        /// </summary>
        private void LoadModel(string path)
        {
            AssimpContext importer = new AssimpContext();
            // 加载选项:  Triangulate--所有图元强制变为三角形;  FlipUVs--翻转纹理Y轴
            var scene = importer.ImportFile(path, PostProcessSteps.Triangulate | PostProcessSteps.FlipUVs | PostProcessSteps.GenerateSmoothNormals | PostProcessSteps.CalculateTangentSpace);

            if (scene == null || (scene.SceneFlags & SceneFlags.Incomplete) > 0 || scene.RootNode == null)
            {
                Console.WriteLine($"ERROR.ASSIMP:  ImportFile Fail");
                return;
            }

            m_directory = Path.GetDirectoryName(path);

            ProcessNode(scene.RootNode, scene);
        }

        /// <summary>
        ///  处理所有节点
        /// </summary>
        private void ProcessNode(Node node, AiScene scene)
        {
            for (int i = 0; i < node.MeshCount; i++)
            {
                var mesh = scene.Meshes[node.MeshIndices[i]];
                m_meshes.Add(ProcessMesh(mesh, scene));
            }

            for (int i = 0; i < node.ChildCount; i++)
            {
                ProcessNode(node.Children[i], scene);
            }
        }

        private Mesh ProcessMesh(AiMesh mesh, AiScene scene)
        {
            var vertexCount = mesh.VertexCount;
            Vertex[] verts = new Vertex[vertexCount];
            List<uint> indices = new List<uint>();
            List<Texture> textures = new List<Texture>();

            // 顶点
            for (int i = 0; i < vertexCount; i++)
            {
                var vert = new Vertex()
                {
                    position = new OpenTK.Mathematics.Vector3(mesh.Vertices[i].X, mesh.Vertices[i].Y, mesh.Vertices[i].Z),
                };

                if (mesh.HasNormals)
                {
                    vert.normal = new OpenTK.Mathematics.Vector3(mesh.Normals[i].X, mesh.Normals[i].Y, mesh.Normals[i].Z);
                }
                if (mesh.HasTextureCoords(0))
                {
                    vert.texCoord = new OpenTK.Mathematics.Vector2(mesh.TextureCoordinateChannels[0][i].X, mesh.TextureCoordinateChannels[0][i].Y);
                }
                else
                {
                    vert.texCoord = new OpenTK.Mathematics.Vector2(0f, 0f);
                }
                verts[i] = vert;
            }

            // 面
            foreach (var face in mesh.Faces)
            {
                foreach (uint indice in face.Indices)
                {
                    indices.Add(indice);
                }
            }

            // 材质
            if (mesh.MaterialIndex >= 0)
            {
                var mat = scene.Materials[mesh.MaterialIndex];
                textures.AddRange(LoadMaterialTextures(mat, TextureType.Diffuse, "texture_diffuse")); // 漫反射
                textures.AddRange(LoadMaterialTextures(mat, TextureType.Specular, "texture_specular")); // 高光
                textures.AddRange(LoadMaterialTextures(mat, TextureType.Normals, "texture_normal")); // 法线
            }

            return new Mesh(verts, indices.ToArray(), textures.ToArray());
        }

        private List<Texture> LoadMaterialTextures(AiMaterial mat, TextureType texType, string typeName)
        {
            List<Texture> textures = new List<Texture>();

            for (int i = 0; i < mat.GetMaterialTextureCount(texType); i++)
            {
                if (mat.GetMaterialTexture(texType, i, out var textureSlot))
                {
                    var hasLoad = false;
                    // 复用已加载过的
                    foreach (var loadTex in m_LoadedTextures)
                    {
                        if (loadTex.path == textureSlot.FilePath)
                        {
                            textures.Add(loadTex);
                            hasLoad = true;
                            break;
                        }
                    }

                    if (hasLoad) continue;

                    Utils.LoadAndSetTexture($"{m_directory}/{textureSlot.FilePath}", out var texID, OTKTexWrapMode.Repeat, OTKTexMinFilter.Linear);
                    var tex = new Texture()
                    {
                        id = texID,
                        type = typeName,
                        path = textureSlot.FilePath,
                    };
                    textures.Add(tex);
                    m_LoadedTextures.Add(tex);
                }
            }

            return textures;
        }
    }
}

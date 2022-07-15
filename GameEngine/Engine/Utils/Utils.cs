using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;


namespace GameEngine.Engine
{
    public static class Utils
    {

        #region 变换

        /// <summary>
        ///   LookAt 矩阵: 看向给定目标的 观察矩阵---世界空间到相机空间的矩阵
        ///     LookAt = [Rx Ry Rz 0]   [1 0 0 -Px]
        ///              [Ux Uy Uz 0] * [0 1 0 -Py]
        ///              [Dx Dy Dz 0]   [0 0 1 -Pz]
        ///              [0  0  0  1]   [0 0 0  1]
        ///     相机空间右向量 R=(Rx, Ry, Rz).  上方向 U=(Ux, Uy, Uz).  Z朝向 D=(Dx, Dy, Dz).  相机位置 P=(Px, Py, Pz)
        /// </summary>
        public static Matrix4 LookAtMatrix(Vector3 initPos, Vector3 targetPos, Vector3 up)
        {
            var forward = (targetPos - initPos).Normalized();
            var cameraRight = Vector3.Cross(up, forward).Normalized();
            var cameraUp = Vector3.Cross(forward, cameraRight).Normalized();

            var mat = Matrix4.Identity;
            mat.Row0 = new Vector4(cameraRight);
            mat.Row1 = new Vector4(cameraUp);
            mat.Row2 = new Vector4(forward);
            return mat * Matrix4.CreateTranslation(-initPos) * Matrix4.Identity;
        }

        /// <summary>
        ///  法线从模型空间到世界空间的矩阵
        ///     是顶点变换矩阵的 逆的转置
        /// </summary>
        /// <param name="localToWorld">顶点从模型空间到世界空间的矩阵</param>
        /// <returns></returns>
        public static Matrix4 NormalToWorldMatrix(Matrix4 localToWorld)
        {
            var mat = localToWorld.Inverted();
            mat.Transpose();
            mat.Row3 = new Vector4(0f, 0f, 0f, 1f);

            return mat;
        }

        #endregion

        #region 贴图

        /// <summary>
        ///  加载图片
        /// </summary>
        public static void LoadTexture(string path, out byte[] pixels, out int width, out int height, out PixelTypeInfo pixelType)
        {
            var img = Image.Load<Rgba32>(path); // SixLabors 从左上角加载像素, OpenGL 从左下角加载像素
            img.Mutate(x => x.Flip(FlipMode.Vertical)); // 上下翻转像素.  适配 OpenGL
            pixels = new byte[4 * img.Width * img.Height];
            img.CopyPixelDataTo(pixels);
            width = img.Width;
            height = img.Height;
            pixelType = img.PixelType;

        }

        /// <summary>
        ///  贴图加载 并绑定到纹理单元上
        /// </summary>
        public static void SetTextureRGBA(string path, out uint texID, int texUnitIndex, TextureWrapMode textureWrap, TextureMinFilter textureFilter)
        {
            GL.GenTextures(1, out texID);
            GL.ActiveTexture(TextureUnit.Texture0 + texUnitIndex);
            GL.BindTexture(TextureTarget.Texture2D, texID);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)textureWrap); // 纹理环绕
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)textureWrap);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)textureFilter); // 纹理过滤
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)textureFilter);

            LoadTexture(path, out var pixels, out var width, out var height, out var pixelType);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, width, height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, pixels);

        }

        public static void LoadAndSetTexture(string path, out uint texID, TextureWrapMode textureWrap, TextureMinFilter textureFilter)
        {
            GL.GenTextures(1, out texID);
            GL.BindTexture(TextureTarget.Texture2D, texID);


            LoadTexture(path, out var pixels, out var width, out var height, out var pixelType);

            if (pixels != null)
            {
                GL.BindTexture(TextureTarget.ProxyTexture2D, texID);
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, width, height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, pixels);
                GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)textureWrap); // 纹理环绕
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)textureWrap);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)textureFilter); // 纹理过滤
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)textureFilter);
            }
            else
            {
                Console.WriteLine($"贴图加载失败:  {path}");
            }
        }

        #endregion

        #region 路径
        public static string SrcRoot
        {
            get
            {
                return $"{Environment.CurrentDirectory}/../../../src";
            }
        }

        public static string ResRoot
        {
            get
            {
                return $"{Environment.CurrentDirectory}/../../../resource";
            }
        }

        public static string TextureRoot
        {
            get
            {
                return $"{ResRoot}/textures";
            }
        }

        public static string ModelRoot
        {
            get
            {
                return $"{ResRoot}/models";
            }
        }

        public static string ShaderRoot
        {
            get
            {
                return $"{SrcRoot}/shader";
            }
        }

        #endregion

    }
}

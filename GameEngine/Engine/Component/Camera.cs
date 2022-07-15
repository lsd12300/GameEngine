using OpenTK.Mathematics;



namespace GameEngine.Engine
{
    public class Camera : Component
    {


        /// <summary>
        ///  创建 看向某处的观察矩阵
        /// </summary>
        public Matrix4 LookAt(Vector3 target)
        {
            return Utils.LookAtMatrix(m_obj.transform.WorldPos, target, Vector3.UnitY);
        }
    }
}

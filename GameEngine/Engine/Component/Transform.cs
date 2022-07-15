using OpenTK.Mathematics;


namespace GameEngine.Engine
{
    public class Transform : Component
    {
        #region 属性

        protected Vector3 m_worldPos = Vector3.Zero;
        public Vector3 WorldPos
        {
            get
            {
                return m_worldPos;
            }
            set
            {
                m_worldPos = value;
                TransformChanged();
            }
        }

        protected Vector3 m_worldScale = Vector3.One;
        public Vector3 WorldScale
        {
            get
            {
                return m_worldScale;
            }
            set
            {
                m_worldScale = value;
                TransformChanged();
            }
        }

        protected Vector3 m_worldRotate = Vector3.Zero;
        public Vector3 WorldRotate
        {
            get
            {
                return m_worldRotate;
            }
            set
            {
                m_worldRotate = value;
                TransformChanged();
            }
        }

        public Matrix4 transform;

        #endregion


        #region 方法

        /// <summary>
        ///  更新变换矩阵
        /// </summary>
        protected void TransformChanged()
        {
            transform = Matrix4.CreateScale(m_worldScale)
                * Matrix4.CreateRotationX(MathHelper.DegreesToRadians(m_worldRotate.X))
                * Matrix4.CreateRotationY(MathHelper.DegreesToRadians(m_worldRotate.Y))
                * Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(m_worldRotate.Z))
                * Matrix4.CreateTranslation(m_worldPos.X, m_worldPos.Y, m_worldPos.Z)
                * Matrix4.Identity;
        }

        #endregion
    }
}

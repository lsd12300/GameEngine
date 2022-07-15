using System;
using System.Collections.Generic;
using System.Text;

namespace GameEngine.Engine
{
    /// <summary>
    ///  渲染器组件
    ///     模型
    ///     材质
    /// </summary>
    public class Renderer : Component
    {
        private Model m_model;
        private Material m_mat;


        public Model Model
        {
            get => m_model;
            set {
                m_model = value;
            }
        }

        public Material Mat
        {
            get => m_mat;
            set
            {
                m_mat = value;
            }
        }

    }
}

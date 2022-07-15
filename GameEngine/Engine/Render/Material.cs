using System;
using System.Collections.Generic;
using System.Text;

namespace GameEngine.Engine
{
    public class Material
    {

        public Shader shader;

        public Material(Shader s)
        {
            shader = s;
        }

        public Material(string vsPath, string fsPath)
        {
            shader = new Shader(vsPath, fsPath);
        }
    }
}

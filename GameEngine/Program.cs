using GameEngine.Engine;



namespace GameEngine
{
    class Program
    {
        static void Main(string[] args)
        {
            var scene = new Scene();

            var camera = new BaseObject("Camera");
            var camComp = camera.AddComponent<Camera>();


            var obj = new BaseObject("Test");
            var render = obj.AddComponent<Renderer>();
            render.Model = new Model($"{Utils.ModelRoot}/cyborg/cyborg.obj");
            render.Mat = new Material($"{Utils.ShaderRoot}/texture.vshader", $"{Utils.ShaderRoot}/texture.fshader");


        }
    }
}

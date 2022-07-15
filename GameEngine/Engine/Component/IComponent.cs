

namespace GameEngine.Engine
{
    public interface IComponent
    {

        /// <summary>
        ///  获取挂载对象的ID
        /// </summary>
        /// <returns></returns>
        public int GetObjID();

        public void OnRemove();

        public void OnAdd(BaseObject obj);
    }
}

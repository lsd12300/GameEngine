using System;
using System.Collections.Generic;
using System.Text;

namespace GameEngine.Engine
{
    public class Scene
    {

        public Scene()
        {
            m_allObjs = new Dictionary<int, BaseObject>();

            CurScene = this;
        }


        #region 场景内对象

        private Dictionary<int, BaseObject> m_allObjs;

        private BaseObject GetObj(int id)
        {
            if (m_allObjs.TryGetValue(id, out var obj)) return obj;
            return null;
        }

        public BaseObject OnCreateObj(BaseObject obj)
        {
            m_allObjs.Add(obj.GetID(), obj);

            return obj;
        }

        #endregion


        #region 唯一ID

        private static int ObjIDCount = 0;

        public static int ObjUnitID()
        {
            return ++ObjIDCount;
        }

        #endregion


        #region 场景管理

        public static Scene CurScene { get; set; }

        #endregion
    }
}

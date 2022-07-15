using System;
using System.Collections.Generic;
using System.Text;

namespace GameEngine.Engine
{
    public class Component : IComponent, IObject
    {

        protected int m_attachedObjID;
        protected int m_id;
        protected BaseObject m_obj;

        #region 接口

        public int GetID()
        {
            return m_id;
        }

        public int GetObjID()
        {
            return m_attachedObjID;
        }

        public void OnAdd(BaseObject obj)
        {
            m_attachedObjID = obj.GetID();
            m_obj = obj;
        }

        public void OnRemove()
        {
        }

        #endregion
    }
}

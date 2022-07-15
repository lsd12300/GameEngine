using System.Collections.Generic;
using OpenTK.Mathematics;
using System;


namespace GameEngine.Engine
{
    /// <summary>
    ///  物体基类
    /// </summary>
    public class BaseObject : IObject
    {

        private int m_id;
        public string m_name;

        public BaseObject()
        {
            Init();
        }

        public BaseObject(string name)
        {
            m_name = name;
            Init();
        }

        private void Init()
        {
            m_id = Scene.ObjUnitID();
            m_comps = new List<IComponent>();

            transform = this.AddComponent<Transform>();

            Scene.CurScene.OnCreateObj(this);
        }

        #region 组件

        public Transform transform { get; private set; }


        private List<IComponent> m_comps;

        public IComponent GetComponent(Type type)
        {
            var c = m_comps.Find(comp =>
            {
                return comp.GetType() == type;
            });
            return c;
        }

        public bool HasComponent(Type type)
        {
            var c = GetComponent(type);
            return c != null;
        }

        public void AddComponent(IComponent comp)
        {
            m_comps.Add(comp);
            comp.OnAdd(this);
        }

        public T AddComponent<T>() where T : Component, new()
        {
            T c = new T();
            m_comps.Add(c);
            c.OnAdd(this);
            return c;
        }

        public void RemoveComponent(IComponent comp)
        {
            var index = m_comps.IndexOf(comp);
            if (index >= 0)
            {
                m_comps.RemoveAt(index);
            }
            comp.OnRemove();
        }

        #endregion

        #region 接口

        public int GetID()
        {
            return m_id;
        }

        #endregion
    }
}

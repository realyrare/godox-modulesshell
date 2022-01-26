using System;
namespace GodOx.ModuleCore
{
    /// <summary>
    /// 模块描述信息
    /// </summary>
    public interface IModuleDescriptor
    {
        /// <summary>
        /// 模块类型
        /// </summary>
        Type ModuleType { get; }
        /// <summary>
        /// 依赖项
        /// </summary>
        IModuleDescriptor[] Dependencies { get; }

        /// <summary>
        /// 实例,只创建一次
        /// </summary>
        object Instance { get; }
    }

    /// <summary>
    /// 模块描述
    /// </summary>
    public class ModuleDescriptor : IModuleDescriptor
    {
        protected object _instance;

        public virtual Type ModuleType { get; protected set; }

        public virtual IModuleDescriptor[] Dependencies { get; protected set; }

        public virtual object Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = Activator.CreateInstance(ModuleType);
                }
                return _instance;
            }
        }

        public ModuleDescriptor(Type moduleType, params IModuleDescriptor[] dependencies)
        {
            ModuleType = moduleType;
            Dependencies = dependencies ?? new ModuleDescriptor[0];
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NetX.AppCore.Extentions
{
    public static class AssemblyExtions
    {
        public static IEnumerable<Type> GetTypesWithAttribute<TAttribute>(this Assembly assembly)
            where TAttribute : Attribute
        {
            return assembly.GetTypes().Where(type => type.GetCustomAttributes(typeof(TAttribute), true).Any());
        }

        public static IEnumerable<Type> GetTypeWithInterface<TInterface>(this Assembly assembly)
        {
            return assembly.GetTypes().Where(type => typeof(TInterface).IsAssignableFrom(type) && type.IsClass);
        }

        /// <summary>
        /// 获取全部程序集
        /// </summary>
        /// <param name="rootAssembly"></param>
        /// <returns></returns>
        private static List<Assembly> GetAllAssemblies(Assembly rootAssembly)
        {
            var assemblies = new List<Assembly> { rootAssembly };
            var checkedAssemblies = new HashSet<string>();
            var assemblyQueue = new Queue<Assembly>(new[] { rootAssembly });

            while (assemblyQueue.Count > 0)
            {
                var assembly = assemblyQueue.Dequeue();
                if (checkedAssemblies.Contains(assembly.FullName))
                    continue;
                checkedAssemblies.Add(assembly.FullName);
                var referencedAssemblies = assembly.GetReferencedAssemblies();
                foreach (var refAssemblyName in referencedAssemblies)
                {
                    try
                    {
                        var refAssembly = Assembly.Load(refAssemblyName);
                        if (!checkedAssemblies.Contains(refAssembly.FullName))
                        {
                            assemblyQueue.Enqueue(refAssembly);
                            assemblies.Add(refAssembly);
                        }
                    }
                    catch
                    {
                        // 忽略加载失败的程序集
                    }
                }
            }

            return assemblies.Distinct().ToList();
        }
    }
}

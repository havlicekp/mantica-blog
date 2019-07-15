using System;
using System.Collections.Generic;
using Mantica.Blog.Data.Contracts;
namespace Mantica.Blog.Data.DocumentDB
{
    /// <summary>
    /// Manages <see cref="Type"/> to counter mapping. Counters are used to generate
    /// incrementing IDs for documents having the specified <see cref="Type"/>.
    /// </summary>
    public class CounterMapper
    {
        private readonly Dictionary<Type, string> map = new Dictionary<Type, string>
        {
            { typeof(Article), "article" }
        };

        /// <summary>
        /// Returns counter name for the <paramref name="obj"/>
        /// </summary>
        /// <typeparam name="T">Type of the object.</typeparam>
        /// <param name="obj">Insnace of the T to return counter for.</param>
        /// <returns>Counter name for the T.</returns>
        public string FromType<T>(T obj)
        {
            Type type = obj.GetType();
            if (map.ContainsKey(type))
            {
                return map[type];
            }
            else
            {
                throw new ArgumentException("No counter defined for the type specified");
            }
        }
    }
}
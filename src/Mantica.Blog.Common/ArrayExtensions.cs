using System;

namespace Mantica.Blog.Common
{
    /// <summary>
    /// Extension methods for arrays.
    /// </summary>
    public static class ArrayExtensions
    {
        /// <summary>
        /// Joins <paramref name="items"/> with the items in <paramref name="target"/> and returns a newly created array. 
        /// </summary>
        /// <typeparam name="T"><see cref="Type"/>Type of the elements in the array.</typeparam>
        /// <param name="target">Source array.</param>
        /// <param name="items">Items to join with <paramref name="target"/></param>
        /// <returns>Newly created array.</returns>
        public static T[] Add<T>(this T[] target, params T[] items)
        {
            // Validate the parameters
            if (target == null)
            {
                target = new T[] { };
            }
            if (items == null)
            {
                items = new T[] { };
            }

            // Join the arrays
            T[] result = new T[target.Length + items.Length];
            target.CopyTo(result, 0);
            items.CopyTo(result, target.Length);
            return result;
        }
    }
}

﻿using System;
using System.Collections.Concurrent;

namespace Projac
{
    /// <summary>
    /// Lists the available concurrent <see cref="SqlProjectionHandlerResolver">resolvers</see>.
    /// </summary>
    public static class ConcurrentResolve
    {
        /// <summary>
        /// Resolves the <see cref="SqlProjectionHandler">handlers</see> that match the type of the message exactly.
        /// </summary>
        /// <param name="handlers">The set of available handlers.</param>
        /// <returns>A <see cref="SqlProjectionHandlerResolver">resolver</see>.</returns>
        public static SqlProjectionHandlerResolver WhenType(SqlProjectionHandler[] handlers)
        {
            return Resolve.WhenType(handlers);
        }

        /// <summary>
        /// Resolves the <see cref="SqlProjectionHandler">handlers</see> to which the message instance is assignable.
        /// </summary>
        /// <param name="handlers">The set of available handlers.</param>
        /// <returns>A <see cref="SqlProjectionHandlerResolver">resolver</see>.</returns>
        public static SqlProjectionHandlerResolver WhenAssignableToType(SqlProjectionHandler[] handlers)
        {
            if (handlers == null)
                throw new ArgumentNullException("handlers");
            var cache = new ConcurrentDictionary<Type, SqlProjectionHandler[]>();
            return message =>
            {
                if (message == null)
                    throw new ArgumentNullException("message");
                SqlProjectionHandler[] result;
                if (!cache.TryGetValue(message.GetType(), out result))
                {
                    result = cache.GetOrAdd(message.GetType(), Array.FindAll(handlers, handler => handler.Message.IsInstanceOfType(message)));
                }
                return result;
            };
        }
    }
}
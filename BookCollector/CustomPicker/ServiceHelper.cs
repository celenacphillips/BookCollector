// <copyright file="ServiceHelper.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.CustomPicker
{
    /// <summary>
    /// ServiceHelper class.
    /// </summary>
    public static class ServiceHelper
    {
        /// <summary>
        /// Gets the current service provider from the MAUI application.
        /// </summary>
        public static IServiceProvider Current =>
            IPlatformApplication.Current?.Services
            ?? throw new InvalidOperationException("Unable to find MAUI service provider.");

        /// <summary>
        /// Gets the service of the specified type from the current service provider.
        /// </summary>
        /// <typeparam name="T">The generic type.</typeparam>
        /// <returns>The service of object of the generic type.</returns>
        public static T? GetService<T>()
        {
            return Current.GetService<T>();
        }
    }
}

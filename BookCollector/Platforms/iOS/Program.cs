// <copyright file="Program.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace BookCollector
#pragma warning restore IDE0130 // Namespace does not match folder structure
{
    using UIKit;

    /// <summary>
    /// Program class.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// This is the main entry point of the application.
        /// </summary>
        /// <param name="args">The arguments.</param>
        public static void Main(string[] args)
        {
            // if you want to use a different Application Delegate class from "AppDelegate"
            // you can specify it here.
            UIApplication.Main(args, null, typeof(AppDelegate));
        }
    }
}

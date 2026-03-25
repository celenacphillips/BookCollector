// <copyright file="AppDelegate.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace BookCollector
#pragma warning restore IDE0130 // Namespace does not match folder structure
{
    using Foundation;

    /// <summary>
    /// AppDelegate class.
    /// </summary>
    [Register("AppDelegate")]
    public class AppDelegate : MauiUIApplicationDelegate
    {
        /// <summary>
        /// Gets the command to create the MauiApp. This method is called by the framework when the application is launched.
        /// </summary>
        /// <returns>The MauiApp.</returns>
        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
    }
}

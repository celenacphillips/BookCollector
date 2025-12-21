// <copyright file="MainApplication.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

using Android.App;
using Android.Runtime;

namespace BookCollector
{
    /// <summary>
    /// Main Application class.
    /// </summary>
    [Application]
    public class MainApplication : MauiApplication
    {
        public MainApplication(IntPtr handle, JniHandleOwnership ownership)
            : base(handle, ownership)
        {
        }

        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
    }
}

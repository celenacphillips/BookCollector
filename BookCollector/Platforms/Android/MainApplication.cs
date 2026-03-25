// <copyright file="MainApplication.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace BookCollector
#pragma warning restore IDE0130 // Namespace does not match folder structure
{
    using Android.App;
    using Android.Runtime;

    /// <summary>
    /// MainApplication class.
    /// </summary>
    [Application]
    public class MainApplication : MauiApplication
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainApplication"/> class.
        /// Disabled IDE0290 warning, since the constructor is required to have
        /// this signature and cannot be converted to a primary constructor.
        /// </summary>
        /// <param name="handle">Java Native Interface (JNI) object for the C# code
        /// to attach to.</param>
        /// <param name="ownership">Indicates who owns the Java Native Interface (JNI)
        /// reference in the handle parameter.</param>
#pragma warning disable IDE0290 // Use primary constructor
        public MainApplication(IntPtr handle, JniHandleOwnership ownership)
#pragma warning restore IDE0290 // Use primary constructor
            : base(handle, ownership)
        {
        }

        /// <summary>
        /// Creates the MauiApp for the application.
        /// </summary>
        /// /// <returns>The created MauiApp.</returns>
        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
    }
}

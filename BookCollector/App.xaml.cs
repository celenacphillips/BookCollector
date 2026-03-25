// <copyright file="App.xaml.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector
{
    using BookCollector.Views.Controls;

    /// <summary>
    /// App class.
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="App"/> class.
        /// </summary>
        public App()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Creates the window for the application. This method is called when the
        /// application starts and can be used to set up the main page or any other
        /// initial UI elements.
        /// </summary>
        /// <param name="activationState">The value of what the window needs to be created.</param>
        /// <returns>The created window.</returns>
        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new FakeSplashScreen());
        }
    }
}

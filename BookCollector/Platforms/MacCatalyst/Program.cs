// <copyright file="Program.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

using UIKit;

namespace BookCollector
{
    public class Program
    {
        // This is the main entry point of the application.
        public static void Main(string[] args)
        {
            // if you want to use a different Application Delegate class from "AppDelegate"
            // you can specify it here.
            UIApplication.Main(args, null, typeof(AppDelegate));
        }
    }
}

// <copyright file="Constants.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

using SQLite;

namespace BookCollector.Data.Database
{
    public class Constants
    {
        public const string DatabaseFilename = "BookCollector.db3";

        public const SQLiteOpenFlags Flags =
                    SQLiteOpenFlags.ReadWrite |
                    SQLiteOpenFlags.Create |
                    SQLiteOpenFlags.SharedCache;

        public static string DatabasePath =>
            Path.Combine(FileSystem.AppDataDirectory, DatabaseFilename);
    }
}

// <copyright file="Constants.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.Data.Database
{
    using SQLite;

    /// <summary>
    /// Constants class.
    /// </summary>
    public class Constants
    {
        /// <summary>
        /// Sets the name of the database file stored on the device.
        /// </summary>
        public const string DatabaseFilename = "BookCollector.db3";

        /// <summary>
        /// Create an enum that defines the flags for opening the database connection.
        /// These flags determine how the database is accessed and used.
        /// </summary>
        public const SQLiteOpenFlags Flags =
                    SQLiteOpenFlags.ReadWrite |
                    SQLiteOpenFlags.Create |
                    SQLiteOpenFlags.SharedCache;

        /// <summary>
        /// Gets the full path of the database file stored on the device.
        /// </summary>
        public static string DatabasePath =>
            Path.Combine(FileSystem.AppDataDirectory, DatabaseFilename);
    }
}

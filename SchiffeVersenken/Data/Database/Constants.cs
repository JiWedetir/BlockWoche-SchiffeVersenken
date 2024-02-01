namespace SchiffeVersenken.Data.Database
{
    internal static class Constants
    {
        internal const string DatabaseFilename = "BattleShipSQLite.db3";

        internal const SQLite.SQLiteOpenFlags Flags =
            // open the database in read/write mode
            SQLite.SQLiteOpenFlags.ReadWrite |
            // create the database if it doesn't exist
            SQLite.SQLiteOpenFlags.Create |
            // enable full atomic commit and rollback
            SQLite.SQLiteOpenFlags.FullMutex;

        internal static string DatabasePath =>
            Path.Combine(FileSystem.AppDataDirectory, DatabaseFilename);
    }
}

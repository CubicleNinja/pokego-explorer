using System;
using System.Data.SQLite;
using System.IO;

namespace PokemonGO.Specialized.SQLite
{
    public class SQLiteLoggerDB
    {
        private SQLiteConnection dbConn;

        public SQLiteLoggerDB()
        {
            if (!File.Exists("pokemonData.sqlite"))
            {
                SetupDatabase();
            }
        }

        private void SetupDatabase()
        {
            SQLiteConnection.CreateFile("pokemonData.sqlite");
            dbConn = new SQLiteConnection("data source=pokemonData.sqlite");

            string createTableQuery = @"CREATE TABLE IF NOT EXISTS [PokemonData] (
                                        [ID] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                                        [PokemonID] NUMERIC NOT NULL,
                                        [PokemonName] TEXT NOT NULL,
                                        [Latitude] NUMERIC NOT NULL,
                                        [Longitude] NUMERIC NOT NULL,
                                        [Expiration] TEXT NOT NULL,
                                        [FirstSeen] TEXT NOT NULL,
                                        unique(PokemonID, Latitude, Longitude, Expiration)
                                        )
                                       ";

            using (dbConn = new SQLiteConnection("data source=pokemonData.sqlite"))
            {
                using (SQLiteCommand cmd = new SQLiteCommand(dbConn))
                {
                    dbConn.Open();
                    {
                        cmd.CommandText = createTableQuery;
                        cmd.ExecuteNonQuery();
                    }
                    dbConn.Close();
                }
            }
        }

        public void AddEntry(int pokemonID,
                             String pokemonName,
                             double latitude,
                             double longitude,
                             String expiration)
        {
            using (dbConn = new SQLiteConnection("data source=pokemonData.sqlite"))
            {
                using (SQLiteCommand cmd = new SQLiteCommand(dbConn))
                {
                    dbConn.Open();
                    {
                        try
                        {
                            cmd.CommandText = String.Format("INSERT OR IGNORE INTO [PokemonData] ([PokemonID], [PokemonName], [Latitude], [Longitude], [Expiration], [FirstSeen]) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', datetime('now', 'localtime'))",
                                                    pokemonID,
                                                    pokemonName,
                                                    latitude,
                                                    longitude,
                                                    expiration);
                            cmd.ExecuteNonQuery();
                        }
                        catch { }
                    }
                    dbConn.Close();
                }
            }
        }
    }
}

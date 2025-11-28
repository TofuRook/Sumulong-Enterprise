using System;
using System.Data.SQLite;
using System.IO;
using System.Windows.Forms;

namespace Sumulong_Enterprise
{
    internal class DatabaseInitializer
    {
        private const string DatabaseFile = "SumulongInventory.db";

        public static void Initialize()
        {
            bool newDb = !File.Exists(DatabaseFile);

            // Create DB file if it doesn't exist
            if (newDb)
            {
                SQLiteConnection.CreateFile(DatabaseFile);
            }

            using var conn = new SQLiteConnection($"Data Source={DatabaseFile};Version=3;");
            conn.Open();

            using var cmd = new SQLiteCommand("PRAGMA journal_mode=WAL;", conn);
            cmd.ExecuteNonQuery();

            CreateTables(conn);
        }

        private static void CreateTables(SQLiteConnection conn)
        {
            string[] tableCommands = new[]
            {
                @"CREATE TABLE IF NOT EXISTS LOCATIONS (
                    LocationID INTEGER PRIMARY KEY AUTOINCREMENT,
                    LocationName TEXT NOT NULL UNIQUE
                );",
                @"CREATE TABLE IF NOT EXISTS SUPPLIERS (
                    SupplierID INTEGER PRIMARY KEY AUTOINCREMENT,
                    SupplierName TEXT NOT NULL UNIQUE
                );",
                @"CREATE TABLE IF NOT EXISTS MOTORCYCLE_MODELS (
                    ModelID INTEGER PRIMARY KEY AUTOINCREMENT,
                    ModelName TEXT NOT NULL UNIQUE
                );",
                @"CREATE TABLE IF NOT EXISTS PARTS (
                    PartID INTEGER PRIMARY KEY AUTOINCREMENT,
                    PartName TEXT NOT NULL,
                    PartNumber TEXT NOT NULL,
                    Brand TEXT NOT NULL,
                    UNIQUE (PartName, PartNumber, Brand)
                );",
                @"CREATE TABLE IF NOT EXISTS INVENTORY (
                    StockID INTEGER PRIMARY KEY AUTOINCREMENT,
                    PartID INTEGER NOT NULL,
                    ModelID INTEGER NOT NULL,
                    SupplierID INTEGER NOT NULL,
                    Quantity INTEGER NOT NULL DEFAULT 0,
                    SRP REAL,
                    WS_Price REAL,
                    InternalCode TEXT,
                    FOREIGN KEY (PartID) REFERENCES PARTS(PartID),
                    FOREIGN KEY (ModelID) REFERENCES MOTORCYCLE_MODELS(ModelID),
                    FOREIGN KEY (SupplierID) REFERENCES SUPPLIERS(SupplierID)
                );",
                @"CREATE TABLE IF NOT EXISTS INVENTORY_LOCATIONS (
                    StockLocationID INTEGER PRIMARY KEY AUTOINCREMENT,
                    StockID INTEGER NOT NULL,
                    LocationID INTEGER NOT NULL,
                    Quantity INTEGER NOT NULL,
                    UnitType TEXT NOT NULL,
                    TransferCode TEXT,
                    TransferDate TEXT,
                    UNIQUE(StockID, LocationID),
                    FOREIGN KEY (StockID) REFERENCES INVENTORY(StockID),
                    FOREIGN KEY (LocationID) REFERENCES LOCATIONS(LocationID)
                );",
                @"CREATE TABLE IF NOT EXISTS STOCK_MOVEMENTS (
                    MovementID INTEGER PRIMARY KEY AUTOINCREMENT,
                    StockID INTEGER NOT NULL,
                    FromLocationID INTEGER,
                    ToLocationID INTEGER,
                    Quantity INTEGER NOT NULL,
                    UnitType TEXT NOT NULL,
                    TransferCode TEXT,
                    DateTime TEXT NOT NULL,
                    User TEXT NOT NULL,
                    MovementType TEXT NOT NULL,
                    FOREIGN KEY (StockID) REFERENCES INVENTORY(StockID),
                    FOREIGN KEY (FromLocationID) REFERENCES LOCATIONS(LocationID),
                    FOREIGN KEY (ToLocationID) REFERENCES LOCATIONS(LocationID)
                );",
                @"CREATE INDEX IF NOT EXISTS idx_PartNumber ON PARTS(PartNumber);",
                @"CREATE INDEX IF NOT EXISTS idx_StockID ON INVENTORY(StockID);"
            };

            foreach (var sql in tableCommands)
            {
                using var cmd = new SQLiteCommand(sql, conn);
                cmd.ExecuteNonQuery();
            }
        }

        public static bool Initialized()
        {
            return File.Exists(DatabaseFile);
        }
    }
}

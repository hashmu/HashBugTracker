using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace HashBugTracker.Models
{
    class SQLiteManager
    {
        #region Members
        SQLiteAsyncConnection db;
        #endregion


        #region Constructors
        public SQLiteManager()
        {
            if (!Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"HashBugTracker"))){
                Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"HashBugTracker"));
            }
            string databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"HashBugTracker\Bugs.db");
            ConnectToDatabase(databasePath);
        }

        public SQLiteManager(string databasePath)
        {
            ConnectToDatabase(databasePath);
        }
        #endregion

        #region Methods
        public async void ConnectToDatabase(string databasePath)
        {
            
            if (!File.Exists(databasePath))
            {
                db = new SQLiteAsyncConnection(databasePath);
                await db.CreateTableAsync<Bug>();
                await db.CreateTableAsync<Feature>();
                Logger.Log("Database Created At " + databasePath);
            }
            else
            {
                db = new SQLiteAsyncConnection(databasePath);
                Logger.Log("Database Connected");
            }
        }

        public async Task AddBug(string label, string description, string notes, Severity severity)
        {
            Bug bug = new Bug()
            {
                Label = label,
                Description = description,
                Notes = notes,
                Severity = (int)severity,
                DateAdded = DateTime.Now
            };
            await db.InsertAsync(bug);
            Logger.Log("Bug Added");
        }

        public async Task AddFeature(string label, string description, string notes, Priority priority)
        {
            Feature feature = new Feature()
            {
                Label = label,
                Description = description,
                Notes = notes,
                Priority = (int)priority,
                Completed = 0,
                DateAdded = DateTime.Now
            };
            await db.InsertAsync(feature);
            Logger.Log("Feature Added");
        }

        public async Task<Bug[]> GetBugs()
        {
            Bug[] results = null;
            if (db != null)
            {
                var query = db.Table<Bug>();
                results = await query.ToArrayAsync();
            }
            Logger.Log("Bugs Refreshed");
            return results;
        }

        public async Task<Feature[]> GetFeatures()
        {
            Feature[] results = null;
            if (db != null)
            {
                var query = db.Table<Feature>();
                results = await query.ToArrayAsync();
            }
            Logger.Log("Features Refreshed");

            return results;
        }

        public async Task DeleteBug(Bug bug)
        {
            await db.DeleteAsync<Bug>(bug.Id);
            Logger.Log("Bug Deleted");
        }

        public async Task Delete(Object entry)
        {
            if (entry is Bug)
            {
                await db.DeleteAsync<Bug>(((Bug)entry).Id);
                Logger.Log("Bug Deleted");
            }
            else if (entry is Feature)
            {
                await db.DeleteAsync<Feature>(((Feature)entry).Id);
                Logger.Log("Feature Deleted");
            }
        }
        #endregion

        public async Task UpdateBug(Bug bug)
        {
            await db.UpdateAsync(bug);
            Logger.Log("Bug Updated");
        }

        public async Task UpdateFeature(Feature feature)
        {
            await db.UpdateAsync(feature);
            Logger.Log("Bug Updated");
        }

        public async Task Update(Object entry)
        {
            await db.UpdateAsync(entry);
            Logger.Log(entry.GetType().Name + " updated");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace HashBugTracker.Models
{
    class SessionManager
    {
        #region Constructors
        public SessionManager()
        {
            Instance = this;
        }
        #endregion


        #region Members
        public SQLiteManager SQLite { get; set; } = new SQLiteManager();
        public static SessionManager Instance;
        #endregion
    }
}

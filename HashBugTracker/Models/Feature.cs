﻿using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace HashBugTracker.Models
{
    public class Feature
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Label { get; set; }
        public string Description { get; set; }
        public int Priority { get; set; }
        public string Notes { get; set; }
        public int Solution { get; set; }
        public DateTime DateAdded { get; set; }
        public int Completed { get; set; }
    }

    public enum Priority
    {
        Low,
        Medium,
        Urgent
    }
}

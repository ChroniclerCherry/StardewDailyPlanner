﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using StardewValley;

namespace DailyPlanner.Framework
{
    class Planner
    {
        private readonly string Filename;
        private StreamReader Reader;
        private List<List<string>> data;
        private List<string> DailyPlan;

        public Planner(int year)
        {
            this.Filename = year.ToString() + ".csv";
            try
            {
                this.Reader = new StreamReader(File.OpenRead(Path.Combine("Mods", "DailyPlanner", "Plans", this.Filename)));
            } catch (FileNotFoundException)
            {
                File.Copy(Path.Combine("Mods", "DailyPlanner", "Plans", "Template.csv"), Path.Combine("Mods", "DailyPlanner", "Plans", this.Filename));
                this.Reader = new StreamReader(File.OpenRead(Path.Combine("Mods", "DailyPlanner", "Plans", this.Filename)));
            }
            
            this.data = new List<List<string>>();

            while (!this.Reader.EndOfStream)
            {
                var line = this.Reader.ReadLine();
                var values = line.Split(',').ToList();

                while (values.Contains("")) { values.Remove(""); }
                values.RemoveAt(0);

                data.Add(values.ToList());
            }

            Reader.Close();
        }

        public override string ToString()
        {
            return data[1][0];
        }

        public void CreateDailyPlan()
        {
            int season = Game1.Date.SeasonIndex + 1;
            int day = Game1.Date.DayOfMonth;

            if (day <= 0) { day = 1; }

            if (day >= 29) { day = 28; }

            int SeasonRow = (season - 1) * 29 + 1;
            int DayRow = SeasonRow + day;

            this.DailyPlan = new List<string>();
            this.DailyPlan.Clear();
            this.DailyPlan = data[0];
            this.DailyPlan.AddRange(data[SeasonRow]);
            this.DailyPlan.AddRange(data[DayRow]);
        }

        public List<string> GetDailyPlan()
        {
            return this.DailyPlan;
        }

        public void CompleteTask(string task)
        {
            this.DailyPlan.Remove(task);
        }
    }
}
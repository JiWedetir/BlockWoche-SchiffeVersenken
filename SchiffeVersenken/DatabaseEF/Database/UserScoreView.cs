﻿namespace SchiffeVersenken.DatabaseEF.Database
{
    public class UserScoreView
    {
        public string Name { get; set; } = string.Empty;
        public int Score { get; set; }
        public string Opponent { get; set; } = string.Empty;
        public bool Won { get; set; }
    }
}
﻿namespace SchiffeVersenken.DatabaseEF.Database
{
    public class User
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;

        public string Salt { get; set; } = string.Empty;

        public DateTime BirthDate { get; set; }

        public Roles Role { get; set; }

        public bool IsActive { get; set; }
    }
}


﻿namespace LegoM.Services.Users
{
    using LegoM.Data;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class UserService : IUserService
    {
        private readonly LegoMDbContext data;

        public UserService(LegoMDbContext data)
        {
            this.data = data;
        }

        public string GetFullName(string userId)
        => this.data.Users
            .Where(x => x.Id == userId)
            .Select(x => x.FullName)
            .FirstOrDefault();
    }
}

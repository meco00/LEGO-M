namespace LegoM.Test.Data
{
    using LegoM.Data.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class Users
    {
        public static IEnumerable<User> GetUsers(int count=5)
        => Enumerable.Range(0, count).Select(i => new User()
        {
           
        });
    }
}

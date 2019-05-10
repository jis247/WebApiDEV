using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EmployeeDataAccess;

namespace WebSampleAPI
{
    public class EmployeeSecurity
    {
        public static bool Login(string username, string password)
        {
            using (PlayDBEntities entities = new PlayDBEntities())
            {
                //if there is a problem in loading entities then right click on employeedatamodel1.edmx and click run custom tools 
                return entities.Users.Any(Users => Users.Username.Equals(username, StringComparison.OrdinalIgnoreCase)
                                         && Users.Password == password);
            }
        }

    }
}
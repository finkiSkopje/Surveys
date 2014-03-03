using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survey.Models
{
    public static class DbContextCreator
    {
        public static ApplicationDbContext createContext()
        {
            var context = new ApplicationDbContext();
            context.Configuration.ValidateOnSaveEnabled = false;

            return context;
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PocketObservatoryServer.Controllers
{
    public class QueryController : Controller
    {
        //GET: /Query/
        public string Index(string query)
        {
            //So, funny story. I have no clue on how to make a server.
            //This is my best shot (at 3am in the morning). lol.
            if (query == null)
            {
                return null;
            }
            return PocketObservatoryLibrary.InterfaceFunctions.BackendInterp(query);
        }
    }
}

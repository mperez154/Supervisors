using Microsoft.AspNetCore.Mvc;
using Supervisors.Interfaces;
using Supervisors.Models;
using System.Collections.Generic;

namespace Supervisors.Controllers
{
    [ApiController]
    [Route("api")]
    public class SupervisorController : ControllerBase
    {
        private readonly IManager _manager;

        public SupervisorController(IManager manager)
        {
            _manager = manager;
        }

        [HttpGet]
        [Route("supervisors")]
        public IEnumerable<string> GetSupervisors()
        {
            // gets the list of managers from AWS endpoint
            List<Manager> managers = _manager.GetManagers().Result;
            
            List<string> formattedManagers = new List<string>();            
            foreach(var manager in managers)
            {
                int output; 
                if(!int.TryParse(manager.Jurisdiction, out output))
                    formattedManagers.Add(string.Format("{0} - {1}, {2}", manager.Jurisdiction, manager.LastName, manager.FirstName));
            }

            formattedManagers.Sort(); 
            return formattedManagers;
        }

        [HttpPost]
        [Route("submit")]
        public NotificationRegistration SubmitSupervisors(NotificationRegistration registration)
        {
            _manager.PostManager(registration);
            return registration;
        }
    }
}

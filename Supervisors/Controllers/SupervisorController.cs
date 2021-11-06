using Microsoft.AspNetCore.Mvc;
using Supervisors.Interfaces;
using Supervisors.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        public async Task<IEnumerable<string>> GetSupervisors()
        {
            // Requirement: When endpoint is hit, a call is made to amazonaws endpoint
            List<Manager> managers = await _manager.GetManagers().ConfigureAwait(false);
            
            List<string> formattedManagers = new();            
            foreach(var manager in managers)
            {
                // Requirement: Numeric jurisdictions should be excluded 
                // Requirement: A list of supervisor strings formatted as "jurisdicion - lastname, firstname"  returned
                if (!int.TryParse(manager.Jurisdiction, out _))
                    formattedManagers.Add(string.Format("{0} - {1}, {2}", manager.Jurisdiction, manager.LastName, manager.FirstName));
            }

            // Requirement: payload should be sorted in alphabetical order first by jurisdiction, then by lastName and firstName
            formattedManagers.Sort(); 
            return formattedManagers;
        }

        [HttpPost]
        [Route("submit")]
        public async Task<NotificationRegistration> SubmitSupervisors(NotificationRegistration registration)
        {
            await Task.Run(() => _manager.PostManager(registration));
            return registration;
        }
    }
}

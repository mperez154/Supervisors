using Supervisors.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Supervisors.Interfaces
{
    public interface IManager
    {
        public Task<List<Manager>> GetManagers();
        public void PostManager(NotificationRegistration registration);
    }
}

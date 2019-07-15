using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeTrackerEtf.Domain;

namespace TimeTrackerEtf.Models
{
    public class ClientInputModel
    {
        public string Name { get; set; }

        internal void MapTo(Client client)
        {
            client.Name = Name;
        }
    }
}

using Coronaverse.IdentityManager.Client.Models;
using Coronaverse.IdentityManager.Shared.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coronaverse.IdentityManager.Client.Events
{
    public class CreateCharacterEvent : ICreateCharacterEvent
    {
        public CreateCharacterStatus Status { get; set; }
        public Character Character { get; set; }
    }
}

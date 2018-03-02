using System;
using System.Collections.Generic;

namespace Covalence.Contracts
{
    public class UserContract
    {
        public UserContract()
        {
            Tags = new List<String>();
            Connections = new List<ConnectionContract>();
        }

        public string Email { get; set; }
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Bio { get; set; }
        public bool IsMentor { get; set; }
        public bool EmailConfirmed { get; set; }
        public ICollection<String> Tags { get; set; }
        public bool NeedsOnboarding { get; set; }
        public ICollection<ConnectionContract> Connections { get; set; }
        public LocationContract Location { get; set; }
    }
}
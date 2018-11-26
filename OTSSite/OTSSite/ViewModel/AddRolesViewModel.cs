using Microsoft.AspNetCore.Identity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OTSSite.ViewModel
{
    public class AddRolesViewModel
    {
        public string UserId { get; set; }
        public bool Administrator { get; set; }
        public bool Moderator { get; set; }
        public bool Author { get; set; }
        public bool Member { get; set; }

        /// <summary>
        /// Access role assignment by name
        /// </summary>
        /// <param name="n">Name of Role</param>
        /// <returns>bool</returns>
        public bool this[string n]
        {
            get
            {
                switch(n)
                {
                    case nameof(Administrator):
                        return Administrator;
                    case nameof(Moderator):
                        return Moderator;
                    case nameof(Author):
                        return Author;
                    case nameof(Member):
                        return Member;
                    default:
                        throw new KeyNotFoundException();
                }
            }
            set
            {
                switch(n)
                {
                    case nameof(Administrator):
                        Administrator = value;
                        break;
                    case nameof(Moderator):
                        Moderator = value;
                        break;
                    case nameof(Author):
                         Author = value;
                        break;
                    case nameof(Member):
                         Member = value;
                        break;
                    default:
                        throw new KeyNotFoundException();
                }
            }
        }

        public string[] AddedRoles
        {
            get
            {
                var addRoles = new List<string>();
                if (Administrator)
                    addRoles.Add(nameof(Administrator));
                if (Moderator)
                    addRoles.Add(nameof(Moderator));
                if (Author)
                    addRoles.Add(nameof(Author));
                if (Member)
                    addRoles.Add(nameof(Member));
                return addRoles.ToArray();
            }
        }

        public string[] RemovedRoles
        {
            get
            {
                var addRoles = new List<string>();
                if (!Administrator)
                    addRoles.Add(nameof(Administrator));
                if (!Moderator)
                    addRoles.Add(nameof(Moderator));
                if (!Author)
                    addRoles.Add(nameof(Author));
                if (!Member)
                    addRoles.Add(nameof(Member));
                return addRoles.ToArray();
            }
        }
    }
}

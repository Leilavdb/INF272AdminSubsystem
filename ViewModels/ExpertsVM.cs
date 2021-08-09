using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace INF272AdminSubsystem.ViewModels
{
    public class ExpertsVM
    {
        public int UserId { get; set; }
        public int ExpertId { get; set; }
        public string UserName { get; set; }
        public string UserSurname { get; set; }
        public string ModuleName { get; set; }
        public string UserPassword { get; set; }
    }
}
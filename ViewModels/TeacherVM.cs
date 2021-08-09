using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace INF272AdminSubsystem.ViewModels
{
    public class TeacherVM
    {
        public int UserId { get; set; }
        public int TeacherId { get; set; }
        public string UserName { get; set; }
        public string UserSurname { get; set; }

        public string UserPassword { get; set; }
        public string SchoolName { get; set; }
    }
}
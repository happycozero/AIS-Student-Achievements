using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Student_Achievements.Classes
{
    public class Record
    {
        public string LabCode { get; set; }
        public string Level { get; set; }
        public string Name { get; set; }
        public string Place { get; set; }
        public string Participants { get; set; }

        public Record(string labCode, string level, string name, string place, string participants)
        {
            LabCode = labCode;
            Level = level;
            Name = name;
            Place = place;
            Participants = participants;
        }
    }
}

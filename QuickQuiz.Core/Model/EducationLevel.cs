using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickQuiz.Core.Model
{
    public enum EducationLevel
    {
        [Description("Lise")]
        HighSchool,

        [Description("Önlisans")]
        AssociatesDegree,

        [Description("Lisans")]
        BachelorsDegree,

        [Description("Master")]
        MastersDegree,

        [Description("Doktora")]
        Doctorate


    }
}

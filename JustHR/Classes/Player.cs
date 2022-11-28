using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace JustHR.Classes
{
    static class Player
    {
        static int professionality { get; set; }
        static int unity { get; set; }
        static int mentality { get; set; }
        static int bossSatisfaction { get; set; }
        public static int Professionality { get { return professionality; } set { professionality = MathHelper.Clamp(value, 0, 100); } }
        public static int Unity { get { return unity; } set { unity = MathHelper.Clamp(value, 0, 100); } }
        public static int Mentality { get { return mentality; } set { mentality = MathHelper.Clamp(value, 0, 100); } }
        public static int BossSatisfaction { get { return bossSatisfaction; } set { bossSatisfaction = MathHelper.Clamp(value, 0, 100); } }
    }
}

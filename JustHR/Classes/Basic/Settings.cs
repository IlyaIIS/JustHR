using System;
using System.Collections.Generic;
using System.Text;

namespace JustHR.Classes.Basic
{
    /// <summary>
    /// Хранилище глобальных настроек игры.
    /// </summary>
    static class Settings
    {
        public static int WindowWidth { get; } = 1024;
        public static int WindowHeight { get; } = 768;
        public static int FPS { get; } = 30;
        public static int DayLengthInSec { get; } = 30;

        public static Dictionary<GradeEnum, int> AvarajeSalary = new Dictionary<GradeEnum, int>
        {
            { GradeEnum.Junior, 50}, //+-30
            { GradeEnum.Middle, 120}, //+-40
            { GradeEnum.Senior, 330} //+-110 
        };

        public static int Professionality { get; set; }
        public static int Unity { get; set; }
        public static int Mentality { get; set; }
        public static int BossSatisfaction { get; set; }

        public static float GlobalVolume { get; } = 0.07f;
        public static float EffectsVolume { get; } = 1f;
    }
}

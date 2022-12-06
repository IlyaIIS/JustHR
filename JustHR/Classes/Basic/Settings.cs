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
            { GradeEnum.Junior, 50},
            { GradeEnum.Middle, 120}, 
            { GradeEnum.Senior, 330} 
        };

        public static float GlobalVolume { get; } = 0.7f;
        public static float EffectsVolume { get; } = 1f;
        public static int DayEndHoud { get; } = 15;
    }
}

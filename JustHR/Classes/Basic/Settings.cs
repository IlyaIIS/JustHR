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
    }
}

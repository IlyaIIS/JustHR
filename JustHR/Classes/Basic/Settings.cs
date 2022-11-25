using System;
using System.Collections.Generic;
using System.Text;

namespace ShaderPack.Classes
{
    /// <summary>
    /// Хранилище глобальных настроек игры.
    /// </summary>
    static class Settings
    {
        public static int WindowWidth { get; } = 1024;
        public static int WindowHeight { get; } = 768;
    }
}

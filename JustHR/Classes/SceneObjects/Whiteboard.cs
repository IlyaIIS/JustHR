using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace JustHR.Classes.SceneObjects
{
    class Whiteboard : ISceneObject
    {
        public float Z { get; set; }

        public float Professionality { get; private set; }
        public float Unity { get; private set; }
        public float Mentality { get; private set; }
        public float BossSatisfaction { get; private set; }

        public void ForceParametersToNormal()
        {
            Professionality += (Player.Professionality - Professionality) / 20;
            Unity += (Player.Unity - Unity) / 20;
            Mentality += (Player.Mentality - Mentality) / 20;
            BossSatisfaction += (Player.BossSatisfaction - BossSatisfaction) / 20;
        }
    }
}

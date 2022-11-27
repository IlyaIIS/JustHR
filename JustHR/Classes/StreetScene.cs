using JustHR.Classes.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework;
using JustHR.Classes.Basic;

namespace JustHR.Classes
{
    class StreetScene : IScene
    {
        public BackgroundsEnum Background { get; } = BackgroundsEnum.Street;
        public Menu Menu { get; }
        public Dictionary<Enum, SoundEffectInstance> SoundEffects;

        public List<ISceneObject> Objects { get; } = new List<ISceneObject>();
        public LinkedList<Particle> Snowflakes = new LinkedList<Particle>();

        public StreetScene(Menu menu, Dictionary<Enum, SoundEffectInstance> soundEffects)
        {
            SoundEffects = soundEffects;
/*            SoundEffectInstance soundEffectInstance = SoundEffects[SoundsEnum.sound_ambient].CreateInstance();
            soundEffectInstance.IsLooped = true;
            soundEffectInstance.Play();*/
            Menu = menu;

            for (int i = 0; i < Settings.WindowHeight; i++)
            {
                DoTick();
            }
        }

        public void DoTick()
        {
            Random rnd = new Random();

            for (int i = 0; i < 1; i++)
            {
                Snowflakes.AddLast(new Particle(new Vector2(rnd.Next(Settings.WindowWidth), 0), new Vector2(0, 1), 2));
            }

            LinkedListNode<Particle> node = Snowflakes.First;
            while(node != null)
            {
                node.Value.Move();

                if (node.Value.Pos.Y > Settings.WindowHeight)
                    Snowflakes.Remove(node);

                node = node.Next;
            }
        }
    }

    class Particle
    {
        public Vector2 Pos { get; set; }
        public Vector2 Speed { get; set; }
        public float Scale { get; }
        public Particle(Vector2 pos, Vector2 speed, float scale)
        {
            Pos = pos;
            Speed = speed;
            Scale = scale;
        }

        public void Move()
        {
            Random rnd = new Random();
            Pos += Speed;
            Speed += new Vector2((float)rnd.NextDouble() - 0.5f, 0);
        }
    }

    enum BackgroundsEnum
    {
        Street,
        Office
    }
}

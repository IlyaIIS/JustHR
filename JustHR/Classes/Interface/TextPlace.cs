using JustHR.Classes.Basic;
using System;
using System.Collections.Generic;
using System.Text;

namespace JustHR.Classes.Interface
{
    class TextPlace : ISceneObject
    {
        private int tick;
        private List<string> speech = new List<string> { "" };
        private int page;
        public bool IsLastPage { get { return page >= speech.Count - 1; } }

        public TextPlace(Controller controller)
        {
            controller.OnMouseButtonReleased += (key, x, y) =>
            {
                if (!IsLastPage)
                    if (x > 900 && y > 700 && x < 1000 && y < 800)
                    {
                        page++;
                        tick = 0;
                    }
            };
        }

        public void BeginSpech(List<string> speech)
        {
            this.speech = speech;
            tick = 0;
            page = 0;
        }

        public string GetText()
        {
            return speech[page].Substring(0, tick);
        }

        public void DoTick()
        {
            if (tick < speech[page].Length)
                tick++;
        }
    }
}
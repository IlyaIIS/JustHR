using System;
using System.Collections.Generic;
using System.Text;

namespace JustHR.Classes.Interface
{
    class Menu
    {
        public Phone Phone { get; }
        public TextPlace TextPlace { get; }

        public Menu(Phone phone, TextPlace textPlace)
        {
            Phone = phone;
            TextPlace = textPlace;
        }
    }
}
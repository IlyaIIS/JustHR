using System;
using System.Collections.Generic;
using System.Text;

namespace JustHR.Classes
{
    class Character
    {
        public HairEnum Hairs { get; }
        public AccessoryEnum Accessory { get; }
        public ClothesEnum Clothes { get; }

        public Character(HairEnum hairs, AccessoryEnum accessory, ClothesEnum clothes)
        {
            Hairs = hairs;
            Accessory = accessory;
            Clothes = clothes;
        }
        //волосы, ?уши, аксесуар, одежда
    }

    enum HairEnum
    {

    }

    enum AccessoryEnum
    {

    }

    enum ClothesEnum
    {

    }
}

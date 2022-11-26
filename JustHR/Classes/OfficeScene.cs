using JustHR.Classes.Basic;
using JustHR.Classes.Interface;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace JustHR.Classes
{
    class OfficeScene : IScene
    {
        public BackgroundsEnum Background { get; } = BackgroundsEnum.Office;
        public Menu Menu { get; }
        public List<ISceneObject> Objects { get; } = new List<ISceneObject>();
        public OfficeScene(int day, List<ISceneObject> objects, Controller controller)
        {
            Objects = objects;
            var buttons = new List<Button>();
            buttons.Add(new Button(ButtonEnum.TakeJob,new Vector2(50, 380), new Vector2(200, 70), controller, () => {
                foreach(ISceneObject objct in Objects)
                {
                    if (objct.GetType() == typeof(Character))
                    {
                        Character character = (Character)objct;
                        if (character.State == CharacterStateEnum.Coming && character.IsAnimationEnded())
                            character.State = CharacterStateEnum.Accepted;
                    }
                }
                Menu.TextPlace.BeginSpech(new List<string> { "" });
            }));
            buttons.Add(new Button(ButtonEnum.Reject, new Vector2(50, 480), new Vector2(200, 70), controller,() => {
                foreach (ISceneObject objct in Objects)
                {
                    if (objct.GetType() == typeof(Character))
                    {
                        Character character = (Character)objct;
                        if (character.State == CharacterStateEnum.Coming && character.IsAnimationEnded())
                            character.State = CharacterStateEnum.Rejected;
                    }
                }
                Menu.TextPlace.BeginSpech(new List<string> { "" });
            }));
            buttons.Add(new Button(ButtonEnum.CallBack, new Vector2(50, 580), new Vector2(200, 70), controller, () => {
                foreach (ISceneObject objct in Objects)
                {
                    if (objct.GetType() == typeof(Character))
                    {
                        Character character = (Character)objct;
                        if (character.State == CharacterStateEnum.Coming && character.IsAnimationEnded())
                            character.State = CharacterStateEnum.Rejected;
                    }
                }
                Menu.TextPlace.BeginSpech(new List<string> { "" });
            }));
            Phone phone = new Phone(buttons);

            TextPlace textPlace = new TextPlace(controller);
            Menu = new Menu(phone, textPlace);

            for (int i = 0; i < Objects.Count; i++)
            {
                if (Objects[i] == null)
                {
                    Character character = new Character(new Vector2(-190, 0), textPlace, HairEnum.Type1, AccessoryEnum.Type1, ClothesEnum.Type1, this, 
                        new List<string> { "А теперь о работе. Мы будем присылать вам каждый день разное количество сотрудников, которых нужно будет принять. Всю вашу работу вы будете вести через ваш телефон. Кол-во людей для интервью вы будете видеть в телефоне",
                        "Мы бы хотели чтобы вы отработали с нами 5 рабочих дней с 9 до 6 вечера. С 27 декабря по 31 декабря включительно."});
                    Objects[i] = character;
                }
            }
        }

        public void GenerateNewCharacter()
        {
            for (int i = 0; i < Objects.Count; i++)
            {
                if (Objects[i].GetType() == typeof(Character))
                {
                    Vector2 pos = new Vector2(-190, 0);
                    HairEnum hair = HairEnum.Type1;
                    AccessoryEnum accessory = AccessoryEnum.Type1;
                    ClothesEnum clothes = ClothesEnum.Type1;
                    List<string> speech = new List<string> { "" };
                    Character character = new Character(pos, Menu.TextPlace, hair, accessory, clothes, this, speech);
                    Objects[i] = character;
                }
            }
        }
    }
}
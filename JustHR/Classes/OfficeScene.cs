using JustHR.Classes.Basic;
using JustHR.Classes.Interface;
using JustHR.Classes.SceneObjects;
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
        public int Day { get; }
        public int Hour { get; private set; } = 9;
        public OfficeScene(int day, Controller controller)
        {
            Objects.Add(new Whiteboard());
            Objects.Add(new Garland());
            Objects.Add(new Door());
            Objects.Add(new ChristmasTree());
            Objects.Add(new Calendar());
            Objects.Add(new Clock());
            Objects.Add(new Cooler());
            Objects.Add(new BackChair());
            Objects.Add(new SideChair());
            Objects.Add(null);
            Objects.Add(new Table());
            Objects.Add(new CurriculumVitae(this, controller));


            Day = day;
            var buttons = new List<Button>();
            buttons.Add(new Button(ButtonEnum.TakeJob,new Vector2(50, 380), new Vector2(200, 70), controller, () => {
                CurriculumVitae cv = GetCV();
                Character character = GetCharacter();
                if (character.IsSitting())
                {
                    character.State = CharacterStateEnum.Accepted;
                    if (cv.IsExpanded)
                        cv.IsExpanded = false;
                }
                Menu.TextPlace.BeginSpeech(new List<string> { "" });
            }));
            buttons.Add(new Button(ButtonEnum.Reject, new Vector2(50, 480), new Vector2(200, 70), controller,() => {
                CurriculumVitae cv = GetCV();
                Character character = GetCharacter();
                if (character.IsSitting())
                {
                    character.State = CharacterStateEnum.Rejected;
                    if (cv.IsExpanded)
                        cv.IsExpanded = false;
                }
                Menu.TextPlace.BeginSpeech(new List<string> { "" });
            }));
            buttons.Add(new Button(ButtonEnum.CallBack, new Vector2(50, 580), new Vector2(200, 70), controller, () => {
                CurriculumVitae cv = GetCV();
                Character character = GetCharacter();
                if (character.IsSitting())
                {
                    character.State = CharacterStateEnum.Rejected;
                    if (cv.IsExpanded)
                        cv.IsExpanded = false;
                }
                Menu.TextPlace.BeginSpeech(new List<string> { "" });
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

            CurriculumVitae GetCV()
            {
                foreach (ISceneObject objct in Objects)
                    if (objct.GetType() == typeof(CurriculumVitae))
                        return (CurriculumVitae)objct;

                throw new Exception("CV not found");
            }
        }

        public void NextHour()
        {
            Hour++;
        }

        public Character GetCharacter()
        {
            foreach (ISceneObject objct in Objects)
                if (objct.GetType() == typeof(Character))
                    return (Character)objct;

            throw new Exception("Character not found");
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
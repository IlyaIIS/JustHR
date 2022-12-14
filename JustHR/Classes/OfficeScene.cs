using JustHR.Classes.Basic;
using JustHR.Classes.Interface;
using JustHR.Classes.SceneObjects;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using System.Linq;

namespace JustHR.Classes
{
    class OfficeScene : IScene
    {
        public BackgroundsEnum Background { get; } = BackgroundsEnum.Office;
        public Menu Menu { get; }
        public Dictionary<Enum, SoundEffectInstance> SoundEffects;
        public OfficeSceneObjectsHolder Objects { get; }

        public int Day { get; }
        public int Hour { get; private set; } = 9;

        public Dictionary<ProfessionEnum, int> Requirements { get; } = new Dictionary<ProfessionEnum, int>();
        public Dictionary<ProfessionEnum, int> HairedRatio { get; } = new Dictionary<ProfessionEnum, int>();

        public bool IsDayEnded { get; private set; } = false;
        public bool IsGameOver { get; private set; } = false;
        public bool WasSecondChanceUsed { get; private set; } = false;

        public BossState BossState { get; private set; }

        public LinkedList<Character> CharactersQueue { get; } = new LinkedList<Character>();
        public List<Character> RecallCharacters { get; } = new List<Character>();

        public ISceneObject SelectedObject { get; set; }

        public OfficeScene(int day, Controller controller, Dictionary<Enum, SoundEffectInstance> soundEffects)
        {
            SoundEffects = soundEffects;

            Objects = new OfficeSceneObjectsHolder
            {
                CurriculumVitae = new CurriculumVitae(this, controller, SoundEffects) { Z = 0.8f }, //0.8f;0.99f
                Table = new Table() { Z = 0.7f },
                Cat = new Cat(SoundEffects) { }, //0.9f;0.31f;0.51f;
                SideChair = new SideChair() { Z = 0.5f },
                ChristmasTree = new ChristmasTree() { Z = 0.4f },
                BackChair = new BackChair() { Z = 0.3f },
                Cooler = new Cooler(SoundEffects) { Z = 0.3f },
                Garland = new Garland() { Z = 0.3f },
                Whiteboard = new Whiteboard() { Z = 0.2f },
                Clock = new Clock(SoundEffects) { Z = 0.2f },
                Calendar = new Calendar() { Z = 0.2f },
                Exit = new Exit() { Z = 0.2f },
                Door = new Door(this, controller) { Z = 0.1f },
            };

            Day = day;
            var buttons = new List<Button>();
            buttons.Add(new Button(ButtonEnum.AcceptButton, new Vector2(35, 475), new Vector2(245, 50), controller, () =>
            {
                SoundEffects[SoundsEnum.phone_btn_sound_2].Play();
                
                Character character = Objects.Character;
                if (character.IsSitting())
                {
                    character.Accept();

                    Objects.CurriculumVitae.IsExpanded = false;

                    Menu.TextPlace.Clear();

                    if (!character.Traits.IsBoss)
                    {
                        Player.Professionality += character.Traits.Professionality;
                        Player.Unity += character.Traits.SocialIntelligence;
                        Player.Mentality += character.Traits.HairMoraleImpact;
                        Player.BossSatisfaction += (int)(10 * (Settings.AvarajeSalary[character.Traits.Grade] - character.Traits.RequiredSalary) / (Settings.AvarajeSalary[character.Traits.Grade] * 0.3f));

                        HairedRatio[character.Traits.Profession]++;
                    }
                }

            }));
            buttons.Add(new Button(ButtonEnum.RejectButton, new Vector2(35, 530), new Vector2(245, 50), controller, () =>
            {
                SoundEffects[SoundsEnum.phone_btn_sound_2].Play();

                Character character = this.Objects.Character;
                if (character.IsSitting())
                {
                    if (!character.Traits.IsBoss)
                        character.Reject();
                    else
                        character.Accept();

                    Objects.CurriculumVitae.IsExpanded = false;

                    Menu.TextPlace.Clear();

                    Player.Mentality += character.Traits.RejectMaraleImpact;
                }

            }));
            buttons.Add(new Button(ButtonEnum.RecallButton, new Vector2(35, 585), new Vector2(245, 50), controller, () =>
            {
                SoundEffects[SoundsEnum.phone_btn_sound_2].Play();

                Character character = this.Objects.Character;
                if (character.IsSitting())
                {
                    if (!character.Traits.IsBoss)
                        character.Reject();
                    else
                        character.Accept();

                    Objects.CurriculumVitae.IsExpanded = false;

                    Menu.TextPlace.Clear();

                    Player.Mentality += character.Traits.RejectMaraleImpact / 2;

                    if (!character.Traits.IsBoss)
                        RecallCharacters.Add(character);
                }

            }));
            buttons.Add(new Button(ButtonEnum.JobResponsibilitiesButton, new Vector2(35, 640), new Vector2(245, 50), controller, () =>
            {
                SoundEffects[SoundsEnum.phone_btn_sound_2].Play();

                Character character = this.Objects.Character;
                if (character.IsSitting())
                {
                    if (!character.Traits.IsBoss)
                        character.Reject();
                    else
                        character.Accept();

                    Objects.CurriculumVitae.IsExpanded = false;

                    Menu.TextPlace.Clear();

                    Player.Mentality += character.Traits.RejectMaraleImpact / 2;

                    if (!character.Traits.IsBoss)
                        RecallCharacters.Add(character);
                    BossState = BossState.Help;
                }
            }));
            Phone phone = new Phone(buttons);
            TextPlace textPlace = new TextPlace(controller);
            Menu = new Menu(phone, textPlace);

            GenerateRequirements();

            Objects.Character = GetBoss();
            Objects.Character.Start();

            for (int i = 0; i < 4; i++)
            {
                CharactersQueue.AddLast(GetNewCharacter());
            }

            controller.OnMouseMoving += (lastPos, newPos) =>
            {
                List<ISceneObject> selectedObjects = new List<ISceneObject>();
                if (!Objects.CurriculumVitae.IsExpanded)
                    foreach (ISceneObject obj in Objects.ObjectsCollection)
                        if (obj is IClickable clObj)
                            if (clObj.Collision.Contains(newPos))
                                selectedObjects.Add(obj);

                if (selectedObjects.Count > 0)
                {
                    SelectedObject = selectedObjects[0];
                    foreach (ISceneObject obj in selectedObjects)
                        if (obj.Z > SelectedObject.Z)
                            SelectedObject = obj;
                }
                else
                {
                    SelectedObject = null;
                }

                if ((new Rectangle(400, 180, 255, 135)).Contains(newPos) && Objects.Character.IsSitting())
                    Objects.Character.Alpha = 100;
                else
                    Objects.Character.Alpha = 255;
            };

            controller.OnMouseButtonReleased += (button, x, y) =>
            {
                if (button == MouseButton.LeftButton)
                    foreach (ISceneObject obj in Objects.ObjectsCollection)
                        if (obj is IClickable clObj)
                            if (clObj.Collision.Contains(x, y))
                                clObj.Click(this);
            };
        }

        private void GenerateRequirements()
        {
            Random rnd = new Random();
            List<int> randomNums = new List<int>();
            do
            {
                int rndNum = rnd.Next(Enum.GetValues(typeof(ProfessionEnum)).Length);
                if (!randomNums.Contains(rndNum))
                    randomNums.Add(rndNum);
            } while (randomNums.Count < 3);

            int dayDif = (Day - 27);
            Requirements.Add((ProfessionEnum)randomNums[0], 2 + dayDif + rnd.Next(1));
            Requirements.Add((ProfessionEnum)randomNums[1], 2 + rnd.Next((dayDif / 2), dayDif));
            Requirements.Add((ProfessionEnum)randomNums[2], 1 + rnd.Next(dayDif / 2));

            HairedRatio.Add((ProfessionEnum)randomNums[0], 0);
            HairedRatio.Add((ProfessionEnum)randomNums[1], 0);
            HairedRatio.Add((ProfessionEnum)randomNums[2], 0);
        }

        public void NextHour()
        {
            if (Hour < 18)
            {
                Hour++;
                Objects.Cooler.CoffeEffectiveness = Objects.Cooler.CoffeEffectiveness + 0.1f;

                if (Objects.Cat.IsAngry)
                    Objects.Cat.AngryWeight--;

                Random rnd = new Random();
                if (Hour <= Settings.DayEndHoud)
                    for (int i = 0; i < rnd.Next(2)+1; i++)
                        if (CharactersQueue.Count < 12)
                            CharactersQueue.AddLast(GetNewCharacter());

                if (Objects.Character.IsExited())
                    CharacterExited();
            }
        }

        public void EndDay()
        {
            Character character = Objects.Character;
            if (!character.Traits.IsBoss && character.IsSitting())
            {
                character.Reject();
                if (Objects.CurriculumVitae.IsExpanded)
                    Objects.CurriculumVitae.IsExpanded = false;
                Menu.TextPlace.Clear();
            }
        }

        public void CharacterExited()
        {
            if (Objects.Character.Traits.IsBoss) //босс вышел после оглашения проигрыша
                if (Player.Professionality <= 0 ||
                    Player.Unity <= 0 ||
                    Player.BossSatisfaction <= 0)
                {
                    GameOver();
                }

            if (Hour < 18 &&
                Player.Professionality > 0 &&
                Player.Unity > 0 &&
                Player.BossSatisfaction > 0 &&
                BossState == BossState.Default)
            {
                if ((Hour < Settings.DayEndHoud || RecallCharacters.Count == 0) && CharactersQueue.Count > 0)
                {
                    Character character = CharactersQueue.First();
                    CharactersQueue.RemoveFirst();
                    Objects.Character = character;
                    Objects.Character.Start();
                }
                else if (RecallCharacters.Count > 0)
                {
                    Character character = PopRndRecallCharacter();
                    Objects.Character = character;
                    Objects.Character.Start();
                }
            }
            else
            {
                Character character = GetBoss();
                Objects.Character = character;
                Objects.Character.Start();
            }
        }

        public void Exit()
        {
            if (Player.BossSatisfaction <= 0)
                GameOver();
            else
                IsDayEnded = true;
        }

        public Character PopRndRecallCharacter()
        {
            Random rnd = new Random();
            Character character = RecallCharacters[rnd.Next(RecallCharacters.Count)];
            RecallCharacters.Remove(character);
            return character;
        }

        public Character GetNewCharacter()
        {
            //TODO: переделать нахер механику спича
            var speeches = new List<List<string>>
            {new List<string>
                {
                    "Здравствуйте, На прежнем месте работы я занимался... решал... выполнял... проводил... руководил... В этом мне помогали знания... и... Кроме того, я умею...",
                " За время работы я добился положительных результатов: количество клиентов выросло на...",
                    "степень удовлетворенности клиентов обслуживанием  возросла на... Мое предложение по оптимизации... сэкономило компании...",
                },new List<string>
                {
                    "Здравствуйте, привет родственик, как ты тут? Надеюсь, мы сработаемся на прежнем месте работы я занимался... решал... выполнял... проводил...",
                    " руководил... В этом мне помогали знания... и... Кроме того, я умею... За время работы я добился положительных результатов: количество клиентов выросло на...",
                    "степень удовлетворенности клиентов обслуживанием возросла на... Мое предложение по оптимизации... сэкономило компании...",
                },new List<string>
                {
                    "В компании на момент ухода у меня не было возможности развиваться как профессионалу. В течение... лет я выполнял примерно одни и те же обязанности и отлично с ними справлялся. Например... ",
                    "Работа и продукт мне нравились, отношения с коллегами были отличные, и я верил, что компания использует не все возможности на нашем рынке. Прошел профессиональное переобучение на...",
                    " и предложил руководству идеи по развитию бизнеса, был готов взять на себя новые задачи и ответственность, но руководству это было не нужно. Я осознал, что достиг потолка",
                },new List<string>
                {
                    "В компании на момент ухода у меня не было возможности развиваться как профессионалу. В течение... лет я выполнял примерно одни и те же обязанности и отлично с ними справлялся. Например... ",
                    "Работа и продукт мне нравились, отношения с коллегами были отличные, и я верил, что компания использует не все возможности на нашем рынке. Прошел профессиональное переобучение на...",
                    " и предложил руководству идеи по развитию бизнеса, был готов взять на себя новые задачи и ответственность, но руководству это было не нужно. Я осознал, что достиг потолка. Надеюсь, что мы сработаемся с тобой мой родственик",
                },new List<string>
                {
                    "Я очень обрадовался, когда увидел эту вакансию. Мне нравятся ваши продукты, особенно... себе такой взял и был бы рад всем знакомым продать. Я вижу, что компания активно растет, планирует выйти на рынки... - а у меня как раз есть соответствующий опыт и связи... ",
                    "Кроме того, я верю, что лучше всего человек работает на своем месте, а я идеально подхожу для этой должности, потому что так давно занимаюсь продажей продуктов... , знаю о них все, и они мне даже во сне снятся"
                },new List<string>
                {
                    "Я очень обрадовался, когда увидел эту вакансию. Думаю это вакансия уже моя, ведь да родственник?Мне нравятся ваши продукты, особенно... себе такой взял и был бы рад всем знакомым продать.",
                    " Я вижу, что компания активно растет, планирует выйти на рынки... - а у меня как раз есть соответствующий опыт и связи... ",
                    "Кроме того, я верю, что лучше всего человек работает на своем месте, а я идеально подхожу для этой должности, потому что так давно занимаюсь продажей продуктов... , знаю о них все, и они мне даже во сне снятся"
                },new List<string>
                {
                    "В компании сложилась ситуация, при которой... Это создавало следующую проблему... Руководство поставило цель...",
                    " Я предложил несколько гипотез и вариантов решений. Принялся тестировать их одну за другой. ",
                    "Гипотеза... оказалась верной, поэтому я провел следующие мероприятия.... В результате... - и цель была достигнута. Считаю, что причиной успешного исхода дела стало то, что я... , умею... , знаю..."
                },new List<string>
                {
                    "В компании сложилась ситуация, при которой... Это создавало следующую проблему... Руководство поставило цель... Я предложил несколько гипотез и вариантов решений. Принялся тестировать их одну за другой. ",
                    "Гипотеза... оказалась верной, поэтому я провел следующие мероприятия... . В результате... - и цель была достигнута. Считаю, что причиной успешного исхода дела стало то, что я... , умею... , знаю..., надеюсь мы сработаемся друг"
                }
            };

            var speeches_num = new List<int> { 0, -5, 0, -5, 0, -5, 0, -5 };
            var full_name = new List<string>
            {
                "Мухин Артем Сергеевич",
"Евдокимова Кристина Александровна",
"Смирнов Роман Святославович",
"Николаев Глеб Михайлович",
"Назаров Александр Артемович",
"Филиппов Александр Максимович",
"Федоров Марк Георгиевич",
"Смирнов Семен Платонович",
"Кузьмин Дмитрий Олегович",
"Смирнов Артем Тимурович",
"Кузьмин Павел Ильич",
"Литвинова Ксения Александровна",
"Иванов Александр Александрович",
"Жданова Елизавета Романовна",
"Наумов Иван Никитич",
"Меркулова Милана Александровна",
"Назарова Анастасия Дмитриевна",
"Свиридов Даниил Васильевич",
"Осипова Полина Яковлевна",
"Никифорова Александра Александровна",
"Завьялов Федор Михайлович",
"Трифонов Захар Глебович",
"Григорьева Дарья Степановна",
"Карпов Ярослав Семенович",
"Авдеев Илья Егорович",
"Иванова Варвара Дмитриевна",
"Громова Нина Романовна",
"Воронцова Александра Егоровна",
"Копылова Дарья Александровна",
"Усова Мария Глебовна",
"Колпакова София Кирилловна",
"Васильев Артем Максимович",
"Вешнякова Ольга Тимуровна",
"Шевелева Софья Львовна",
"Федосеева Виктория Максимовна",
"Савин Егор Романович",
"Лавров Владислав Сергеевич",
"Озеров Ярослав Денисович",
"Устинова Варвара Данииловна",
"Емельянов Матвей Романович",
"Миронова Ева Матвеевна",
"Горбунов Семен Всеволодович",
"Горохова Ева Александровна",
"Чернышева Елизавета Евгеньевна",
"Константинова Анна Михайловна",
"Меркулова Амалия Григорьевна",
"Карташова Софья Александровна",
"Наумов Дмитрий Ярославович",
"Терехова Алина Матвеевна",
"Тарасов Вадим Петрович",
"Филиппова Марьям Ильинична",
"Иванов Константин Егорович",
"Богомолова Таисия Сергеевна",
"Волкова Мария Александровна",
"Степанова Арина Егоровна",
"Иванов Семен Артемович",
"Чернышева Наталья Святославовна",
"Мухин Артем Романович",
"Попов Никита Сергеевич",
"Беляев Петр Робертович",
"Малышева София Романовна",
"Лебедев Даниил Марсельевич",
"Смирнов Илья Святославович",
"Иванов Михаил Никитич",
"Абрамов Михаил Егорович",
"Михайлова Милана Кирилловна",
"Горбачева Валерия Денисовна",
"Князева Софья Максимовна",
"Семенов Давид Артемович",
"Романов Кирилл Романович",
"Васильев Даниил Михайлович",
"Королева Татьяна Ивановна",
"Еремина Дарья Михайловна",
"Чернышева Вера Лукинична",
"Ильин Никита Маркович",
"Корнева Полина Макаровна",
"Черная Елизавета Данииловна",
"Жукова Маргарита Ивановна",
"Калашникова Арина Давидовна",
"Абрамов Тимофей Захарович",
"Бурова Алиса Никитична",
"Трошин Ярослав Кириллович",
"Серов Александр Михайлович",
"Симонов Даниил Глебович",
"Попов Артем Степанович",
"Сергеева Вероника Павловна",
"Кузнецова Полина Тимофеевна",
"Кузьмина Антонина Григорьевна",
"Калмыкова Валерия Даниловна",
"Крылов Иван Александрович",
"Волков Михаил Ильич",
"Степанова Мирослава Мироновна",
"Богданов Вячеслав Дмитриевич",
"Крюков Давид Иванович",
"Ильина Майя Ярославовна",
"Свиридова Ангелина Александровна",
"Мальцева Дарья Александровна",
"Майоров Тимофей Артемович",
"Аксенов Тимофей Романович",
"Артамонова Ариана Максимовна",
"Пирогов Илья Денисович",
"Королев Михаил Матвеевич",
"Белова Ева Семеновна",
"Воронов Владимир Демьянович",
"Софронов Савелий Богданович",
"Симонова Кристина Артемовна",
"Малинин Эмин Даниилович",
"Шишкин Александр Борисович",
"Яковлева Алина Михайловна",
"Панова Марьям Марковна",
"Киселева Ангелина Давидовна",
"Морозова Маргарита Тимофеевна",
"Павлова Елизавета Алексеевна",
"Васильев Никита Михайлович",
"Соколов Руслан Германович",
"Коровина Варвара Ивановна",
"Соколова Виктория Федоровна",
"Фролова Анастасия Арсентьевна",
"Пирогов Мирослав Дмитриевич",
"Олейникова Александра Максимовна",
"Ильина Николь Егоровна",
"Нефедова Аглая Глебовна",
"Воронина Виктория Данииловна",
"Лаврентьев Артем Никитич",
"Пантелеева Мария Станиславовна",
"Кондратьев Ярослав Дмитриевич",
"Елисеева Ульяна Львовна",
"Попов Эмин Гордеевич",
"Васильева Мария Артемовна",
"Зуева Евдокия Леоновна",
"Измайлова Алия Максимовна",
"Шувалова София Ильинична",
"Назарова Алиса Викторовна",
"Щербакова Анастасия Владимировна",
"Матвеев Максим Максимович",
"Воробьев Марк Максимович",
"Ермакова Алиса Андреевна",
"Зорин Александр Никитич",
"Петровская Стефания Михайловна",
"Дроздов Александр Никитич",
"Козырева Ульяна Артемьевна",
"Иванова Дарина Тимофеевна",
"Хохлов Иван Арсентьевич",
"Лебедев Тимур Всеволодович",
"Бочаров Эмин Максимович",
"Кузнецова Полина Алексеевна",
"Калинина Алина Савельевна",
"Миронова Елизавета Тимофеевна",
"Семин Марк Матвеевич",
"Новикова Анастасия Федоровна",
"Журавлев Давид Савельевич",
"Белоусов Дмитрий Евгеньевич",
"Макеев Артем Вадимович",
"Кузнецова Арина Егоровна",
"Ларина Мария Олеговна",
"Глухова Вера Артемовна",
"Боброва Виктория Марковна",
"Токарев Николай Семенович",
"Симонова Нина Федоровна",
"Соловьев Кирилл Львович",
"Кузнецова Ольга Эминовна",
"Борисова София Алексеевна",
"Крюкова Асия Денисовна",
"Новикова София Демидовна",
"Кольцов Максим Адамович",
"Мартынова Любовь Ивановна",
"Титов Артур Кириллович",
"Степанова Варвара Мирославовна",
"Давыдов Александр Михайлович",
"Коротков Ярослав Ибрагимович",
"Рожков Константин Богданович",
"Чернов Роман Ильич",
"Ларионов Егор Никитич",
"Кириллова Арина Тимуровна",
"Власова София Максимовна",
"Громова Юлия Дмитриевна",
"Тимофеев Даниил Даниилович",
"Герасимова Пелагея Данииловна",
"Кукушкин Адам Никитич",
"Павлова Полина Сергеевна",
"Свешников Тимофей Егорович",
"Трофимова Милана Артемьевна",
"Панова Анастасия Дмитриевна",
"Волкова Варвара Платоновна",
"Кольцова Дарья Всеволодовна",
"Никитин Тимофей Святославович",
"Кузьмин Иван Александрович",
"Алексеев Максим Алексеевич",
"Рябов Ярослав Тимурович",
"Сергеева Мария Федоровна",
"Яковлев Александр Артемович",
"Иванов Павел Тимофеевич",
"Лапшин Даниил Александрович",
"Дьякова Анна Никитична",
"Никитин Юрий Артемович",
"Дмитриев Даниэль Серафимович",
"Куприянов Лев Вадимович",
"Зотова Татьяна Петровна",
"Олейников Мирослав Георгиевич",
"Королева Елизавета Олеговна",
            };

            Random rnd = new Random();

            Vector2 pos = new Vector2(0, 185);
            int eyes = rnd.Next(5);
            int hair = rnd.Next(4);
            int accessory = rnd.Next(3) - 1;

            int clothNum = rnd.Next(5);

            int rndD = rnd.Next(full_name.Count);

            string firstName = full_name[rndD]; //получать из вне
            string lastName = "";
            string patronumic = "";
            string birthday = (rnd.Next(29) + 1) + "." + (rnd.Next(12) + 1) + "." + (rnd.Next(-10, 11) + 1992);

            ProfessionEnum professtion = new List<ProfessionEnum>(Requirements.Keys)[rnd.Next(3)];
            int[] array = new int[] { 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 3, 3 };
            GradeEnum grade = (GradeEnum)array[rnd.Next(array.Length)] - 1;

            int professionality = rnd.Next(-10, 11) + (int)grade - rnd.Next((int)(Player.Professionality / 50f - 1) * 4);
            int socialIntelligence = rnd.Next(-10, 11) - (int)grade - rnd.Next((int)(Player.Unity / 50f - 1) * 4);

            //
            int index = rnd.Next(speeches.Count);
            //

            int hairMoraleImpact = speeches_num[index]; //получать из вне
            int rejectMoraleImpact = -5;

            int requiredSalary = Settings.AvarajeSalary[grade];
            requiredSalary = (int)(requiredSalary * (1 + ((float)rnd.NextDouble() - 0.5) * 2 * 0.3f));

            List<string> speech = new List<string>(); //получать из вне
            speech = speeches[index];
            Character character = new Character(pos, Menu.TextPlace, this, clothNum,
                new CharacterTraits(false, firstName, lastName, patronumic, birthday,
                eyes, hair, accessory,
                professtion, grade,
                professionality, socialIntelligence, hairMoraleImpact, rejectMoraleImpact,
                requiredSalary, speech), SoundEffects);

            return character;
        }

        private Character GetBoss()
        {
            //начало дня
            if (!Objects.Contains(typeof(Character)))
            {
                var text2 = new List<string>
                    {
                        "Здравствуйте, мы очень рады что вы согласились работать с нами в эти новогодние праздники.",
"Мы бы хотели чтобы вы отработали с нами 5 рабочих дней с 9 до 6 вечера. С 27 декабря по 31 декабря включительно.",
"Я понимаю, что это не слишком этично, но мы все вам компенсируем.",
"А теперь о работе. Мы будем присылать вам каждый день разное количество сотрудников, которых нужно будет принять. Всю вашу работу вы будете вести через ваш телефон. Кол-во людей для интервью вы будете видеть в телефоне.",
"Прозрачный фон - вы еще не интервьюировали этого человека. Если зеленый, вы приняли этого человека, если желтый то он вернется позже для повторного интервью. Красный, вы отказали ему.",
"Также вам будут предоставлены отчеты, откройте его.",
"Тут вы видите ср.зарплату по рынку. ЗП, которую хочет кандидат. Его влияние",
"Кроме того, эффективные совы внедрили важные KPI для оценки нашей деятельности, вовлечености и проверки вашего стресса.",
"Справа позади меня вы видете доску на ней видны 2 метрики - зеленная сверху моя удовлетворенность вашей работой, а красная снизу ваш психологическое здоровье.","Если вы опустите зеленую метрику ниже нуля мы будем вынужденны уволить вас в 27 декабря, чтобы аннулировать наш договор и попробывать снова...",
                        "Удачи вам в первый ваш рабочий день вас уже ожидает новый сотрудник",
                    };

                Character character = new Character(new Vector2(0, 185), Menu.TextPlace, this, 5, new CharacterTraits(true, "Босс", "", "", "02.06.1981",
                    0, 2, 0,
                    ProfessionEnum.Developer, GradeEnum.Senior, 0, 0, 0, 0, 500,
                    text2), SoundEffects); //Директор

                return character;
            }
            else
            {
                List<string> speech = new List<string> { "..." };

                if (Hour >= 18) //конец дня
                {
                    if (Player.Mentality < 25)
                        speech = new List<string> { "Все, день закончился.", "Выглядишь неважно. Иди отдохни." };

                    int dif = 0;
                    foreach (KeyValuePair<ProfessionEnum, int> pair in Requirements)
                        dif += Math.Abs(pair.Value - HairedRatio[pair.Key]);

                    if (dif == 0)
                        Player.BossSatisfaction += 20;
                    else
                        Player.BossSatisfaction -= dif * 25;

                    if (Player.BossSatisfaction <= 0)
                        speech = new List<string> { "Ты мне не нравишься. теперь ты уволен.", "Да-да, уволен", "Я все сказал" };
                    else if (dif > 1)
                        speech = new List<string> { "Я не доволен." };
                    else if (dif == 1)
                        speech = new List<string> { "Норм." };
                    else
                        speech = new List<string> { "Отлично поработал." };
                }

                if (BossState == BossState.Help)
                {
                    speech = new List<string> { "Типа рассказываею че да как." };
                }
                else if (BossState == BossState.ExitTrigger)
                {
                    speech = new List<string> { "Так, куда собрался? Рабочий день до 6!" };
                    Player.Mentality -= 5;
                }
                BossState = BossState.Default;

                if (Player.BossSatisfaction <= 0)
                {
                    speech = new List<string> { "Ты мне не нравишься. теперь ты уволен.", "Да-да, уволен", "Я все сказал" };
                }

                if (Player.Professionality <= 0)
                {
                    if (!WasSecondChanceUsed)
                    {
                        Player.Professionality = Player.Unity / 2;
                        Player.Unity /= 2;
                        WasSecondChanceUsed = true;
                        speech = new List<string> { "В компании все плохо, все раздалбаи.", "Но я попросил их работать более усердно.", "Постарайся нанимать более квалифицированных сотрудников." };
                    }
                    else
                    {
                        speech = new List<string> { "Мы разорились из-за раздолбайства рабочих.", "Компанию почему-то заполнили неквалифицированные работники.", "Пока." };
                    }
                }
                else if (Player.Unity <= 0)
                {
                    if (!WasSecondChanceUsed)
                    {
                        Player.Unity = Player.Professionality / 2;
                        Player.Professionality /= 2;
                        WasSecondChanceUsed = true;
                        speech = new List<string> { "В компании все плохо, все без конца ругаются.", "На плаву сможем оставаться только за счет переработок.", "Постарайся нанимать более общительных сотрудников." };
                    }
                    else
                    {
                        speech = new List<string> { "Мы разорились из-за неслаженной работы.", "Все со всеми ругаются вместо работы.", "Пока." };
                    }
                }

                Character character = new Character(new Vector2(0, 185), Menu.TextPlace, this, 5, new CharacterTraits(true, "Босс", "", "", "02.06.1981",
                    0, 2, 0,
                    ProfessionEnum.Developer, GradeEnum.Senior, 0, 0, 0, 0, 500,
                    speech), SoundEffects); //Директор

                return character;
            }
        }

        public void TriggerBossByExit()
        {
            if (Objects.Character.Traits.IsBoss)
            {
                Menu.TextPlace.BeginSpeech(new List<string> { "Так, куда собрался. Рабочий день до 6!" });
                Player.Mentality -= 5;
            }
            else
            {
                Objects.Character.Reject();
                BossState = BossState.ExitTrigger;
            }
        }

        public void GameOver()
        {
            IsGameOver = true;
        }
    }

    class OfficeSceneObjectsHolder
    {
        private readonly Dictionary<Type, ISceneObject> dictionary = new Dictionary<Type, ISceneObject>();
        public List<ISceneObject> ObjectsCollection { get { return new List<ISceneObject>(dictionary.Values); } }
        public bool Contains(Type type) => dictionary.ContainsKey(type);

        public Whiteboard Whiteboard { get { return (Whiteboard)dictionary[typeof(Whiteboard)]; } 
            internal set { dictionary.Add(value.GetType(), value); } }
        public Garland Garland { get { return (Garland)dictionary[typeof(Garland)]; } 
            internal set { dictionary.Add(value.GetType(), value); } }
        public Door Door { get { return (Door)dictionary[typeof(Door)]; } 
            internal set { dictionary.Add(value.GetType(), value); } }
        public Exit Exit { get { return (Exit)dictionary[typeof(Exit)]; } 
            internal set { dictionary.Add(value.GetType(), value); } }
        public ChristmasTree ChristmasTree { get { return (ChristmasTree)dictionary[typeof(ChristmasTree)]; } 
            internal set { dictionary.Add(value.GetType(), value); } }
        public Calendar Calendar { get { return (Calendar)dictionary[typeof(Calendar)]; } 
            internal set { dictionary.Add(value.GetType(), value); } }
        public Clock Clock { get { return (Clock)dictionary[typeof(Clock)]; } 
            internal set { dictionary.Add(value.GetType(), value); } }
        public Cooler Cooler { get { return (Cooler)dictionary[typeof(Cooler)]; } 
            internal set { dictionary.Add(value.GetType(), value); } }
        public BackChair BackChair { get { return (BackChair)dictionary[typeof(BackChair)]; } 
            internal set { dictionary.Add(value.GetType(), value); } }
        public SideChair SideChair { get { return (SideChair)dictionary[typeof(SideChair)]; } 
            internal set { dictionary.Add(value.GetType(), value); } }
        public Character Character { get { return (Character)dictionary[typeof(Character)]; } 
            internal set { if (dictionary.ContainsKey(value.GetType())) dictionary[value.GetType()] = value; else dictionary.Add(value.GetType(), value); } }
        public Table Table { get { return (Table)dictionary[typeof(Table)]; } 
            internal set { dictionary.Add(value.GetType(), value); } }
        public CurriculumVitae CurriculumVitae { get { return (CurriculumVitae)dictionary[typeof(CurriculumVitae)]; } 
            internal set { dictionary.Add(value.GetType(), value); } }
        public Cat Cat { get { return (Cat)dictionary[typeof(Cat)]; } 
            internal set { dictionary.Add(value.GetType(), value); } }
    }

    enum BossState
    {
        Default,
        Help,
        ExitTrigger
    }
}
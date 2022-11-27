using JustHR.Classes.Basic;
using JustHR.Classes.Interface;
using JustHR.Classes.SceneObjects;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Audio;

namespace JustHR.Classes
{
    class OfficeScene : IScene
    {
        public BackgroundsEnum Background { get; } = BackgroundsEnum.Office;
        public Menu Menu { get; }
        public Dictionary<Enum, SoundEffectInstance> SoundEffects;
        public List<ISceneObject> Objects { get; } = new List<ISceneObject>();

        public int Day { get; }
        public int Hour { get; private set; } = 9;

        public Dictionary<ProfessionEnum, int> Requirements { get; } = new Dictionary<ProfessionEnum, int>();
        public Dictionary<ProfessionEnum, int> HairedRatio { get; } = new Dictionary<ProfessionEnum, int>();

        public bool IsDayEnded { get; private set; } = false;
        public bool IsGameOver { get; private set; } = false;
        public bool WasSecondChanceUsed { get; private set; } = false;

        public BossState BossState { get; private set; }

        public List<Character> RecallCharacters { get; } = new List<Character>();
        
        public OfficeScene(int day, Controller controller, Dictionary<Enum, SoundEffectInstance> soundEffects)
        {
            SoundEffects = soundEffects;

            Objects.Add(new Whiteboard());
            Objects.Add(new Garland());
            Objects.Add(new Door(this, controller));
            Objects.Add(new ChristmasTree());
            Objects.Add(new Calendar());
            Objects.Add(new Clock(this, controller, SoundEffects));
            Objects.Add(new Cooler(controller, SoundEffects));
            Objects.Add(new BackChair());
            Objects.Add(new SideChair());
            Objects.Add(null);
            Objects.Add(new Table());
            Objects.Add(new CurriculumVitae(this, controller, SoundEffects));
            Objects.Add(new Cat(controller, SoundEffects));


            Day = day;
            var buttons = new List<Button>();
            buttons.Add(new Button(ButtonEnum.AcceptButton, new Vector2(35, 475), new Vector2(245, 50), controller, () => {
                SoundEffects[SoundsEnum.phone_btn_sound_2].Play();

                CurriculumVitae cv = GetCV();
                Character character = GetCharacter();
                if (character.IsSitting())
                {
                    character.State = CharacterStateEnum.Accepted;
                    if (cv.IsExpanded)
                        cv.IsExpanded = false;
                    Menu.TextPlace.BeginSpeech(new List<string> { "" });

                    if (!character.IsBoss)
                    {
                        Settings.Professionality += character.Professionality;
                        Settings.Unity += character.SocialIntelligence;
                        Settings.Mentality = MathHelper.Clamp(Settings.Mentality + character.HairMoraleImpact, 0, 100);
                        Settings.BossSatisfaction += (int)(10 * (Settings.AvarajeSalary[character.Grade] - character.RequiredSalary) / (Settings.AvarajeSalary[character.Grade] * 0.3f));

                        HairedRatio[character.Profession]++;
                    }
                }
                
            }));
            buttons.Add(new Button(ButtonEnum.RejectButton, new Vector2(35, 530), new Vector2(245, 50), controller,() => {
                SoundEffects[SoundsEnum.phone_btn_sound_2].Play();

                CurriculumVitae cv = GetCV();
                Character character = GetCharacter();
                if (character.IsSitting())
                {
                    if (!character.IsBoss)
                        character.State = CharacterStateEnum.Rejected;
                    else
                        character.State = CharacterStateEnum.Accepted;

                    if (cv.IsExpanded)
                        cv.IsExpanded = false;
                    Menu.TextPlace.BeginSpeech(new List<string> { "" });

                    Settings.Mentality = MathHelper.Clamp(Settings.Mentality + character.RejectMaraleImpact, 0, 100);
                }
                
            }));
            buttons.Add(new Button(ButtonEnum.RecallButton, new Vector2(35, 585), new Vector2(245, 50), controller, () => {
                SoundEffects[SoundsEnum.phone_btn_sound_2].Play();

                CurriculumVitae cv = GetCV();
                Character character = GetCharacter();
                if (character.IsSitting())
                {
                    if (!character.IsBoss)
                        character.State = CharacterStateEnum.Rejected;
                    else
                        character.State = CharacterStateEnum.Accepted;

                    if (cv.IsExpanded)
                        cv.IsExpanded = false;
                    Menu.TextPlace.BeginSpeech(new List<string> { "" });

                    Settings.Mentality = MathHelper.Clamp(Settings.Mentality + character.RejectMaraleImpact/2, 0, 100);

                    if (!character.IsBoss)
                        RecallCharacters.Add(character);
                }
                
            }));
            buttons.Add(new Button(ButtonEnum.JobResponsibilitiesButton, new Vector2(35, 640), new Vector2(245, 50), controller, () => {
                SoundEffects[SoundsEnum.phone_btn_sound_2].Play();

                CurriculumVitae cv = GetCV();
                Character character = GetCharacter();
                if (character.IsSitting())
                {
                    if (!character.IsBoss)
                        character.State = CharacterStateEnum.Rejected;
                    else
                        character.State = CharacterStateEnum.Accepted;

                    if (cv.IsExpanded)
                        cv.IsExpanded = false;
                    Menu.TextPlace.BeginSpeech(new List<string> { "" });

                    Settings.Mentality = MathHelper.Clamp(Settings.Mentality + character.RejectMaraleImpact / 2, 0, 100);

                    if (!character.IsBoss)
                        RecallCharacters.Add(character);
                    BossState = BossState.Help;
                }
            }));
            Phone phone = new Phone(buttons);

            TextPlace textPlace = new TextPlace(controller);
            Menu = new Menu(phone, textPlace);

            CurriculumVitae GetCV()
            {
                foreach (ISceneObject objct in Objects)
                    if (objct.GetType() == typeof(CurriculumVitae))
                        return (CurriculumVitae)objct;

                throw new Exception("CV not found");
            }

            Random rnd = new Random();

            List<int> randomNums = new List<int>();
            do
            {
                int rndNum = rnd.Next(Enum.GetValues(typeof(ProfessionEnum)).Length);
                if (!randomNums.Contains(rndNum))
                    randomNums.Add(rndNum);
            } while (randomNums.Count < 3);

            int dayDif = (day - 27);
            Requirements.Add((ProfessionEnum)randomNums[0], 2 + dayDif + rnd.Next(1));
            Requirements.Add((ProfessionEnum)randomNums[1], 2 + rnd.Next((dayDif/2),dayDif));
            Requirements.Add((ProfessionEnum)randomNums[2], 1 + rnd.Next(dayDif/2));

            HairedRatio.Add((ProfessionEnum)randomNums[0], 0);
            HairedRatio.Add((ProfessionEnum)randomNums[1], 0);
            HairedRatio.Add((ProfessionEnum)randomNums[2], 0);

            GenerateBoss();
        }

        public void NextHour()
        {
            if (Hour < 18)
            {
                Hour++;
                Cooler coller = GetObject<Cooler>();
                coller.CoffeEffectiveness = MathHelper.Clamp(coller.CoffeEffectiveness + 0.1f, 0, 1);

                Cat cat = GetObject<Cat>();
                if (cat.IsAngry)
                    cat.AngryWeight--;
            }
        }

        public void EndDay()
        {
            Character character = GetObject<Character>();
            if (!character.IsBoss && character.IsSitting())
            {
                CurriculumVitae cv = GetObject<CurriculumVitae>();
                character.State = CharacterStateEnum.Rejected;
                if (cv.IsExpanded)
                    cv.IsExpanded = false;
                Menu.TextPlace.BeginSpeech(new List<string> { "" });
            }
        }

        public Character GetCharacter()
        {
            foreach (ISceneObject objct in Objects)
                if (objct.GetType() == typeof(Character))
                    return (Character)objct;

            throw new Exception("Character not found");
        }

        public T GetObject<T>() where T : ISceneObject
        {
            foreach (ISceneObject objct in Objects)
                if (objct != null && objct.GetType() == typeof(T))
                    return (T)objct;

            throw new Exception("Object not found");
        }

        public void CharacterExited()
        {
            if (GetObject<Character>().IsBoss) //босс вышел после оглашения проигрыша
                if (Settings.Professionality <= 0 ||
                    Settings.Unity <= 0 ||
                    Settings.BossSatisfaction <= 0)
                {
                    GameOver();
                }

            if (Hour < 18 &&
                Settings.Professionality > 0 &&
                Settings.Unity > 0 &&
                Settings.BossSatisfaction > 0 &&
                BossState == BossState.Default)
            {
                GenerateNewCharacter();
            }
            else
                GenerateBoss();
        }

        public void Exit()
        {
            if (Settings.BossSatisfaction <= 0)
                GameOver();
            else
                IsDayEnded = true;
        }

        public void GenerateNewCharacter()
        {
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

            var speeches_num = new List<int> { 0, -5, 0, -5, 0, -5, 0, -5  };
            var full_name = new List<String>
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

            for (int i = 0; i < Objects.Count; i++)
            {
                if (Objects[i].GetType() == typeof(Character))
                {
                    Random rnd = new Random();
                    if (Hour >= 14 && RecallCharacters.Count > 0)
                    {
                        Character character = RecallCharacters[rnd.Next(RecallCharacters.Count)];
                        RecallCharacters.Remove(character);
                        Objects[i] = character;
                    }
                    else
                    {
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

                        int professionality = rnd.Next(-10, 11) + (int)grade * 2 - rnd.Next((int)(Settings.Professionality / 50f - 1) * 4);
                        int socialIntelligence = rnd.Next(-10, 11) - (int)grade - rnd.Next((int)(Settings.Unity / 50f - 1) * 4);

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
                        Objects[i] = character;
                    }

                    Door door = GetObject<Door>();
                    door.State = DoorState.Opend;

                    SoundEffects[SoundsEnum.door_opening].Play();
                }
            }
        }

        private void GenerateBoss()
        {
            for (int i = 0; i < Objects.Count; i++)
            {
                //начало дня
                if (Objects[i] == null)
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
                    Objects[i] = character;

                    Door door = GetObject<Door>();
                    door.State = DoorState.Opend;

                    SoundEffects[SoundsEnum.door_opening].Play();
                }
                else if (Objects[i].GetType() == typeof(Character))
                {
                    List<string> speech = new List<string> { "..." };

                    if (Hour >= 18) //конец дня
                    {
                        if (Settings.Mentality < 25)
                            speech = new List<string> { "Все, день закончился.", "Выглядишь неважно. Иди отдохни." };

                        int dif = 0;
                        foreach (KeyValuePair<ProfessionEnum, int> pair in Requirements)
                            dif += Math.Abs(pair.Value - HairedRatio[pair.Key]);

                        if (dif == 0)
                            Settings.BossSatisfaction += 20;
                        else
                            Settings.BossSatisfaction -= dif * 25;

                        if (Settings.BossSatisfaction <= 0)
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
                    }else if (BossState == BossState.ExitTrigger)
                    {
                        speech = new List<string> { "Так, куда собрался? Рабочий день до 6!" };
                        Settings.Mentality = MathHelper.Clamp(Settings.Mentality - 5, 0, 100);
                    }
                    BossState = BossState.Default;

                    if (Settings.BossSatisfaction <= 0)
                    {
                        speech = new List<string> { "Ты мне не нравишься. теперь ты уволен.", "Да-да, уволен", "Я все сказал" };
                    }

                    if (Settings.Professionality <= 0)
                    {
                        if (!WasSecondChanceUsed)
                        {
                            Settings.Professionality = Settings.Unity / 2;
                            Settings.Unity /= 2;
                            WasSecondChanceUsed = true;
                            speech = new List<string> { "В компании все плохо, все раздалбаи.", "Но я попросил их работать более усердно.", "Постарайся нанимать более квалифицированных сотрудников." };
                        } else
                        {
                            speech = new List<string> { "Мы разорились из-за раздолбайства рабочих.", "Компанию почему-то заполнили неквалифицированные работники.", "Пока." };
                        }
                    } else if (Settings.Unity <= 0)
                    {
                        if (!WasSecondChanceUsed)
                        {
                            Settings.Unity = Settings.Professionality / 2;
                            Settings.Professionality /= 2;
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
                    Objects[i] = character;

                    Door door = GetObject<Door>();
                    door.State = DoorState.Opend;

                    SoundEffects[SoundsEnum.door_opening].Play();
                }
            }
        }

        public void TriggerBoosByExit()
        {
            Character character = GetCharacter();
            if (character.IsBoss)
            {
                Menu.TextPlace.BeginSpeech(new List<string> { "Так, куда собрался. Рабочий день до 6!" });
                Settings.Mentality = MathHelper.Clamp(Settings.Mentality - 5, 0, 100);
            }
            else
            {
                character.State = CharacterStateEnum.Rejected;
                BossState = BossState.ExitTrigger;
            }
        }

        public void GameOver()
        {
            IsGameOver = true;
        }
    }

    enum BossState
    {
        Default,
        Help,
        ExitTrigger
    }
}
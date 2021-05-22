using System;
using System.Collections.Generic;

namespace Exam
{
    // обычный класс Program
    class Program
    {
        static void Main(string[] args)
        {
            Baby bb = new Baby(2, 15, "Boy");

            // Пример Поллиморфизма - возможности "одного" метода/функции обарабатывать разные входные значения
            // здесь мы передаем ей сначало тип int 
            // а в следующий раз тип double
            Console.WriteLine(bb.Live(0));
            Console.WriteLine(bb.Live(0.0));

            // Пример сокрытия в инкапсюляции
            // Console.WriteLine(bb.BeHappy()); - Этот мтод мы использовать не можем так как он скрыт от нас
            Console.WriteLine(bb.DontWorry()); // А этот метод мы легко можем использовать из любого места пространства имен так как он публичный

            General gg = General.GetGeneral();
            Marine marine = new Marine();
            gg.Add(marine);
            marine.BeAttacked();
        }
    }

    interface IAnimal
    {
        // Свойство интерфейса
        // READ ONLY свойство
        string Kingdom { get; }
        int Age { get; set; }

        // Методы интерфейса с перегрузкой 
        bool Live(double damage);
        bool Live(int damage);
    }

    // Так обьявляется абстрактынй класс
    abstract class Human
    {
        protected const string kingdom = "Млекопитающиеся";
        protected int age;
        // Свойтва абстрактного класса и привязанные к ним переменные
        private int height;
        private string name;
        public int Height
        {
            get
            {
                return height;
            }
            set
            {
                height = value;
            }
        }

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }

        // Конструктор класса
        public Human(int height, string name)
        {
            this.height = height;
            this.name = name;
        }

        // Абстрактынй метод делающий класс абстрактным
        abstract public void Walk();

        // Скорытый метод
        protected string BeHappy()
        {
            return "Be happy!";
        }
    }

    class Baby : Human, IAnimal
    {
        // Наследуемые от интерефейса свойтсва и методы обязательно должны иметь можификатор доступа public 
        // Пример инкапсюляции здесь для свойств этого класса мы используем protected поля класса родителя.
        public string Kingdom
        { get { return kingdom; } }
        public int Age { get { return age; } set { age = value; } }

        //                                        Передаем значения в конструктор родителя
        public Baby(int age, int height, string name) : base(height, name)
        {
            if (age > 4) this.age = 4;
            else this.age = age;
        }

        // Даем определение унаследованному абстрактному методу с помощью ключевого слова override
        public override void Walk()
        {
            Console.WriteLine("Ползти вперёд!");
        }


        // Для опредления методов интерфейса нам не обязательно использовать override, так как достаточно что бы они были
        public bool Live(double damage)
        {
            if (damage > 100.0) return false;
            else return true;
        }
        public bool Live(int damage)
        {
            if (damage > 100) return false;
            else return true;
        }

        // Использование protected метода в дочернем классе
        public string DontWorry()
        {
            return "Don't Worry / " + BeHappy();
        }
    }

    // Интерфейс наблюдателя
    public interface IObserver
    {
        void Add(ISubject millitaryUnit);
        void Delete(ISubject millitaryUnit);
        string GiveOrder(ISubject millitaryUnit, int situationCode);
    }

    // Singlton - Одиночка
    class General : IObserver
    {
        private List<ISubject> units = new List<ISubject>();
        // Закрываем доступ к обычному конструктору
        private General() { }

        // Создаём закрытое статическое поле с инстансом нашего одиночки
        private static General generalInstance;

        // Данный метод будет выполнять роль констркутора
        public static General GetGeneral()
        {
            // Если мы обращаемся к одиночке первый раз, то инициализиаруем его,
            // Всё последующее время работы программы просто будем возвращать сохраненный инстанс
            if (generalInstance == null) generalInstance = new General();
            return generalInstance;
        }

        // Добавляем обьекты в список
        public void Add(ISubject millitaryUnit)
        {
            units.Add(millitaryUnit);
        }
        public void Delete(ISubject millitaryUnit)
        {
            units.Remove(millitaryUnit);
        }

        // Получаем обстановку о отдаем приказ
        public string GiveOrder(ISubject millitaryUnit, int situationCode)
        {
            if(situationCode == 0)
            {
                return "Relax";
            }
            else if(situationCode == 1)
            {
                return "To Battle!";
            }
            else
            {
                foreach(ISubject un in units)
                {
                    un.ToBattle();
                    
                }
                return "To Battle!";
            }
        }
    }


    // Интерфейс субьектов
    public interface ISubject
    {
        void ConveyTheSetting(int code);
        void ToBattle();
    }
    // Субьект
    class Marine : ISubject
    {
        // Получает экземпляр обьекта к которому обращается
        private IObserver myGeneral = General.GetGeneral();
        protected void Deffence()
        {
            Console.WriteLine("Deff");
        }
        public void ToBattle()
        {
            Console.WriteLine("Yeeeeeeee");
            myGeneral.Delete(this);
        }
        public void BeAttacked()
        {
            ConveyTheSetting(1);
        }
        // По ситуации передаём необходимые значения
        public void ConveyTheSetting(int code)
        {
            // передаем приказ и получаем ответ
            var order = myGeneral.GiveOrder(this, code);
            if (order == "Relax") Deffence();
            else if (order == "To Battle!") ToBattle();
        }

    }

    class Internal : ISubject
    {
        private IObserver myGeneral = General.GetGeneral();
        protected void Deffence()
        {
            Console.WriteLine("Deff");
        }
        public void ToBattle()
        {
            Console.WriteLine("Nooooo");
            myGeneral.Delete(this);
        }
        public void BeAttacked()
        {
            ConveyTheSetting(1);
        }
        public void ConveyTheSetting(int code)
        {
            var order = myGeneral.GiveOrder(this, code);
            if (order == "Relax") Deffence();
            else if (order == "To Battle!") ToBattle();
        }
    }

    class Air : ISubject
    {
        private IObserver myGeneral = General.GetGeneral();
        protected void Deffence()
        {
            Console.WriteLine("Deff");
        }
        public void ToBattle()
        {
            Console.WriteLine("Vzhuuuuhh");
            myGeneral.Delete(this);
        }
        public void BeAttacked()
        {
            ConveyTheSetting(1);
        }
        public void ConveyTheSetting(int code)
        {
            var order = myGeneral.GiveOrder(this, code);
            if (order == "Relax") Deffence();
            else if (order == "To Battle!") ToBattle();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Threading;

namespace Train
{
    internal class Program
    {
        static void Main()
        {
            Console.Title = "Конструктор поездов";

            DataBase data = new DataBase();
            TrainCreator creator = new TrainCreator();
            creator.Work(data);
        }
    }

    class DataBase
    {
        List<Train> _trains = new List<Train>();

        public void Add(Train train)
        {
            _trains.Add(train);
        }

        public void ShowInfo()
        {
            const int positionTop = 24;

            if(_trains.Count > 0)
            {
                Console.SetCursorPosition(0, positionTop);

                Console.WriteLine("Поезда на линии");

                foreach(Train train in _trains)
                {
                    train.ShowInfo();
                }

                Console.SetCursorPosition(0, 0);
            }
        }
    }

    class TrainCreator
    {
        private Direction _direction = new Direction("не", "задано");
        private int _tickets = 0;
        private int _freePlaces = 0;
        private int _wagons = 0;
        private string _stat="Не отправлен на маршрут";
        
        public int SeatsOneWagon => 24;
        
        public void Work(DataBase trainData)
        {
            int nextStep = 0;
            bool isNewTrain = true;

            while(isNewTrain)
            {
                if(nextStep > 0)
                    trainData.ShowInfo();

                ShowRealTimeInfo();

                switch(nextStep)
                {
                    case 0:
                    _direction = GetDirection();
                    nextStep++;
                    Console.WriteLine("Нажмите любую клавишу для продолжения ");
                    break;

                    case 1:
                    _tickets = SoldTickets();
                    nextStep++;
                    Console.WriteLine("Нажмите любую клавишу для продажи билетов ");
                    break;

                    case 2:
                    ShowSellingTickets(_tickets);
                    _wagons = CreateWagons(_tickets, SeatsOneWagon);
                    _freePlaces = GetFreePlaces(_wagons, _tickets, SeatsOneWagon);
                    nextStep++;
                    Console.WriteLine("Нажмите любую клавишу для создания вагонов");
                    break;

                    case 3:
                    Train train = new Train(_direction, _wagons, _freePlaces);
                    trainData.Add(train);
                    _stat = "На маршруте";
                    nextStep++;
                    Console.WriteLine("Нажмите любую клавишу для запуска поезда");
                    break;

                    case 4:
                    isNewTrain = IsContinue();
                    nextStep = 0;
                    Console.WriteLine("Нажмите любую клавишу для продолжения ");
                    break;
                }

                Console.ReadKey();
                Console.Clear();
            }
        }

        private Direction GetDirection()
        {
            string[] directions =
            {
                "Санкт-Петербург",
                "Обухово","Колпино",
                "Тосно",
                "Чудово-московское",
                "Малая Вишера",
                "Окуловка",
                "Бологое",
                "Вышний Волочёк",
                "Спирово","Тверь",
                "Редкино*","Крюково",
                "Лихоборы (бывш. НАТИ)",
                "Москва (Ленинградский вокзал)"
            };

            string departurePoint = null;
            string arrivalPoint = null;
            bool isCorrectInput = false;

            Console.WriteLine("Список актуальных маршрутов");

            for(int i = 0; i < directions.Length; i++)
            {
                Console.WriteLine($"[{i + 1}] {directions[i]}");
            }

            while(isCorrectInput == false)
            {
                Console.Write("\nВведите номер точки отправления : ");
                int index = ReadInt();

                if(index >= 0 && index < directions.Length)
                {
                    departurePoint = directions[index];
                }

                Console.Write("\nВведите номер точки прибытия : ");
                index = ReadInt();

                if(index >= 0 && index < directions.Length)
                {
                    arrivalPoint = directions[index];
                }

                if(departurePoint == arrivalPoint)
                {
                    Console.WriteLine("Неверный Ввод!!! Точки отбытия и  прибытия не могут быть одинаковыми ");

                }
                else if(departurePoint == null || arrivalPoint == null)
                {
                    Console.WriteLine("Ошибка!!! Выбран не существующий маршрут");
                }
                else
                {
                    isCorrectInput = true;
                }
            }

            return new Direction(departurePoint, arrivalPoint);
        }

        private static int ReadInt()
        {
            int.TryParse(Console.ReadLine(), out int number);
            int index = number - 1;
            return index;
        }

        private void ShowSellingTickets(int tickets)
        {
            const int millisecondsTimeout = 40;
            Console.CursorVisible = false;
            Console.Clear();

            for(int i = 0; i <= tickets; i++)
            {
                Console.SetCursorPosition(0, 0);
                Console.WriteLine($"проданных билетов : {i}");
                Thread.Sleep(millisecondsTimeout);
            }

            Console.CursorVisible = true;
        }

        private bool IsContinue()
        {
            const string CommandСonfirm = "yes";
            const string CommanfDoNotСonfirm = "no";
            Direction newDirection = new Direction("не", "задано");
            Console.WriteLine($"продолжить работу {CommandСonfirm}/{CommanfDoNotСonfirm}");

            if(Console.ReadLine() == CommanfDoNotСonfirm)
                return false;
            else
            {
                _tickets = 0;
                _freePlaces = 0;
                _wagons = 0;
                _direction = newDirection;
                _stat = "Не отправлен на маршрут";
                return true;
            }
        }

        private void ShowRealTimeInfo()
        {
            Console.WriteLine(new string('-', 30));
            Console.Write("Направление : ");
            _direction.Show();
            Console.WriteLine($"\nПроданных билетов : {_tickets}");
            Console.WriteLine($"Вагонов : {_wagons}");
            Console.WriteLine($"Свободных мест в поезде : {_freePlaces}");
            Console.WriteLine($"Статус поезда : {_stat}");
            Console.WriteLine(new string('-', 30));
        }

        private int SoldTickets()
        {
            Random random = new Random();
            return random.Next(120, 250);
        }

        private int CreateWagons(int tickets, int oneWagonSeats)
        {
            int wagonsCount = tickets / oneWagonSeats;

            if(tickets % oneWagonSeats != 0)
            {
                wagonsCount++;
            }

            return wagonsCount;
        }

        private int GetFreePlaces(int wagons, int tickets, int oneWagonSeats)
        {
            return (wagons * oneWagonSeats) - tickets;
        }
    }

    class Train
    {
        private int _wagonsCount;
        private int _freePlaces;
        private Direction _direction;

        public Train(Direction direction, int wagonsCount, int freePlaces)
        {
            _direction = direction;
            _wagonsCount = wagonsCount;
            _freePlaces = freePlaces;
        }

        public void ShowInfo()
        {
            Console.Write("|поезд ");
            _direction.Show();
            Console.WriteLine($"| количество вагонов {_wagonsCount}| свободных мест {_freePlaces}|");
        }
    }

    class Direction
    {
        private string _departure;
        private string _arrival;

        public Direction(string departure, string arrival)
        {
            _departure = departure;
            _arrival = arrival;
        }

        public void Show()
        {
            Console.Write($"{_departure} - {_arrival} ");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;

namespace Train1
{
    internal class Program
    {
        private void Main()
        {
            Dispatcher dispatcher = new Dispatcher();
            TrainBuilder trainConstructor = new TrainBuilder();
            WaitingRoom waitingRoom = new WaitingRoom();
            Logger logger = new Logger();

            dispatcher.StartNewShift(trainConstructor, waitingRoom, logger);
        }
    }

    class Logger
    {
        private readonly List<Train> _departedTrains;

        public Logger()
        {
            _departedTrains = new List<Train>();
        }

        public int DepartedAmount { get => _departedTrains.Count; }

        public void LogTrain(Train train)
            => _departedTrains.Add(train);

        public void ShowTrains()
        {
            if(DepartedAmount != 0)
            {
                for(int i = 0; i < _departedTrains.Count; i++)
                {
                    int index = i + 1;

                    Console.Write(index + ". ");
                    _departedTrains[i].ShowInfo();
                }
            }
            else
            {
                Console.WriteLine("Отправленных поездов пока нет.");
            }
        }
    }

    class Dispatcher
    {
        public void StartNewShift(TrainBuilder trainConstructor, WaitingRoom waitingRoom, Logger logger)
        {
            const string NewRouteCommand = "1";
            const string ExitCommand = "2";

            string userInput = "";

            while(userInput != ExitCommand)
            {
                logger.ShowTrains();

                Console.WriteLine($"{NewRouteCommand} - создать новое направление.");
                Console.WriteLine($"{ExitCommand} - закончить смену.");

                userInput = Console.ReadLine();

                switch(userInput)
                {
                    case NewRouteCommand:
                    logger.LogTrain(CrearteTrainRoute(trainConstructor, waitingRoom));
                    break;

                    case ExitCommand:
                    Console.WriteLine($"Вы закончили смену, отправив {logger.DepartedAmount} поездов.");
                    break;

                    default:
                    Console.WriteLine("Неизвестная команда.");
                    break;
                }

                Console.ReadKey();
                Console.Clear();
            }
        }

        private Train CrearteTrainRoute(TrainBuilder trainBuilder, WaitingRoom waitingRoom)
        {
            Dictionary<string, int> tickets = waitingRoom.GetTickets();
            List<TrainCart> carts = new CartBuilder().Build(tickets);

            return trainBuilder.Build(carts);
        }
    }

    class TrainBuilder
    {
        public Train Build(List<TrainCart> carts)
        {
            CreateRoute(out string departurePoint, out string destinationPoint);
            Train train = new Train(destinationPoint, departurePoint, carts);

            return train;
        }

        private void CreateRoute(out string departurePoint, out string destinationPoint)
        {
            departurePoint = GetLine("Введите точку отправления поезда: ");
            destinationPoint = GetLine("Введите точку назначения поезда: ");
        }

        private string GetLine(string message)
        {
            string userInput = "";
            bool isValid = false;

            while(isValid == false)
            {
                Console.Write(message);
                userInput = Console.ReadLine();

                if(userInput.Length != 0)
                    isValid = true;
            }

            return userInput;
        }
    }

    class CartBuilder
    {
        public List<TrainCart> Build(Dictionary<string, int> tickets)
        {
            Dictionary<string, int> cartTypes = new CartTypes().Get();
            List<TrainCart> carts = new List<TrainCart>();

            foreach(KeyValuePair<string, int> ticket in tickets)
            {
                int ticketsAmount = ticket.Value;
                int capacity = cartTypes[ticket.Key];
                string type = ticket.Key;

                carts.AddRange(CreateCarts(type, ticketsAmount, capacity));
            }

            return carts;
        }

        private List<TrainCart> CreateCarts(string type, int ticketsAmount, int capacity)
        {
            List<TrainCart> carts = new List<TrainCart>();

            while(capacity < ticketsAmount)
            {
                ticketsAmount -= capacity;
                carts.Add(new TrainCart(type, capacity));
            }

            if(ticketsAmount != 0)
            {
                carts.Add(new TrainCart(type, ticketsAmount));
            }

            return carts;
        }
    }

    class Train
    {
        private readonly List<TrainCart> _carts = new List<TrainCart>();
        private readonly string _destinationPoint;
        private readonly string _departurePoint;

        public Train(string destinationPoint, string departurePoint, List<TrainCart> carts)
        {
            _destinationPoint = destinationPoint;
            _departurePoint = departurePoint;
            _carts = carts;
        }

        public void ShowInfo()
        {
            Console.WriteLine($"Направление: {_departurePoint} - {_destinationPoint}");

            foreach(TrainCart cart in _carts)
            {
                cart.ShowInfo();
            }
        }
    }

    class WaitingRoom
    {
        private static readonly Random _random = new Random();

        public int EconomWaiters { get; private set; }
        public int BusinessWaiters { get; private set; }

        public Dictionary<string, int> GetTickets()
        {
            int maxTickets = 100;

            List<string> ticketTypes = new CartTypes().Get().Keys.ToList();
            Dictionary<string, int> tickets = new Dictionary<string, int>();

            foreach(string ticketType in ticketTypes)
            {
                tickets[ticketType] = _random.Next(maxTickets);
            }

            return tickets;
        }
    }

    class TrainCart
    {
        private readonly int _passangersAmount;
        private readonly string _type;

        public TrainCart(string type, int passengersAmount)
        {
            _type = type;
            _passangersAmount = passengersAmount;
        }

        public void ShowInfo()
        {
            Console.WriteLine($"  {_type} - занято {_passangersAmount}");
        }
    }

    class CartTypes
    {
        public const string Business = "Купе";
        public const string Economy = "Плацкарт";

        private readonly Dictionary<string, int> _capacity = new Dictionary<string, int>()
        {
            { Business, 20 },
            { Economy, 54 },
        };

        public Dictionary<string, int> Get()
        {
            return new Dictionary<string, int>(_capacity);
        }
    }
}

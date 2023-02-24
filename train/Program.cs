using System;

namespace Train
{
    internal class Program
    {
        static void Main()
        {
            TrainCreator creator = new TrainCreator();
            Train train = creator.GetNewTrain();
            Console.WriteLine(new string('-', 40));

            const string CommandSetDeparturePoint = "1";
            const string CommandSetPointOfArrival = "2";

            bool isWork = true;

            while(isWork)
            {
                train.ShowInfo();

                switch(Console.ReadLine())
                {
                    case CommandSetDeparturePoint:

                    break;

                    case CommandSetPointOfArrival:

                    break;
                }
            }
        }
    }


    class TrainCreator
    {


        public Train GetNewTrain()
        {
            Direction direction = GetDirection();
            Wagon wagons = null;

            int tickets = SoldTickets();
            Console.WriteLine($"количество проданных билетов {tickets}");
            wagons = CreateWagons(tickets, wagons.OneWagonSeats);
            int freePlaces = GetFreePlaces(wagons.WagonCount, tickets, wagons.OneWagonSeats);
            // Train train = new Train(direction, wagons, freePlaces);
            Train train = new Train(direction, wagons, freePlaces);
            return train;
        }

        public Direction GetDirection()
        {
            string departurePoint = "";
            string arrivalPoint = "";

            while(departurePoint == arrivalPoint)
            {
                Console.WriteLine("введите точку отправления ");
                departurePoint = Console.ReadLine();

                Console.WriteLine("Введите точку прибытия");
                arrivalPoint = Console.ReadLine();
            }

            return new Direction(departurePoint, arrivalPoint);
        }

        private int SoldTickets()
        {
            Random random = new Random();
            return random.Next(120, 250);
        }

        private Wagon CreateWagons(int tickets, int OneWagonSeats)
        {
            int wagonsCount = tickets / OneWagonSeats;

            if(tickets % OneWagonSeats != 0)
            {
                wagonsCount++;

            }

            return new Wagon(wagonsCount);
        }

        private int GetFreePlaces(int wagons, int tickets, int oneWagonSeats)
        {
            return (wagons * oneWagonSeats) - tickets;
        }
    }

    //class CartBuilder
    //{

    //}

    class Train
    {
        private Wagon _wagonsCount;
        private int _freePlaces;
        private Direction _direction;

        public Train(string direction, Wagon wagonsCount, int freePlaces)
        {
            // _direction = direction;
            _wagonsCount = wagonsCount;
            _freePlaces = freePlaces;
        }

        public Train(Direction direction, Wagon wagonsCount, int freePlaces)
        {
            _direction = direction;
            _wagonsCount = wagonsCount;
            _freePlaces = freePlaces;
        }

        //public void ShowInfo()
        //{
        //    Console.WriteLine($"| поезд {_departurePoint} - {_arrivalPoint} | количество вагонов {_wagonsCount}| свободных мест {_freePlaces}|");
        //}

        public void ShowInfo()
        {
            Console.Write("|поезд ");
            _direction.Show();
            Console.WriteLine($"| количество вагонов {_wagonsCount.WagonCount}| свободных мест {_freePlaces}|");

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

    class Wagon
    {
        private int _wagonCount;

        public Wagon(int wagonCount)
        {
            _wagonCount = wagonCount;
        }

        public int WagonCount => _wagonCount;
        public int OneWagonSeats => 24;
    }

    //class WaitingRoom
    //{

    //}

    //class TrainCart
    //{

    //}

    //class CartTypes
    //{

    //}

}

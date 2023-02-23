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
            train.ShowInfo();
            int step = 1;
            while(true)
            {
                    Console.WriteLine($"step на входе {step}");
                switch(step)
                {
                    case 1:
                    step++;
                    break;

                    case 2:
                    //Console.WriteLine($"step на входе {step}");
                    step++;
                   // Console.WriteLine($"на выходе {step}");
                    break;

                    case 3:
                   // Console.WriteLine($"step на входе {step}");
                    step++;
                   // Console.WriteLine($"на выходе {step}");
                    break;

                    case 4:
                        step=1;
                    break;



                }
                    Console.WriteLine($"на выходе {step}");
                Console.ReadKey();
            }




        }
    }

    //class Logger
    //{

    //}

    //class Dispatcher
    //{

    //}

    class TrainCreator
    {
        int _oneWagonSeats = 24;

        public Train GetNewTrain()
        {
            Direction direction = GetDirection();
            int tickets = SoldTickets();
            Console.WriteLine($"количество проданных билетов {tickets}");
            int wagons = CreateWagons(tickets);
            int freePlaces = GetFreePlaces(wagons, tickets);
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

        private int CreateWagons(int tickets)
        {
            int wagonsCount = tickets / _oneWagonSeats;

            if(tickets % _oneWagonSeats != 0)
            {
                wagonsCount++;

            }

            return wagonsCount;
        }

        private int GetFreePlaces(int wagons, int tickets)
        {
            return (wagons * _oneWagonSeats) - tickets;
        }
    }

    //class CartBuilder
    //{

    //}

    class Train
    {
        private int _wagonsCount;
        private int _freePlaces;
        private Direction _direction;

        public Train(string direction, int wagonsCount, int freePlaces)
        {
            // _direction = direction;
            _wagonsCount = wagonsCount;
            _freePlaces = freePlaces;
        }

        public Train(Direction direction, int wagonsCount, int freePlaces)
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

    class Wagon
    {
        int _oneWagonSeats = 24;

        public  int OneWagonSeats =>  _oneWagonSeats;
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

using System.Security.Cryptography.X509Certificates;

namespace CarRace
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            await CarRace();
        }

        public static async Task CarRace()
        {
            Console.WriteLine("Welcome to the Car race! Press any key to start the race!");
            Console.WriteLine("Press enter to get an update on the cars!");
            Console.ReadKey();
            Console.WriteLine("3..2..1 Race have started!");

            //skapar objekt av bilar
            Car astonMartin = new()
            {
                Id = 1,
                Name = "Aston Martin"
            };
            Car jaguar = new()
            {
                Id = 2,
                Name = "Jaguar"
            };

            //tasks
            var astonMartinTask = Race(astonMartin);
            var jaguarTask = Race(jaguar);
            var statusCarTask = CarStatus(new List<Car> { astonMartin, jaguar });
            var carTask = new List<Task> { astonMartinTask, jaguarTask, statusCarTask };
            bool winner = false;

            //kör sålänge racet är igång
            while (carTask.Count > 0)
            {
                Task finishedTask = await Task.WhenAny(carTask);

                if (finishedTask == statusCarTask)
                {
                    Console.WriteLine("Race Done");
                }

                if (finishedTask == astonMartinTask)
                {
                    if (winner == false) //om vinnare
                    {
                        winner = true;
                        Console.WriteLine($"We have a winner!! {astonMartin.Name} passed the line first!");
                    }
                    else
                        Console.WriteLine($"{astonMartin.Name} has passed the line.");


                }
                else if (finishedTask == jaguarTask)
                {
                    if (winner == false) //om vinnare
                    {
                        winner = true;
                        Console.WriteLine($"We have a winner!! {jaguar.Name} passed the line first!");
                    }
                    else
                        Console.WriteLine($"{jaguar.Name} has passed the line.");

                }
                else if (finishedTask == statusCarTask)
                {
                    Console.WriteLine("All cars crossed the finnish line!");
                }


                await finishedTask;
                carTask.Remove(finishedTask);
            }


        }

        public static async Task Wait(int delay = 30)
        {
            await Task.Delay(TimeSpan.FromSeconds(delay / 10));
        }

        public static async Task<Car> Race(Car car)
        {
            
            int time = 30;
            Random random = new Random();
            while (true)
            {
                if (car.RaceTime % 30 == 0)
                {
                    int randomPenalty = random.Next(1, 51);
                    if (randomPenalty == 1)
                    {

                        Console.WriteLine(car.Name + " is out of gas! Stopping for 30 sec");
                        await Wait(30);
                        car.Penalty += 30;
                    }
                    else if (randomPenalty <= 3)
                    {
                        Console.WriteLine(car.Name + " got a flat tire! Stopping for 20 sec");
                        await Wait(20);
                        car.Penalty += 20;
                    }
                    else if (randomPenalty >= 3 && randomPenalty <= 7)
                    {
                        Console.WriteLine(car.Name + " got a bird on the windshield! Stopping for 10 sec");
                        await Wait(10);
                        car.Penalty += 10;


                    }
                    else if (randomPenalty >= 8 && randomPenalty <= 17)
                    {
                        Console.WriteLine($"{car.Name} got a engine issue! Going 1km/h slower");
                        car.Speed -= 1;
                    }
                }
                double speed = car.Speed / 3.6;

                await Wait(time);
                
                car.DistanceLeft -= (speed * time);

                if(car.DistanceLeft <= 20)
                {
                    
                    return car;
                }

            }
        }    
            public static async Task CarStatus(List<Car> cars)
            {
                while (true)
                {
                await Wait(100);

                    DateTime start = DateTime.Now;
                    bool pressedKey = false;
                    while ((DateTime.Now - start).TotalSeconds < 2) 
                    {
                        if (Console.KeyAvailable) //om en knapp trycks
                        {
                            pressedKey = true;
                            break;
                        }
                    }
                    if (pressedKey)
                    {
                        if (Console.ReadKey().Key == ConsoleKey.Enter) //När enter trycks
                        {
                            cars.ForEach(car => //skriv ut alla bilar
                            {
                                Console.WriteLine($"{car.Name}: Drives {car.Speed}Km/h and has {car.DistanceLeft} meters left.");
                            });
                            pressedKey = false;
                        }
                    }
                    var remaining = cars.Select(car => car.DistanceLeft).Sum(); 
                    if (remaining <= 0) //avslutar loopen när alla bilar kommit i mål
                    {
                        return; 
                    }
                }
            }

        
    }
}
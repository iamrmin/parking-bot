using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingBot
{
    public interface IParkingCalculator
    {
        float TotalCollection { get; set; }
        bool IsCoinAllowed(float value);
        DateTime CalculateTime(float money);
        bool IsParkingFree();
    }

    public class EuropeanParking : IParkingCalculator
    {
        readonly float[] _allowedCoins = { 0.10f, 0.20f, 0.50f, 1, 2 };
        private readonly DateTime[] _publicHolidays = { new DateTime(DateTime.Now.Year, 5, 14), new DateTime(DateTime.Now.Year, 12, 25) };

        public float TotalCollection { get; set; }

        /// <summary>
        /// Method to verify allowed coins
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool IsCoinAllowed(float value)
        {
            return _allowedCoins.Contains(value);
        }

        /// <summary>
        /// Clear the screen and print the value on screen.
        /// </summary>
        /// <param name="money"></param>
        /// <returns></returns>
        public DateTime CalculateTime(float money)
        {
            TotalCollection += money;

            var result = DateTime.Now;

            if ((int)money / 8 > 0)
            {
                result = result.AddDays((int)(money / 8));
                money = money - (8 * ((int)(money / 8)));
            }

            if ((int)money / 5 > 0)
            {
                result = result.AddHours(12);
                money -= 5;
            }

            if ((int)money / 3 > 0)
            {
                result = result.AddHours(2);
                money -= 3;
            }

            if ((int)money / 2 > 0)
            {
                result = result.AddHours(1);
                money -= 2;
            }

            if ((int)money / 1 > 0)
            {
                result = result.AddMinutes(20);
                money -= 1;
            }

            return result;
        }

        public bool IsParkingFree()
        {
            return IsParkingFree(DateTime.Now);
        }

        public bool IsParkingFree(DateTime dateTime)
        {
            return dateTime.Month == 8
                   || _publicHolidays.Contains(dateTime.Date)
                   || !(dateTime.DayOfWeek > 0 && (int)dateTime.DayOfWeek < 7
                        && (dateTime.Hour >= 8 && dateTime <= new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 12, 0, 0)
                            || dateTime.Hour >= 14 && dateTime <= new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 18, 0, 0)));
        }

    }

    class Program
    {
        static void Main(string[] args)
        {
            IParkingCalculator parkingCalculator = new EuropeanParking();

            // Check for free parking
            void CheckForFreeParking()
            {
                if (parkingCalculator.IsParkingFree())
                {
                    ClearAndPrint(new[] { " Parking is free! Enjoy!" });
                    Console.ReadKey();
                    Environment.Exit(0);
                }
            }
            CheckForFreeParking();

            // Get user input
            ClearAndPrint(new[] { " Enter Coin : " });
            string rawInput = Console.ReadLine();

            // Run contineously till user choose to quit
            while (rawInput.ToLower() != "q")
            {
                // Check if parking is free
                CheckForFreeParking();

                var coinValue = float.Parse(rawInput);

                // Get the result
                var isCoinAllowed = parkingCalculator.IsCoinAllowed(coinValue);
                DateTime result = parkingCalculator.CalculateTime(isCoinAllowed ? coinValue : 0);

                ClearAndPrint(new[]
                {
                    !isCoinAllowed ? $" {coinValue} is not a valid coin! Please try again!\n\r" : "",
                    $" Total money collected : {parkingCalculator.TotalCollection}",
                    $" You can park your vehicle till : {result}",
                    " Enter coin : "
                });

                rawInput = Console.ReadLine();
            }
        }


        /// <summary>
        /// Clear the screen and print the value on screen.
        /// </summary>
        /// <param name="value"></param>
        static void ClearAndPrint(string[] value)
        {
            Console.Clear();
            value.ToList().ForEach(a =>
            {
                Console.Write(Environment.NewLine + a);
            });
        }
    }
}




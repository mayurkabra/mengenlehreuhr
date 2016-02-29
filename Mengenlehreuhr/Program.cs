using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mengenlehreuhr
{

    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine(System.Environment.NewLine + "Please select one of the following options for conversion to Mengenlehreuhr format :- ");
                Console.WriteLine("a. Input as hh:mm:ss ");
                Console.WriteLine("b. Convert using DateTime Object. The current time will be taken as input.");
                Console.WriteLine("c. Exit.");
                Boolean exit = false;

                char input = Console.ReadKey().KeyChar;
                switch (input)
                {
                    case 'a':
                        {
                            Console.WriteLine(System.Environment.NewLine + "Please enter an input in the format of hh:mm:ss");
                            Console.WriteLine(System.Environment.NewLine + "00:00:00 to 23:59:59");
                            String dateInput = Console.ReadLine().ToString();
                            try
                            {
                                Console.WriteLine(System.Environment.NewLine + Clock.ConvertTime(dateInput));
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                            }
                            break;
                        }
                    case 'b':
                        {
                            Console.WriteLine(System.Environment.NewLine + Clock.ConvertTime(DateTime.Now));
                            break;
                        }
                    case 'c':
                        {
                            exit = true;
                            break;
                        }
                    default:
                        {
                            Console.WriteLine(System.Environment.NewLine + "Please select a valid option.");
                            break;
                        }
                }
                if (exit)
                {
                    break;
                }
            }
        }
    }

    partial class Clock
    {
        //Following contains the customizable area
        //You can customize the amount by which each 'primary' unit increases (eg:- By default its after every 5 hours that the first line of hours increase
        //You can customize the color of the light
        //You can add a checkpoint and its color (eg:- By default the primary line of minutes has a 'red' checkpoint on every 3rd unit)
        private static readonly int DEFAULT_HOUR_PRIMARY_UNIT = 5;
        private static readonly string DEFAULT_HOUR_PRIMARY_UNIT_COLOR = "R";
        private static readonly int DEFAULT_HOUR_PRIMARY_CHECKPOINT_UNIT = DEFAULT_NO_CHECKPOINT_UNIT;
        private static readonly string DEFAULT_HOUR_PRIMARY_CHECKPOINT_UNIT_COLOR = DEFAULT_NO_CHECKPOINT_COLOR;

        private static readonly string DEFAULT_HOUR_SECONDARY_UNIT_COLOR = "R";
        private static readonly int DEFAULT_HOUR_SECONDARY_CHECKPOINT_UNIT = DEFAULT_NO_CHECKPOINT_UNIT;
        private static readonly string DEFAULT_HOUR_SECONDARY_CHECKPOINT_UNIT_COLOR = DEFAULT_NO_CHECKPOINT_COLOR;

        private static readonly int DEFAULT_MINUTE_PRIMARY_UNIT = 5;
        private static readonly string DEFAULT_MINUTE_PRIMARY_UNIT_COLOR = "Y";
        private static readonly int DEFAULT_MINUTE_PRIMARY_CHECKPOINT_UNIT = 3;
        private static readonly string DEFAULT_MINUTE_PRIMARY_CHECKPOINT_UNIT_COLOR = "R";

        private static readonly string DEFAULT_MINUTE_SECONDARY_UNIT_COLOR = "Y";
        private static readonly int DEFAULT_MINUTE_SECONDARY_CHECKPOINT_UNIT = DEFAULT_NO_CHECKPOINT_UNIT;
        private static readonly string DEFAULT_MINUTE_SECONDARY_CHECKPOINT_UNIT_COLOR = DEFAULT_NO_CHECKPOINT_COLOR;

        private static readonly int DEFAULT_SECOND_UNIT = 1;
        private static readonly string DEFAULT_SECOND_UNIT_COLOR = "Y";
        private static readonly int DEFAULT_SECOND_CHECKPOINT_UNIT = DEFAULT_NO_CHECKPOINT_UNIT;
        private static readonly string DEFAULT_SECOND_CHECKPOINT_UNIT_COLOR = DEFAULT_NO_CHECKPOINT_COLOR;

        internal static string ConvertTime(string v)
        {
            int hours=-1;
            int minutes = -1;
            int seconds = -1;
            String[] times = v.Split(':');
            try
            {
                hours = int.Parse(times[0]);
                minutes = int.Parse(times[1]);
                seconds = int.Parse(times[2]);
            }
            catch (Exception e)
            {
                throw new Exception("Please check the format of the string.",e);
            }
            return ConvertTime(hours, minutes, seconds);
        }

        internal static string ConvertTime(DateTime dateTime)
        {
            return ConvertTime(dateTime.Hour, dateTime.Minute, dateTime.Second);
        }

        internal static string ConvertTime(int hours, int minutes, int seconds)
        {
            if (hours<0 || hours>23 || minutes<0 || minutes>59 || seconds<0|| seconds>59)
            {
                throw new Exception("Time units out of limit.");
            }

            StringBuilder finalString = new StringBuilder();
            
            finalString.Append(LevelStatusString(DEFAULT_SECOND_MAXIMUM, DEFAULT_SECOND_UNIT, DEFAULT_SECOND_UNIT_COLOR, DEFAULT_SECOND_CHECKPOINT_UNIT, DEFAULT_SECOND_CHECKPOINT_UNIT_COLOR, (seconds+1)%2));
            finalString.Append(" ");
            finalString.Append(LevelStatusString(DEFAULT_HOUR_PRIMARY_MAXIMUM, DEFAULT_HOUR_PRIMARY_UNIT, DEFAULT_HOUR_PRIMARY_UNIT_COLOR, DEFAULT_HOUR_PRIMARY_CHECKPOINT_UNIT, DEFAULT_HOUR_PRIMARY_CHECKPOINT_UNIT_COLOR, hours));
            finalString.Append(" ");
            finalString.Append(LevelStatusString(DEFAULT_HOUR_SECONDARY_MAXIMUM, DEFAULT_HOUR_SECONDARY_UNIT, DEFAULT_HOUR_SECONDARY_UNIT_COLOR, DEFAULT_HOUR_SECONDARY_CHECKPOINT_UNIT, DEFAULT_HOUR_SECONDARY_CHECKPOINT_UNIT_COLOR, hours % DEFAULT_HOUR_PRIMARY_UNIT));
            finalString.Append(" ");
            finalString.Append(LevelStatusString(DEFAULT_MINUTE_PRIMARY_MAXIMUM, DEFAULT_MINUTE_PRIMARY_UNIT, DEFAULT_MINUTE_PRIMARY_UNIT_COLOR, DEFAULT_MINUTE_PRIMARY_CHECKPOINT_UNIT, DEFAULT_MINUTE_PRIMARY_CHECKPOINT_UNIT_COLOR, minutes));
            finalString.Append(" ");
            finalString.Append(LevelStatusString(DEFAULT_MINUTE_SECONDARY_MAXIMUM, DEFAULT_MINUTE_SECONDARY_UNIT, DEFAULT_MINUTE_SECONDARY_UNIT_COLOR, DEFAULT_MINUTE_SECONDARY_CHECKPOINT_UNIT, DEFAULT_MINUTE_SECONDARY_CHECKPOINT_UNIT_COLOR, minutes % DEFAULT_MINUTE_PRIMARY_UNIT));

            return finalString.ToString();
        }

        internal static string LevelStatusString(int maximum, int unit, string unitColour, int checkPointUnit, string checkpointColor, int value)
        {
            int noOfTotalLamps = maximum / unit;
            int noOfLitLamps = value / unit;

            StringBuilder stringBuilder = new StringBuilder();
            for (int lampNumber=1;lampNumber<= noOfLitLamps; lampNumber++)
            {
                AddLampToString(stringBuilder, lampNumber, unitColour, checkPointUnit, checkpointColor);
            }
            for (int lampNumber = noOfLitLamps+1; lampNumber <= noOfTotalLamps; lampNumber++)
            {
                stringBuilder.Append(DEFAULT_OFF);
            }
            return stringBuilder.ToString();
        }

        private static void AddLampToString(StringBuilder stringBuilder, int lampNumber, string unitColour, int checkPointUnit, string checkpointColor)
        {
            if (checkPointUnit > 0 && lampNumber % checkPointUnit == 0)
            {
                stringBuilder.Append(checkpointColor);
            }
            else
            {
                stringBuilder.Append(unitColour);
            }
        }
    }

    partial class Clock
    {
        private static readonly string DEFAULT_OFF = "O";
        private static readonly int DEFAULT_NO_CHECKPOINT_UNIT = -1;
        private static readonly string DEFAULT_NO_CHECKPOINT_COLOR = "";

        private static readonly int DEFAULT_HOUR_SECONDARY_UNIT = 1;
        private static readonly int DEFAULT_MINUTE_SECONDARY_UNIT = 1;

        private static readonly int TOTAL_HOURS = 24;
        private static readonly int TOTAL_MINUTES = 59;

        private static readonly int DEFAULT_HOUR_PRIMARY_MAXIMUM = (TOTAL_HOURS/ DEFAULT_HOUR_PRIMARY_UNIT)* DEFAULT_HOUR_PRIMARY_UNIT;
        private static readonly int DEFAULT_HOUR_SECONDARY_MAXIMUM = DEFAULT_HOUR_PRIMARY_UNIT-1;
        private static readonly int DEFAULT_MINUTE_PRIMARY_MAXIMUM = (TOTAL_MINUTES / DEFAULT_MINUTE_PRIMARY_UNIT)* DEFAULT_MINUTE_PRIMARY_UNIT;
        private static readonly int DEFAULT_MINUTE_SECONDARY_MAXIMUM = DEFAULT_MINUTE_PRIMARY_UNIT - 1;
        private static readonly int DEFAULT_SECOND_MAXIMUM = 1;
    }
}

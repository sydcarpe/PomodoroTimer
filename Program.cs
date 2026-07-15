using System;
using System.Diagnostics;
using System.Timers;
using NAudio.Wave;

namespace TaskTracker
{
    class Program
    {


        public static int timeLeftinSession = 0; // countdown 
        public static int unchangingSessionTime; // to reset timeLeftinSession back 
        public static int shortBreakTime;
        public static int unchangingBreakTime;
        public static int sessionsLeft; // default 0
        public static int unchangingSessions;
        public static bool onBreak = false; // automatically set to false since you do not start on a break 
        public static int longBreakTime;
        public static int unchangingLongBreak;
        public static System.Timers.Timer myTimer = new System.Timers.Timer();


        public static void Main(string[] args)
        {
            myTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);

            //First session
            Console.WriteLine("Enter in the Session time (minutes) - ");
            string userInputSession = Console.ReadLine();
            int userInputSesTime = Int32.Parse(userInputSession) *60; //converting to minutes 

            //Break time var
            Console.WriteLine("Enter in the short break time - ");
            var userInputShortBreak = Console.ReadLine();
            int userInputShort = Int32.Parse(userInputShortBreak) *60; //converting to minutes
            shortBreakTime = userInputShort;


            //How many sessions the user will have
            Console.WriteLine("How many sessions would you like?");
            string userInputSessionTotal = Console.ReadLine();
            int userInputSessionRead = Int32.Parse(userInputSessionTotal); //no need to convert 

            //long break time

            Console.WriteLine("How long is your long break after sessions?");
            string userInptLongBreak = Console.ReadLine();
            int userInputLongBrk = Int32.Parse(userInptLongBreak) *60; //converting to minutes
            longBreakTime = userInputLongBrk;


            //time left is set to userInputSesTime and sessionTotal is so we can reset it 
            timeLeftinSession = userInputSesTime;
            unchangingSessionTime = userInputSesTime;
            sessionsLeft = userInputSessionRead;
            unchangingSessions = userInputSessionRead;
            unchangingBreakTime = userInputShort;
            unchangingLongBreak = userInputLongBrk;

            myTimer.Interval = 1000; // this will run every second. 
            myTimer.Enabled = true;

            Console.WriteLine("Press \'q\' to quit.");
            while (Console.Read() != 'q') ;
        }

        static void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            //After x seconds this will all run...
            int seconds = timeLeftinSession % 60;
            int minutes = timeLeftinSession / 60;
            string timeLeftDisplay = minutes + ":" + seconds;

            // if the break is false (not on a break) it will run session time 
            // if the break is true... it will run the break
            if (!onBreak)
            {
                Console.BackgroundColor = ConsoleColor.Yellow;
                if (timeLeftinSession > 0)
                {
                    Console.Write($"\r" + timeLeftDisplay);
                    timeLeftinSession--; //down by the second
                }
                else if (timeLeftinSession == 0)
                {
                    if (sessionsLeft != 0)
                    {
                        sessionsLeft--;
                        onBreak = true;
                        shortBreakTime = unchangingBreakTime;
                        longBreakTime = unchangingLongBreak;
                        //playing a sound below
                        Process.Start("afplay", "SynthChime9.mp3");
                        Console.ResetColor();

                        //if no sessions say it is time for long break
                        if (sessionsLeft == 0)
                        {
                            Console.WriteLine("Great job! Time for a long break!");
                        }
                        else
                        {
                            Console.WriteLine("Great job!! It's time for a break!");

                        }

                    }
                }
            }
            else if (onBreak) //if break is set to true do this
            {
                //if sessions are done then do long break, if there is more than 0 sessions left, do short break 
                if (sessionsLeft > 0)
                {
                    Console.BackgroundColor = ConsoleColor.Green;
                    int breakSeconds = shortBreakTime % 60;
                    int breakMinutes = shortBreakTime / 60;
                    string breakDisplay = breakMinutes + ":" + breakSeconds;
                    if (shortBreakTime != 0)
                    {
                        Console.Write($"\r" + breakDisplay);
                        shortBreakTime--;
                    }
                    else if (shortBreakTime == 0 && sessionsLeft != 0)
                    {
                        Process.Start("afplay", "PowerUp2.mp3");
                        Console.ResetColor();
                        Console.WriteLine("Break over! You have " + sessionsLeft + " sessions left!");
                        onBreak = false;
                        if (sessionsLeft != 0)
                        {
                            timeLeftinSession = unchangingSessionTime;
                        }
                    }

                }
                else if (sessionsLeft == 0)
                {
                    Console.BackgroundColor = ConsoleColor.Magenta;
                    int longBreakSeconds = longBreakTime % 60;
                    int longBreakMin = longBreakTime / 60;
                    string longBreakDisplay = longBreakMin + ":" + longBreakSeconds;

                    if (longBreakTime != 0)
                    {
                        Console.Write($"\r" + longBreakDisplay);
                        longBreakTime--;
                    }
                    else
                    {
                        Process.Start("afplay", "PowerUp2.mp3");
                        Console.ResetColor();
                        Console.WriteLine("Long break is over! Sessions will begin again automatically");
                        sessionsLeft = unchangingSessions;
                        onBreak = false;
                        timeLeftinSession = unchangingSessionTime;
                        sessionsLeft = unchangingSessions;
                    }
                }

            }
        }
    }
}
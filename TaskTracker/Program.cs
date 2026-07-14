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
        public static bool onBreak = false; // automatically set to false since you do not start on a break 
        public static System.Timers.Timer myTimer = new System.Timers.Timer();
        

        public static void Main(string[] args)
        {
            myTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);

            //First session
            Console.WriteLine("Enter in the Session time (minutes) - ");
            string userInputSession = Console.ReadLine();
            int userInputSesTime = Int32.Parse(userInputSession)* 60; //converting to minutes 

            //Break time var
            Console.WriteLine("Enter in the short break time - ");
            var userInputShortBreak = Console.ReadLine();
            int userInputShort = Int32.Parse(userInputShortBreak) *60; //converting to minutes
            shortBreakTime = userInputShort;


            //How many sessions the user will have
            Console.WriteLine("How many sessions would you like?");
            string userInputSessionTotal = Console.ReadLine();
            int userInputSessionRead = Int32.Parse(userInputSessionTotal); //no need to convert 


            //time left is set to userInputSesTime and sessionTotal is so we can reset it 
            timeLeftinSession = userInputSesTime;
            unchangingSessionTime = userInputSesTime;
            sessionsLeft = userInputSessionRead;
            unchangingBreakTime = userInputShort;

            myTimer.Interval = 1000; // this will run every second. 
            myTimer.Enabled = true;

            Console.WriteLine("Press \'q\' to quit.");
            while (Console.Read() != 'q') ;
        }

        static void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            //After x seconds this will all run...

            // if the break is false it will run session time 
            // if the break is true... it will run the break
            if (!onBreak)
            {
                if (sessionsLeft == 0)
                {
                    Console.WriteLine("Pomodoro over! Type q to restart");
                    myTimer.Stop();
                    return;
                }

                else if (timeLeftinSession > 0)
                {
                    timeLeftinSession--; //down by the second
                } 
                else if (timeLeftinSession == 0)
                {
                    sessionsLeft--;
                    onBreak = true;
                    shortBreakTime = unchangingBreakTime;
                    //playing a sound below
                    Process.Start("afplay", "bellSound.mp3");
                }
            }
            else if (onBreak) //if break is set to true do this
            {
                if (shortBreakTime != 0)
                {
                    Console.WriteLine("ON BREAK! " + shortBreakTime);
                    shortBreakTime--;
                }
                else if (shortBreakTime == 0 && sessionsLeft != 0)
                {
                    Console.WriteLine("Break over! You have " + sessionsLeft + " sessions left!" );
                    onBreak = false;
                    if (sessionsLeft != 0)
                    {
                        timeLeftinSession = unchangingSessionTime;
                    }
                }
                else if(sessionsLeft == 0)
                {
                    Console.WriteLine("Pomodoro over! Type q to restart");
                    myTimer.Stop();
                    return;
                }

            }
        }
    }
}
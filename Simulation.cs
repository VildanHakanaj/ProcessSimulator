using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment_2COIS2020H
{
    public class Simulation
    {
        #region Variable declaration

        #region Integers + Array
        private int[] Processors; //An array of processors 
        private int P;
        private int jobCount;
        private int totalJobs = 1; //Total amount of jobs processed
        #endregion
        #region Double
        private int MeanIA, MeanPT;
        private double clock;
        private double startWaiting = 0; //Store the times at which the jobs enter the waiting queue
        private double endWaiting = 0;   //Store the times at which the jobs leave the waiting queue
        #endregion

        Random rnd = new Random(); //Random object for logorithmic formula

        bool check = true;  //Bool to display tiem limit reached method only once

        public PriorityQueue<Event> priorityQueue = new PriorityQueue<Event>(1000);  //Create the priority queue
        public Queue<Job> waitingQueue = new Queue<Job>();  //Create the waiting queue 
        #endregion


        /// <summary>
        /// Set the mean interArrival, mean execution time and the number of processors to what user inputs
        /// </summary>
        /// <param name="MeanIA">Inter arrival time </param>
        /// <param name="MeanPT">mean Execution time </param>
        /// <param name="P">NUmber of processors</param>
        public Simulation(int MeanIA, int MeanPT, int P)
        {
            this.MeanIA = MeanIA;
            this.MeanPT = MeanPT;
            jobCount = 0;
            this.P = P;
            Processors = new int[P];
        }

        //run the simulation
        public void Simulate()
        {

            CreateArrival(); //Create an event and a job
            while (!priorityQueue.Empty() || waitingQueue.Count != 0) //Do while the waiting queue is not empty or the priority queue is not empty
            {

                Event myEvent = priorityQueue.Front(); //Take the front of the priority queue
                priorityQueue.Remove();  //Remove the event from the queue

                Job myJob = myEvent.myJob; //Get the job from the event.

                clock = myEvent.eventTime;  //Update clock
                if (myEvent.flag)  //Event is an arrival
                {
                    if (clock < 7200) //If time limit isn't reached, create arrival
                    {
                        CreateArrival(); //Create a new arrival;
                    }

                    if (clock >= 7200 && check)  //If time limit is reached and check is true
                    {
                        ConsoleColor original = Console.ForegroundColor; //Store original text color
                        Console.ForegroundColor = ConsoleColor.Green;    //Change text colour to green
                        Console.WriteLine("Time limit reached, jobs stopped being created\nEmptying queues in progress\n");
                        Console.ForegroundColor = original;  //Set text colour back to original
                        check = false;  //Set bool check to false so message is only displayed once
                    }

                    if (myJob.jobType == 0) // If the job is an interrup
                    {
                        ConsoleColor original = Console.ForegroundColor;  //Store original text color
                        Console.ForegroundColor = ConsoleColor.Red;       //Change text colour to red
                        int index = myJob.processorPos; //Grab the postion from the interrupt job
                        if (Processors[index] == 1)     //Check if the processors is executing a regular job
                        {
                            Event interruptEvent;
                            Job interruptJob;
                            interruptEvent = priorityQueue.RemoveEvent(index); //Remove the departure event from the queue and get the job;
                             
                            interruptJob = interruptEvent.myJob; //Get the job from the removed event

                            interruptJob.processTime = interruptJob.processTime - (clock - (interruptEvent.eventTime - interruptJob.processTime)); //Calculate the time that it has left to execute
                            waitingQueue.Enqueue(interruptJob);   //Store the job that was interrupted into the waiting queue
                            startWaiting = startWaiting + clock;  //Update clock
                            Processors[index] = 2;  //Set the processors index = 2 to tell that the job is an interrupt

                            Event departureEvent = new Event(myJob, index, clock + myJob.processTime, false); //Create a departure event for the interrupt
                            priorityQueue.Add(departureEvent);  //Add departure event to Priority Queue
                            Console.WriteLine("Job {0} arrives in processor {1} at {2}, interupting job {3}\nJob {0} should finish at {4}\n", myJob.jobID, myJob.processorPos, Time(clock), interruptJob.jobID, Time(myJob.processTime + clock));//Interupt info
                        }
                        else if (Processors[index] == 0) //Else if the processor is empty store the interrup
                        {
                            Processors[index] = 2; //Set the processors index = 2 to tell that the job is an interrupt
                            Event departureEvent = new Event(myJob, index, clock + myJob.processTime, false); //Create a departure event for the interrupt
                            Console.WriteLine("Job {0} arrives in processor {1} at {2} and should finish at {3}\n", myJob.jobID, myJob.processorPos, Time(clock), Time(myJob.processTime + clock));//Interupt info
                            priorityQueue.Add(departureEvent); //Add the departure to Priority queue
                        }
                        else if (Processors[index] == 2)  //Else if processor is busy with an interrupt, forget new job
                        {
                            Console.WriteLine("Interrupt for processor {0} ingnored at {1}\n", myJob.processorPos, Time(clock));
                        }
                        Console.ForegroundColor = original; //reset color to original


                    }
                    else  //Job is regular
                    {
                        int procIndex = FindProcessor();
                        if (procIndex >= 0) //If the processor its free
                        {
                            Processors[procIndex] = 1; //Occupy the processor
                            Console.WriteLine("Job {0} arrives in processor {2} at {1} and should finish at {3}\n", myJob.jobID, Time(myEvent.eventTime), procIndex, Time(clock + myJob.processTime)); //Display when the job arrived
                            Event DepartureEvent = new Event(myJob, procIndex, clock + myJob.processTime, false); //We create a departure event 
                            //And we set the flag to false to indicate departure 

                            priorityQueue.Add(DepartureEvent); //Add the departure event into the Priority queue
                        }
                        else  //All processors are full
                        {
                            waitingQueue.Enqueue(myJob);          //Put the job into the waiting queue.
                            startWaiting = startWaiting + clock;  //Update 1st variable to calculate average waiting time
                            Console.WriteLine("Job {0} arrives and is placed in the waiting queue\n", myJob.jobID);
                        }

                    }
                }
                else //Even is a departure
                {
                    Job newJob;
                    Event DepartureEvent;
                    Processors[myEvent.indexer] = 0; //Remove event from its processor
                    if (waitingQueue.Count != 0)     //If waitign queue isn't empty, pull job from there
                    {
                        newJob = waitingQueue.Dequeue();  //Grab job from waiting queue
                        endWaiting = endWaiting + clock;  //Update 2nd varialble to calulate average waiting time
                        DepartureEvent = new Event(newJob, myEvent.indexer, clock + newJob.processTime, false); //We create a departure event 
                        priorityQueue.Add(DepartureEvent); //Add new departure to waiting queue
                        Console.WriteLine("Job {0} finihsed execution on processor {1} at {2}\nJob {3} begins execution and should finish at {4}\n", myJob.jobID, myEvent.indexer, Time(clock), newJob.jobID, Time(clock + newJob.processTime));
                    }
                    else //If waiting queue is empty
                    {
                        Console.WriteLine("Job {0} finished execution at {1}\n", myJob.jobID, Time(myEvent.eventTime));
                    }

                }
            }//End of while 
            Console.WriteLine("\n\nSimulation finished at {0}", Time(clock));  //End of simulation


            //Calulate the average waiting time
            double avgWaitTime = 0;
            avgWaitTime = (endWaiting - startWaiting) / totalJobs;  //(Time at which jobs left waiting queue - tiem at which jobs left waiting queue) / total jobs processed
            Console.WriteLine("\n{0} jobs were processed and they waited on average {1:N} seconds", totalJobs, avgWaitTime);
        
        }

        /// <summary>
        /// Finds available processor for regular job 
        /// </summary>
        /// <returns></returns>
        public int FindProcessor()
        {
            int index = 0;

            while (index < P && Processors[index] > 0) //While the index is within the array && the processor is taken keep looping
            {
                index++; //increase index;
            }
            if (index == P) //Check if the index is = to P that means that the index is at the end of the array se we should decrement
            {
                index = -1; //There is no available processor
            }

            return index;
        }


        /// <summary>
        /// Creates arrivals 
        /// </summary>
        /// <returns></returns>
        public void CreateArrival()
        {

            #region Variable declaration && job class creation
            int jobNum, procNum; //The job number and the number of processors
            double interArrival, u, exeTime;
            Job job;
            #endregion

            #region Random Generations
            u = rnd.NextDouble(); //Uniform u
            interArrival = Math.Round(Math.Log(u) * MeanIA * -1, 3);//InterArrivale time;
            u = rnd.NextDouble(); //Uniform u
            exeTime = Math.Round(Math.Log(u) * MeanPT * -1, 3); //Time it takes to execute
            jobNum = rnd.Next(0, 9); //randomly generate a number from 1 - 10;
            procNum = rnd.Next(1, P); //Random number of processors form 1 - 3;
            #endregion

            jobCount++;//Increase the job count 
            if (jobNum == 0) //Check if is an interrupt
            {
                job = new Job(0, exeTime, procNum, P); //set the job number to 0;
                totalJobs++; //Keep track of how many jobs are created
            }
            else
            {
                job = new Job(jobCount, exeTime, procNum, P);
                totalJobs++; //Keep track of how many jobs are created
            }

            for (int i = 0; i < job.p; i++) //For loop to create as many events as the job requires processors
            {
                Event myEvent = new Event(job, clock + interArrival, interArrival); //Create the event and pass the time for the arrival
                priorityQueue.Add(myEvent); //Add the event to the queue;
            }
        }

        /// <summary>
        /// Converts thime from seconds to (hour : minutes : seconds) 
        /// </summary>
        /// <returns></returns>
        public string Time(double timeInSeconds)
        {
            int hours, seconds, minutes, days;
            string time;

            hours = (int)timeInSeconds / 3600;
            minutes = ((int)timeInSeconds % 3600) / 60;
            seconds = ((int)timeInSeconds % 3600) % 60;
            days = (int)timeInSeconds / 86400;

            if (days == 0)
            {
                time = string.Format("{0:D2}:{1:D2}:{2:D2}", hours, minutes, seconds);
            }
            else
                time = string.Format("{3} days, {0:D2}:{1:D2}:{2:D2}", hours, minutes, seconds, days);
            return time;
        }
    }
}
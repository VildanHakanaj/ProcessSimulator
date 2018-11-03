using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment_2COIS2020H
{
    /*
     * Event Class
     * 
     * Data Fields
     * 
     * [x] int Job time - stores the job time that has been passed to the event
     * [x] int Job number - stores the job number 
     * [x] Job count - stores how many jobs have been created
     * [x] interArrival - time it takes in between to create
     * [x] int processor number - number of processors it takes to complete the job 
     * [x] int timer - keeps track of the timer
     * [x] bool flag - to check if its an arrival or a departure
     * [x] int indexer - keeps track where the job is stored in which processor
     *
     * Class Constructor
     * [x] public Event(Job job, int timer) - get the job object to get the time of it, pass in the timer to know what time the event was created.
     * [x] public Event(Job job, int indexer, int timer) - This is for the departure event to store the indexer so we know where the job was stored
     * 
     * Class Methods
     * [x] CompareTo(object obj) - Compares the two event based on the time they were created.
     * 
     * 
     
         */




    public class Event : IComparable
    {
        public bool flag { get; set; }
        public double eventTime { get; set; }
        public int indexer { get; set; }
        public Job myJob { get; set; }
        public double interArrival;

        /// <summary>
        /// Arrival Event Constructor
        /// </summary>
        /// <param name="obj">Takes the Job Associated with the event</param>
        /// <param name="eventTime">The time that the event Arrived</param>
        /// <param name="A">The interArrival Time</param>
        public Event(Job obj, double eventTime, double A)
        {
            this.eventTime = eventTime;  //Get the timer from the main program
            myJob = obj;
            interArrival = A;
            flag = true;
        }


        /// <summary>
        /// Departure Constructor
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="indexer"></param>
        /// <param name="eventTime"></param>
        /// <param name="flag"></param>
        public Event(Job obj, int indexer, double eventTime, bool flag)
        {
            this.eventTime = eventTime;
            this.indexer = indexer;     //Store the position //Rename the var;
            myJob = obj;
            this.flag = flag;
        }

        /// <summary>
        /// Compares the event time from the Pirority Queue.
        /// </summary>
        /// <param name="obj">Pass the second event</param>
        /// <returns></returns>
        public int CompareTo(object obj) //The compare method.
        {

            Event newEvent = obj as Event; //Cast the obj as an event


            return (newEvent.eventTime.CompareTo(eventTime)); //If child is not bigger than the parent

        }
    }

}

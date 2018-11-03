using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment_2COIS2020H
{

    /*
     * Job Class
     * 
     * Data Fields
     * [x] JobNum - If < 2 Interrupt, if > 2 Regular, Have a static variable to increment every time it gets created;
     * [x] procNum - number of processors it takes to processes this job
     * [x] time - Time it takes to processes
     * [x] Job count - keeps track of how many jobs have been created.
     * 
     * Construcor
     * [x] Check if its a regular or a interrupt job
     * [x] Set the data fields to their values.
     * 
     * Methods 
     * [ ] N/A
     */

    public class Job
    {

        public int p { get; set; }              //Number of processors it takes
        public double processTime { get; set; } //Time it takes to process.
        public int jobType { get; set; }        //To check if the job is a regular or interrup
        public int processorPos { get; set; }   //Processor where interupt will go
        public int jobID { get; set; }          //Name of the job

        Random rnd = new Random();  //Random for interupt processor location

        /// <summary>
        /// Constructor for Job class
        /// </summary>
        /// <param name="jobType">Job number</param>
        /// <param name="time">Time it takes to process</param>
        /// <param name="p">number of processors it takes to perform this job</param>


        public Job(int jobNum, double processTime, int p, int P = 0)
        {
            if (jobNum == 0)  //Check if the job should an interupt
            {
                jobType = 0;  //Set job to interupt
                this.processTime = processTime;  //Set the execution time

                processorPos = rnd.Next(0, P - 1); //Pick a processor to go to
                this.p = 1; //Set the processor that it takes to one

            }
            else   //Regular job
            {
                jobType = 1;     //Set job to regular
                jobID = jobNum;  //Set job name
                this.processTime = processTime; //Set execution time
                this.p = p;  //How many processors does the job want
            }

        }
    }
}

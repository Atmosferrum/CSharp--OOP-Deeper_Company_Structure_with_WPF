﻿namespace OOP_Organization
{
    class Worker : Employee 
    {

        #region Constructor;

        /// <summary>
        /// Constructor DERIVED from Employee Class (But with differnet Salary)
        /// </summary>
        /// <param name="EmployeeName">Employee Name</param>
        /// <param name="LastName">Employee Last Name</param>
        /// <param name="Age">Employee Name</param>
        /// <param name="Department">Employee Department</param>
        /// <param name="DaysWorked">Employee Days Worked</param>
        public Worker(string EmployeeName,
                      string LastName,
                      int Age,
                      string Department,
                      int DaysWorked)
        : base(EmployeeName,
               LastName,
               Age,
               Department,
               DaysWorked)
        { }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public Worker() : this("", "", 0, "", 0) { }

        #endregion Constructor

        public override int DaysWorked //Days Worked Property
        {
            get { return this.daysWorked; }
            set { this.daysWorked = value;
                int rate = 4;
                int hours = 12;
                int shifts = 15;
                int factor = (int)(daysWorked / 4);
                Salary = rate * hours * shifts + factor;
            }
        }
    }
}

namespace OOP_Organization
{
    class Intern : Employee
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
        public Intern(string EmployeeName,
                      string LastName,
                      int Age,
                      string Department,
                      int DaysWorked)
            : base(EmployeeName,
                   LastName,
                   Age,
                   Department,
                   DaysWorked)
        {
            Salary = 500;
        }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public Intern() : this("", "", 0, "", 0) { }

        #endregion Constructor
    }
}

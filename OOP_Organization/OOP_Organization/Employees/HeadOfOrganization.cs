namespace OOP_Organization
{
    class HeadOfOrganization : Employee
    {
        #region Constructor;

        /// <summary>
        /// Constructor DERIVED from Employee Class
        /// </summary>
        /// <param name="EmployeeName">Employee Name</param>
        /// <param name="LastName">Employee Last Name</param>
        /// <param name="Age">Employee Name</param>
        /// <param name="Department">Employee Department</param>
        /// <param name="DaysWorked">Employee Days Worked</param>
        public HeadOfOrganization(string EmployeeName,
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
        public HeadOfOrganization() : this("", "", 0, "", 0) { }

        #endregion Constructor
    }
}



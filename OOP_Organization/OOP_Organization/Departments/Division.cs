namespace OOP_Organization
{
    class Division : Department
    {

        #region Constructor;

        /// <summary>
        /// Constructor DERIVED from Department Class
        /// </summary>
        /// <param name="DepartmentName">Name of the Division</param>
        /// <param name="ParentDepartment">Parent Department for the Division</param>
        public Division(string DepartmentName,                        
                        string ParentDepartment)
            : base(DepartmentName,
                  ParentDepartment)
        { }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public Division() : this("", "") { } 

        #endregion Constructor
    }
}

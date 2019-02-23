using System;

namespace Server.Resources.Shared.Helpers
{
    public class FilterParams
    {
        #region Global Properties
        // Global Properties
        public int ID { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 12;
        public bool IncludeChildren { get; set; } = false;
        public bool IncludeParents { get; set; } = false;
        public bool IsActive { get; set; } = true;
        public string Name { get; set; }
        public string Search { get; set; }
        public DateTime? MinDate { get; set; }
        public DateTime? MaxDate { get; set; }
        #endregion // Global Properties

        #region Constructors
        // Constructors
        public FilterParams() {}
        public FilterParams(int id, int pageNumber, bool includeChildren, bool includeParents, bool isActive, string name, string search) : this()
        {
            this.ID = id;
            this.PageNumber = pageNumber;
            this.IncludeChildren = includeChildren;
            this.IncludeParents = includeParents;
            this.IsActive = isActive;
            this.Name = name;
            this.Search = search;
        }
        #endregion // Constructors
    }
}
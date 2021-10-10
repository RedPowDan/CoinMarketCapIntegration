namespace FrontOffice.Models
{
    using System;

    public abstract class PaginateView
    {
        public abstract int GetPageNumber();

        public abstract int GetTotalPages();

        public bool HasPreviousPage
        {
            get
            {
                return (GetPageNumber() > 1);
            }
        }

        public bool HasNextPage
        {
            get
            {
                return (GetPageNumber() < GetTotalPages());
            }
        }
    }
}
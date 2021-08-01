using System;

namespace FileUploader.Models
{
    public class CustomerDataPoint
    {
        public DateTime TransactionDate { get; set; }
        public decimal CurrentBalance { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
    }
}
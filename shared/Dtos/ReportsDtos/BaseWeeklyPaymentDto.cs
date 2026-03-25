using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Dtos.ReportsDtos
{
    public abstract class BaseWeeklyPaymentDto
    {
        public int Worker_Id { get; set; }
        public string? Worker_Name { get; set; }
        public string? Worker_Type { get; set; }
        public decimal TotalEarned { get; set; }
        public decimal TotalAdvances { get; set; }
        public decimal TotalDeductions { get; set; }
        public decimal NetPayment { get; set; }
    }
    public class ViewWeeklyPaymentDto : BaseWeeklyPaymentDto
    {
        public List<PaymentLineDto> Lines { get; set; } = new List<PaymentLineDto>();
        public int TotalQuantity { get; set; }
    }
    public class ViewWeeklyPaymentForGirlsDto : BaseWeeklyPaymentDto
    {

    }
    public class PaymentLineDto
    {
        public string Model_Name { get; set; }
        public int Quantity { get; set; }
        public decimal PricePerUnit { get; set; }
        public decimal LineTotal { get; set; }
    }
}

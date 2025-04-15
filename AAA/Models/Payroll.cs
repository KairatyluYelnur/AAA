using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
namespace AAA.Models
{
    public class Payroll
    {
        public int Id { get; set; }

        [Required]
        public int EmployeeId { get; set; }

        [ForeignKey("EmployeeId")]
        public Employee? Employee { get; set; }

        [Required]
        [Display(Name = "Начало периода")]
        [DataType(DataType.Date)]
        public DateTime PeriodStart { get; set; }

        [Required]
        [Display(Name = "Конец периода")]
        [DataType(DataType.Date)]
        public DateTime PeriodEnd { get; set; }

        [Required]
        [Display(Name = "Отработано часов")]
        public double TotalHours { get; set; }

        [Required]
        [Display(Name = "Начислено зарплаты (₽)")]
        public decimal TotalPay { get; set; }
    }
}


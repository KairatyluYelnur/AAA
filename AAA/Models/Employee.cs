using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace AAA.Models
{
   
        public class Employee
        {
            public int Id { get; set; }

            [Required]
            [Display(Name = "ФИО")]
            public string FullName { get; set; } = string.Empty;

            [Display(Name = "Должность")]
            public string? Position { get; set; }

            [Display(Name = "Ставка (Т/ч)")]
            [Range(0, double.MaxValue)]
            public decimal HourlyRate { get; set; }

            public ICollection<WorkLog>? WorkLogs { get; set; }
        }
    }


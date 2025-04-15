using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace AAA.Models
{


    public class WorkLog
    {


        public int Id { get; set; }

        [Required]
        public int EmployeeId { get; set; }

        [ForeignKey("EmployeeId")]
        public Employee? Employee { get; set; }

        [Required]
        [Display(Name = "Начало работы")]
        public DateTime StartTime { get; set; }

        [Required]
        [Display(Name = "Окончание работы")]
        public DateTime EndTime { get; set; }

        [NotMapped]
        [Display(Name = "Продолжительность (часов)")]
        public double Duration
        {
            get
            {
                return Math.Round((EndTime - StartTime).TotalHours, 2);
            }
        }

        [NotMapped]
        [Display(Name = "Дата")]
        public DateTime Date => StartTime.Date;
    }


}


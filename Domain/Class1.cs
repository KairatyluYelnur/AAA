namespace Domain
{
   
        public class WorkTimeEntity
        {
            public long Id { get; set; }
            public required string EmployeeId { get; set; } // Ссылка на сотрудника
            public DateTime StartTime { get; set; }
            public DateTime? EndTime { get; set; } // Nullable, так как окончание может быть не зафиксировано
            public DateTime? BreakStart { get; set; }
            public DateTime? BreakEnd { get; set; }
            public TimeSpan TotalWorkTime { get; set; }
            public TimeSpan TotalBreakTime { get; set; }
            public bool IsCompleted { get; set; } = false;
        }
}

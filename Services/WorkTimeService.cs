using DAL.Repositories;
using Domain;

namespace Services
{
    public class WorkTimeService
    {
        private readonly IWorkTimeRepository _repository;
        private readonly IEmployeeRepository _employeeRepository;

        public WorkTimeService(
            IWorkTimeRepository repository,
            IEmployeeRepository employeeRepository)
        {
            _repository = repository;
            _employeeRepository = employeeRepository;
        }

        public async Task StartWorkAsync(string employeeId)
        {
            await ValidateEmployeeExists(employeeId);

            if (await _repository.HasActiveSessionAsync(employeeId))
            {
                throw new InvalidOperationException("У сотрудника уже есть активная сессия");
            }

            await _repository.AddAsync(new WorkTimeEntity
            {
                EmployeeId = employeeId,
                StartTime = DateTime.UtcNow,
                IsCompleted = false
            });
        }

        public async Task EndWorkAsync(string employeeId)
        {
            var session = await GetValidSession(employeeId);

            session.EndTime = DateTime.UtcNow;
            session.IsCompleted = true;
            session.TotalWorkTime = CalculateSessionDuration(session);

            await _repository.UpdateAsync(session);
        }

        public async Task StartBreakAsync(string employeeId)
        {
            var session = await GetValidSession(employeeId);

            if (session.BreakStart.HasValue)
            {
                throw new InvalidOperationException("Перерыв уже начат");
            }

            session.BreakStart = DateTime.UtcNow;
            await _repository.UpdateAsync(session);
        }

        public async Task EndBreakAsync(string employeeId)
        {
            var session = await GetValidSession(employeeId);

            if (!session.BreakStart.HasValue)
            {
                throw new InvalidOperationException("Перерыв не был начат");
            }

            session.BreakEnd = DateTime.UtcNow;
            session.TotalBreakTime = CalculateBreakDuration(session);
            await _repository.UpdateAsync(session);
        }

        public async Task<WorkTimeReport> GetDailyReportAsync(string employeeId, DateTime date)
        {
            await ValidateEmployeeExists(employeeId);

            var sessions = await _repository.GetEmployeeSessionsAsync(
                employeeId,
                date.Date,
                date.Date.AddDays(1));

            return new WorkTimeReport
            {
                EmployeeId = employeeId,
                Date = date,
                TotalWorkTime = CalculateTotalDuration(sessions, s => s.TotalWorkTime),
                TotalBreakTime = CalculateTotalDuration(sessions, s => s.TotalBreakTime),
                Sessions = sessions
            };
        }

        private async Task ValidateEmployeeExists(string employeeId)
        {
            if (!await _employeeRepository.ExistsAsync(employeeId))
            {
                throw new ArgumentException("Сотрудник не найден", nameof(employeeId));
            }
        }

        private async Task<WorkTimeEntity> GetValidSession(string employeeId)
        {
            return await _repository.GetCurrentSessionAsync(employeeId)
                ?? throw new InvalidOperationException("Активная сессия не найдена");
        }

        private TimeSpan CalculateSessionDuration(WorkTimeEntity session)
        {
            if (!session.EndTime.HasValue) return TimeSpan.Zero;

            var totalDuration = session.EndTime.Value - session.StartTime;
            return totalDuration - (session.TotalBreakTime ?? TimeSpan.Zero);
        }

        private TimeSpan CalculateBreakDuration(WorkTimeEntity session)
        {
            if (!session.BreakStart.HasValue || !session.BreakEnd.HasValue)
                return TimeSpan.Zero;

            return session.BreakEnd.Value - session.BreakStart.Value;
        }

        private static TimeSpan CalculateTotalDuration(
            IEnumerable<WorkTimeEntity> sessions,
            Func<WorkTimeEntity, TimeSpan?> selector)
        {
            return sessions.Sum(s => selector(s) ?? TimeSpan.Zero);
        }
    }

    public class WorkTimeReport
    {
        public required string EmployeeId { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan? TotalWorkTime { get; set; }
        public TimeSpan? TotalBreakTime { get; set; }
        public required IEnumerable<WorkTimeEntity> Sessions { get; set; }
    }
}
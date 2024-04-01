using Application.Behaviors;
using Application.Interfaces;
using Domain.Entities.CP;
using Domain.Entities.DB;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.CP.CPDT05
{
    public class Update
    {
        public class Command : Data, ICommand<int?>
        {
        }
        public class Data
        {
            public List<CpCapacityDetail> cpCapacityDetails { get; set; }
            public string Year { get; set; }
        }
        public class Handler : IRequestHandler<Command, int?>
        {
            private readonly ICleanDbContext _context;
            private readonly ICurrentUserAccessor _user;
            public Handler(ICleanDbContext context, ICurrentUserAccessor user)
            {
                _context = context;
                _user = user;
            }
            public async Task<int?> Handle(Command request, CancellationToken cancellationToken)
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("select * from db_employee de2 where de2.team_id = (select de.team_id from db_employee de where de.employee_code = (select su.employee_code from st_user su where su.user_id = @userId limit 1))");
                var employeeOfTeam = await _context.QueryAsync<DbEmployee>(sql.ToString(), new { userId = _user.UserId }, cancellationToken);
                int year = int.Parse(request.Year) - 543;
                int dayOfYear = new DateTime(year, 12, 31).DayOfYear;
                int holiday = _context.Set<DbHoliday>().Where(w => w.HolidayDate.Value.Year == year && w.HolidayEndDate.Value.Year == year).Count();
                double dayOfYearMinusHoliday = ((dayOfYear / 7) * 5) - holiday;
                double defaultMD = (dayOfYearMinusHoliday - (dayOfYearMinusHoliday * 0.2)) / 12;

                foreach (var item in employeeOfTeam)
                {
                    sql = new StringBuilder();
                    sql.AppendLine(@"select count(cd.capacity_detail_id) from cp_capacity c inner join cp_capacity_detail cd  on c.capacity_id = cd.capacity_id where c.year = @Year and cd.employee_code = @employee");
                    var checkEmp = await _context.QueryFirstOrDefaultAsync<int>(sql.ToString(), new { Year = request.Year, employee = item.EmployeeCode }, cancellationToken);
                    if (checkEmp == 0)
                    {
                        CpCapacityDetail cpCapacityDetail = new CpCapacityDetail();
                        cpCapacityDetail.EmployeeCode = item.EmployeeCode;
                        cpCapacityDetail.CapacityId = request.cpCapacityDetails[0].CapacityId;
                        cpCapacityDetail.M01 = defaultMD;
                        cpCapacityDetail.M02 = defaultMD;
                        cpCapacityDetail.M03 = defaultMD;
                        cpCapacityDetail.M04 = defaultMD;
                        cpCapacityDetail.M05 = defaultMD;
                        cpCapacityDetail.M06 = defaultMD;
                        cpCapacityDetail.M07 = defaultMD;
                        cpCapacityDetail.M08 = defaultMD;
                        cpCapacityDetail.M09 = defaultMD;
                        cpCapacityDetail.M10 = defaultMD;
                        cpCapacityDetail.M11 = defaultMD;
                        cpCapacityDetail.M12 = defaultMD;
                        _context.Set<CpCapacityDetail>().AddRange(cpCapacityDetail);
                    }
                }
                await _context.SaveChangesAsync(cancellationToken);
                return request.cpCapacityDetails[0].CapacityId;
            }
        }
    }
}




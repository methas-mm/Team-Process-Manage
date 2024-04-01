using Application.Behaviors;
using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities.DB;
using Domain.Entities.PM;
using Domain.Entities.ST;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.PM.PMDT20
{
    public class Create
    {
        public class Command : PmTaskBug, ICommand<int>
        {

        }

        public class Handler : IRequestHandler<Command, int>
        {
            private readonly ICleanDbContext _context;
            private readonly ICurrentUserAccessor _user;

            public Handler(ICleanDbContext context, ICurrentUserAccessor user)
            {
                _context = context;
                _user = user;
            }

            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
                if (_context.Set<PmTaskBug>().Any(i => i.CustomerId == request.CustomerId && i.ProjectId == request.ProjectId && i.ModuleDetailPlanId == request.ModuleDetailPlanId && i.ProgramId == request.ProgramId))
                    throw new RestException(HttpStatusCode.BadRequest, "message.STD00004", "label.SURT03.MenuCode");

                request.Status = "N";
                _context.Set<PmTaskBug>().Add((PmTaskBug)request);
                await _context.SaveChangesAsync(cancellationToken);
                var ProjectName = _context.Set<PmProject>().Where(i => i.ProjectId == request.ProjectId).
                    Select(s => this._user.Language == "th" ? s.ProjectNameTh : s.ProjectNameEn).FirstOrDefault();


                var token = _context.Set<StParameter>().Where(i => i.ParameterGroupCode == "Path" && i.ParameterCode == "LineToken").Select(s => s.ParameterValue).FirstOrDefault();
                var url = _context.Set<StParameter>().Where(i => i.ParameterGroupCode == "Path" && i.ParameterCode == "BugPath").Select(s => s.ParameterValue).FirstOrDefault();

                foreach (var item in request.PmTaskBugSubs)
                {
                    var EmpName = _context.Set<DbEmployee>().Where(i => i.EmployeeCode == item.EmployeeCodeAssign).
                        Select(s => this._user.Language == "th" ? s.FirstNameTh + " " + s.LastNameTh : s.FirstNameEn + " " + s.LastNameEn).FirstOrDefault();
                    var Priority = _context.Set<DbStatus>().Where(i => i.TableName == "pm_task_sub" && i.ColumnName == "priority" && i.StatusValue == item.Priority).
                        Select(s => this._user.Language == "th" ? s.StatusDescTh : s.StatusDescEn).FirstOrDefault();
                    var lineRequest = (HttpWebRequest)WebRequest.Create("https://notify-api.line.me/api/notify");
                    var message = string.Format("message=\n{0} [{1}] : {2}\nวันที่ครบกำหนด : {3} \nผู้ได้รับมอบหมาย {4}\n Url : {5} ", ProjectName, Priority, item.TaskBugSubName,item.DueDate.ToString(), EmpName, url);
                    var data = Encoding.UTF8.GetBytes(message);

                    lineRequest.Method = "POST";
                    lineRequest.ContentType = "application/x-www-form-urlencoded";
                    lineRequest.ContentLength = data.Length;
                    lineRequest.Headers.Add("Authorization", "Bearer "+ token);
                    using (var stream = lineRequest.GetRequestStream()) stream.Write(data, 0, data.Length);
                    var response = (HttpWebResponse)lineRequest.GetResponse();
                    var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                }
               
                return request.TaskBugId;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain.Entities.PM;
using MediatR;

namespace Application.Features.PM.PMRT10
{
    public class List
    {
        public class Query : IRequest<CustomerDTO>
        {
            public string Keyword { get; set; }
        }
        public class CustomerDTO
        {
            public IEnumerable<PmCustomer> CustomerList { get; set; }
        }
        public class Handler : IRequestHandler<Query, CustomerDTO>
        {
            private readonly ICleanDbContext _context;
            private readonly ICurrentUserAccessor _user;

            public Handler(ICleanDbContext context, ICurrentUserAccessor user)
            {
                _context = context;
                _user = user;
            }
            public async Task<CustomerDTO> Handle(Query request, CancellationToken cancellationToken)
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("select");
                sql.AppendLine(" pc.company_code as \"companyCode\", ");
                sql.AppendLine(" pc.customer_id as \"customerId\", ");
                sql.AppendLine(" pc.customer_code as \"customerCode\", ");
                sql.AppendLine(" pc.customer_name_th as \"customerNameTh\", ");
                sql.AppendLine(" pc.customer_name_en as \"customerNameEn\", ");
                sql.AppendLine(" pc.image_id as \"imageId\", ");
                sql.AppendLine(" pc.active AS \"active\",");
                sql.AppendLine(" pc.xmin AS \"rowVersion\"");
                sql.AppendLine(" from pm_customer pc ");

                if (!string.IsNullOrEmpty(request.Keyword))
                {
                    sql.AppendLine("WHERE       CONCAT(pc.customer_code,");
                    sql.AppendLine("                   pc.customer_name_th,");
                    sql.AppendLine("                   pc.customer_name_en)");
                    sql.AppendLine("            ILIKE CONCAT('%', @Keyword, '%')");
                }

                sql.AppendLine(" order by pc.customer_code");

                CustomerDTO Model = new CustomerDTO();
                Model.CustomerList = await _context.QueryAsync<PmCustomer>(sql.ToString(), new { Keyword = request.Keyword }, cancellationToken);
                return Model;


            }
        }
    }
}


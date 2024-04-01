using Application.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.DB.DBRT10
{
    public class MasterDependency
    {
        public class MasterList
        {
            public IEnumerable<dynamic> SubDistrictId { get; set; }
            public IEnumerable<dynamic> DistrictId { get; set; }
            public IEnumerable<dynamic> ProvinceId { get; set; }
        }


        public class Query : IRequest<MasterList>
        {
            public string PostalCode { get; set; }
            public int? ProvinceId { get; set; }
            public int? DistrictId { get; set; }
            public int? SubDistrictId { get; set; }
            public string Case { get; set; }

        }

        public class Handler : IRequestHandler<Query, MasterList>

        {
            private readonly ICleanDbContext _context;
            private readonly ICurrentUserAccessor _user;

            public Handler(ICleanDbContext context, ICurrentUserAccessor user)
            {
                _context = context;
                _user = user;
            }
            public async Task<MasterList> Handle(Query request, CancellationToken cancellationToken)
            {
                MasterList master = new MasterList();

                switch (request.Case)
                {
                    case "Province":
                        master.ProvinceId = await _context.QueryAsync<dynamic>(@"select distinct dpc.province_id as value , get_wording_lang(@lang,province_name_tha ,province_name_eng ) as text
                                                                                from db_postal_code dpc 
                                                                                inner join db_province dp on dpc.province_id = dp.province_id 
                                                                                inner join db_district dd on dpc.district_id = dd.district_id 
                                                                                inner join db_sub_district dsd on dpc.sub_district_id = dsd.sub_district_id 
                                                                                where dpc.postal_code = @PostCode ",
                                                                                new { lang = _user.Language, PostCode = request.PostalCode }, cancellationToken
                                                                                );
                        break;
                    case "District":
                        master.DistrictId = await _context.QueryAsync<dynamic>(@" select distinct dd.district_id as value , get_wording_lang(@lang,dd.district_name_tha ,dd.district_name_eng ) as text
                                                                                            from db_postal_code dpc 
                                                                                                  inner join db_province dp on dpc.province_id = dp.province_id 
                                                                                                  inner join db_district dd on dpc.district_id = dd.district_id 
                                                                                                  inner join db_sub_district dsd on dpc.sub_district_id = dsd.sub_district_id 
                                                                                                  where dpc.postal_code = @PostCode and dpc.province_id = @ProvinceId ",
                                                                                  new { lang = _user.Language, PostCode = request.PostalCode, ProvinceId = request.ProvinceId }, cancellationToken
                                                                                  );
                        break;
                    case "SubDistrict":
                        master.SubDistrictId = await _context.QueryAsync<dynamic>(@"select distinct dsd.sub_district_id as value , get_wording_lang(@lang,dsd.sub_district_name_tha ,dsd.sub_district_name_eng ) as text
                                                                                     from  db_sub_district dsd 
                                                                                where dsd.district_id = @DistrictId ",
                                                                                new { lang = _user.Language, PostCode = request.PostalCode, ProvinceId = request.ProvinceId, DistrictId = request.DistrictId }, cancellationToken
                                                                                );
                        break;
                }

                return master;

            }


        }
    }
}
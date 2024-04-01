using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities.ST;
using MediatR;

namespace Application.Features.ST.STRT03
{
    public class Detail
    {
        public class StProfileDTO : StProfile
        {
            public IEnumerable<StMenuProfileDTO> MenuProfiles { get; set; }
        }
        public class StMenuProfileDTO : StMenuProfile
        {
            public string MenuName { get; set; }
        }
        public class Query : IRequest<StProfileDTO>
        {
            public string ProfileCode { get; set; }
        }
        public class Handler : IRequestHandler<Query, StProfileDTO>
        {
            private readonly ICleanDbContext _context;
            private readonly ICurrentUserAccessor _user;

            public Handler(ICleanDbContext context, ICurrentUserAccessor user)
            {
                _context = context;
                _user = user;
            }

            public async Task<StProfileDTO> Handle(Query request, CancellationToken cancellationToken)
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("SELECT  profile_code AS \"profileCode\",");
                sql.AppendLine("        profile_desc AS \"profileDesc\",");
                sql.AppendLine("        active,");
                sql.AppendLine("        xmin AS \"rowVersion\"");
                sql.AppendLine("FROM    st_profile");
                sql.AppendLine("WHERE   profile_code = @ProfileCode");

                var profile = await _context.QueryFirstOrDefaultAsync<StProfileDTO>(sql.ToString(), new { ProfileCode = request.ProfileCode }, cancellationToken);

                if (profile == null)
                {
                    throw new RestException(HttpStatusCode.NotFound, "Message.NotFound");
                }
                sql = new StringBuilder();
                sql.AppendLine("SELECT      a.profile_code AS \"profileCode\",");
                sql.AppendLine("            a.menu_code AS \"menuCode\",");
                sql.AppendLine("            CASE @Language WHEN @Tha THEN b.menuNameTha ELSE COALESCE(b.menuNameEng, b.menuNameTha) END AS \"menuName\",");
                sql.AppendLine("            a.xmin AS \"rowVersion\"");
                sql.AppendLine("FROM        st_menu_profile AS a");
                sql.AppendLine("INNER JOIN	(SELECT	    x.menu_code,");
                sql.AppendLine("                        x.menu_name AS menuNameTha,");
                sql.AppendLine("                        y.menu_name AS menuNameEng,");
                sql.AppendLine("                        x.xmin AS rowVersionTha,");
                sql.AppendLine("                        y.xmin AS rowVersionEng");
                sql.AppendLine("            FROM 	    st_menu_label AS x");
                sql.AppendLine("            LEFT JOIN   (SELECT menu_code, menu_name, xmin FROM st_menu_label WHERE lang_code = @Eng::Lang) AS y");
                sql.AppendLine("            ON			x.menu_code = y.menu_code");
                sql.AppendLine("            WHERE 		x.lang_code = @Tha::Lang) AS b");
                sql.AppendLine("ON			a.menu_code = b.menu_code");
                sql.AppendLine("WHERE       a.profile_code = @ProfileCode");
                sql.AppendLine("ORDER BY    a.menu_code");
                profile.MenuProfiles = await _context.QueryAsync<StMenuProfileDTO>(sql.ToString(), new { Language = this._user.Language, Tha = "th", Eng = "en", ProfileCode = request.ProfileCode }, cancellationToken);
                return profile;
            }
        }
    }
}

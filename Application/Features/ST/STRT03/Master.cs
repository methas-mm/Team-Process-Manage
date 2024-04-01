﻿using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.ST.STRT03
{
    public class Master
    {
        public class Query : IRequest<MenuDTO>
        {
            public string Keyword { get; set; }
        }
        public class Menu
        {
            public string menuCode { get; set; }
            public string menuName { get; set; }
        }
        public class MenuDTO
        {
            public IEnumerable<Menu> menus { get; set; }
        }

        public class Handler : IRequestHandler<Query, MenuDTO>
        {
            private readonly ICleanDbContext _context;

            private readonly ICurrentUserAccessor _user;

            public Handler(ICleanDbContext context, ICurrentUserAccessor user)
            {
                _context = context;
                _user = user;
            }

            public async Task<MenuDTO> Handle(Query request, CancellationToken cancellationToken)
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("select      sm.menu_code as \"menuCode\",");
                sql.AppendLine("            case @Language when 'th' then sml_th.menu_name else coalesce(sml_en.menu_name ,sml_th.menu_name) end as \"menuName\"");
                sql.AppendLine("from        st_menu as sm");
                sql.AppendLine("left join   st_menu_label as sml_th");
                sql.AppendLine("on          sml_th.menu_code = sm.menu_code");
                sql.AppendLine("            and sml_th.lang_code = 'th'");
                sql.AppendLine("left join   st_menu_label as sml_en");
                sql.AppendLine("on          sml_en.menu_code = sm.menu_code");
                sql.AppendLine("            and sml_en.lang_code = 'en'");
                sql.AppendLine("where       sm.active = true");

                if (!string.IsNullOrWhiteSpace(request.Keyword))
                {
                    sql.AppendLine("        and concat(sm.menu_code,");
                    sql.AppendLine("                   sml_th.menu_name,");
                    sql.AppendLine("                   sml_en.menu_name)");
                    sql.AppendLine("            ilike concat('%', @Keyword, '%')");
                }

                sql.AppendLine("order by    sm.menu_code");
                MenuDTO Menu = new MenuDTO();
                Menu.menus = await _context.QueryAsync<Menu>(sql.ToString(), new { Keyword = request.Keyword, Language = this._user.Language }, cancellationToken);
                return Menu;
            }
        }
    }
}
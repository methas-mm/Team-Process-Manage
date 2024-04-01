using Application.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.SU.Menu
{
    public class List
    {
        public class Menu
        {
            public string MenuCode { get; set; }
            public string Name { get; set; }
            public string Type { get; set; }
            public string Icon { get; set; }
            public string State { get; set; }
            public bool Active { get; set; }
            public string MainMenu { get; set; }
            public List<Menu> SubMenus { get; set; }
        }
        
        public class Query : IRequest<IEnumerable<Menu>>
        {

        }

        public class Handler : IRequestHandler<Query, IEnumerable<Menu>>
        {
            private readonly ICleanDbContext _context;
            private readonly ICurrentUserAccessor _user;
            public Handler(ICleanDbContext context, ICurrentUserAccessor user)
            {
                _context = context;
                _user = user;
            }
            public async Task<IEnumerable<Menu>> Handle(Query request, CancellationToken cancellationToken)
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine(@"
                    select 		sm.menu_code,
			                    concat('menu.', sm.menu_code) ""name"",
			                    sp.program_path state,
			                    sm.main_menu mainMenu,
			                    sm.icon,
                                sm.active active
                    from 		st_menu sm
                    left join 	st_program sp
                    on 			sp.program_code = sm.program_code
                    where 		exists(	select 		'x'	
             		                    from 		st_menu_profile smp 	
                	                    inner join 	st_user_profile sup
                	                    on 			sup.profile_code = smp.profile_code 	
               		                    where 		smp.menu_code = sm.menu_code 	
                	 			                    and sup.user_id = @UserId)
                	 			                    and sm.active = true
                    order by 	sm.menu_code");
                var menus = await _context.QueryAsync<Menu>(sql.ToString(), new { UserId = _user.UserId }, cancellationToken);
                var root = new Menu();
                this.GetMenus(root, menus, null);
                return root.SubMenus;
            }

            public void GetMenus(Menu parent, IEnumerable<Menu> menus, string menuCode)
            {
                List<Menu> childs = menus.Where(o => o.MainMenu == menuCode).ToList();
                if (parent.SubMenus == null) parent.SubMenus = new List<Menu>();
                parent.SubMenus.AddRange(childs);
                foreach (Menu child in parent.SubMenus)
                    this.GetMenus(child, menus, child.MenuCode);
            }
        }
    }
}

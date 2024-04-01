using Domain.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces
{
    public interface ICurrentUserAccessor
    {
        int UserId { get; }
        string UserName { get; }
        string Company { get; }
        string Language { get; }
        string Program { get; }
    }
}

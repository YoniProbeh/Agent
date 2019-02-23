using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Server.Resources.Identity.Context;
using Server.Resources.Shared.DTOs;
using Server.Resources.Shared.Helpers;
using Server.Resources.Shared.Models;
using Server.Resources.Shared.Services;

namespace Server.Resources.Identity.Services
{
    public interface IIdentityRepository<TDomain> : IModelRepository<TDomain> where TDomain : BaseModel<TDomain>, new() {}
    public class IdentityRepository<TDomain> : ModelRepository<TDomain, IdentityDBContext>, IIdentityRepository<TDomain> where TDomain : BaseModel<TDomain>, new()
    {
        private readonly IdentityDBContext context;
        private readonly IMapper mapper;

        public IdentityRepository(IdentityDBContext context, IMapper mapper) : base(context, mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }
    }
}
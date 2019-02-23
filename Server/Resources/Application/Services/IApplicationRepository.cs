using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Server.Resources.Application.Context;
using Server.Resources.Application.Models;
using Server.Resources.Identity.Context;
using Server.Resources.Shared.DTOs;
using Server.Resources.Shared.Helpers;
using Server.Resources.Shared.Models;
using Server.Resources.Shared.Services;

namespace Server.Resources.Application.Services
{
    public interface IApplicationRepository<TDomain> : IModelRepository<TDomain> where TDomain : BaseModel<TDomain>, new() {}
    public class ApplicationRepository<TDomain> : ModelRepository<TDomain, ApplicationDBContext>, IApplicationRepository<TDomain> where TDomain : BaseModel<TDomain>, new() 
        {
            private readonly ApplicationDBContext context;
            private readonly IMapper mapper;

            public ApplicationRepository(ApplicationDBContext context, IMapper mapper) : base(context, mapper)
            {
                this.context = context;
                this.mapper = mapper;
            }
        }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Resources.Application.Context;
using Server.Resources.Application.DTOs;
using Server.Resources.Application.Models;
using Server.Resources.Application.Services;
using Server.Resources.Shared.Helpers;
using Server.Resources.Shared.Services;

namespace Server.Controllers.Application
{
    public class CategoriesController : BaseController<Category, CategoryDTO, CategoryResultDTO, IApplicationRepository<Category>>
    {
        public CategoriesController(IApplicationRepository<Category> modelRepo) : base(modelRepo) {}
    }
}
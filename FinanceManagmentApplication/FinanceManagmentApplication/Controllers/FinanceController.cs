﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinanceManagmentApplication.Models.ProjectModels;
using FinanceManagmentApplication.Services;
using FinanceManagmentApplication.Services.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FinanceManagmentApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FinanceController : ControllerBase
    {
        IFinanceService financeService { get; set; }

        public FinanceController(IFinanceService service)
        {
            financeService = service;
        }

        [HttpGet]
        [Route("Index")]
        public async Task<ActionResult<List<ProjectFinanceModel>>> Index()
        {
            return await financeService.GetFinanceInformationToProjects();
        }
    }
}

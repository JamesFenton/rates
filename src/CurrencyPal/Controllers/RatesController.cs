﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CurrencyPal.Services;
using CurrencyPal.Dto;

namespace CurrencyPal.Controllers
{
    [Route("api/[controller]")]
    public class RatesController : Controller
    {
        private readonly RateService _RateService;

        public RatesController(RateService rateService)
        {
            _RateService = rateService;
        }

        [HttpGet]
        public async Task<RatesDto> Get()
        {
            return await _RateService.GetExchangeRates();
        }
    }
}

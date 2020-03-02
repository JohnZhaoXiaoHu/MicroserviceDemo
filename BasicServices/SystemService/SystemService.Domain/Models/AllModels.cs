﻿using AutoMapper;
using ServiceCommon;
using ServiceCommon.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SystemService.Domain.Models
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Resource, ResourceData>();
        }
    }
}

﻿using System;
using AutoMapper;

namespace PlaygroundApi.Mapping
{
    /// <summary> 
    /// Configures AutoMapper. 
    /// </summary> 
    public class AutoMapperConfig
    {
        /// <summary> 
        /// Configures AutoMapper. Profiles are initialized. 
        /// </summary> 
        public static IMapper Configure(IServiceProvider serviceProvider)
        {
            var config = new MapperConfiguration(x =>
            {
                x.AddProfile(new PlaygroundProfile(serviceProvider));
                x.AllowNullCollections = true;
            });

            var mapper = config.CreateMapper();
            mapper.ConfigurationProvider.AssertConfigurationIsValid();

            return mapper;
        }
    }
}

﻿using Files.Application.Common.Exceptions;
using FluentValidation;
using Serilog;

namespace Files.Presentation.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder ConfigureBuilder(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((context, configuration)
            => configuration.ReadFrom.Configuration(context.Configuration));

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.ConfigureDatabaseContext(builder.Configuration);
        builder.Services.ConfigureMediator();
        builder.Services.ConfigureRepositories();
        builder.Services.ConfigureServices();
        builder.Services.ConfigureMassTransit(builder.Configuration);
        builder.Services.AddAutoMapper(typeof(InvalidFileIdBadRequestException).Assembly);
        builder.Services.AddValidatorsFromAssembly(typeof(InvalidFileIdBadRequestException).Assembly);
        
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        
        return builder;
    }
}
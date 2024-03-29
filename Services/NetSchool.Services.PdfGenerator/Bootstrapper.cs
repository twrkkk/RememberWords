﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text;

namespace NetSchool.Services.PdfGenerator;

public static class Bootstrapper
{
    public static IServiceCollection AddPdfGenerator(this IServiceCollection services, IConfiguration configuration = null)
    {
        services.AddSingleton(typeof(PdfGenerator));

        return services;
    }
}

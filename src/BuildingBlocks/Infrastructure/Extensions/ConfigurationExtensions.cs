using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Extensions
{
    public static class ConfigurationExtensions
    {
        /// <summary>
        /// Extension method to add configuration settings
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="services"></param>
        /// <param name="sectionName"></param>
        /// <returns></returns>
        public static T GetOptions<T> (this IServiceCollection services, string sectionName) where T : class, new()
        {
            using var serviceProvider = services.BuildServiceProvider();
            var configuration = serviceProvider.GetService<Microsoft.Extensions.Configuration.IConfiguration>();
            var options = new T();
            configuration?.GetSection(sectionName).Bind(options);
            return options;
        }
    }
}

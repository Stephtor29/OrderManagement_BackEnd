using Microsoft.Extensions.DependencyInjection;
using OrderManagement.BusinessLogic.Services;
using OrderManagement.DataAccess;
using OrderManagement.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.BusinessLogic
{
    public static class ServiceConfiguration
    {
        public static void DataAccess(this IServiceCollection services, string connectionString)
        {

             services.AddScoped<ProductoRepository>();
            services.AddScoped<ClienteRepository>();
            services.AddScoped<OrdenRepository>();
            
            services.AddScoped<DetalleOrdenRepository>();




            OrderManagementContext.BuildConnectionString(connectionString);
        }


        public static void BusinessLogic(this IServiceCollection services)
        {
           services.AddScoped<OrderManagementServices>();
        }
    }
}

using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OGE3K6_Abstract_and_document_management.Data;
using OGE3K6_Abstract_and_document_management.Models;

[assembly: HostingStartup(typeof(OGE3K6_Abstract_and_document_management.Areas.Identity.IdentityHostingStartup))]
namespace OGE3K6_Abstract_and_document_management.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}
using System;
using System.Reflection;

using Microsoft.AspNetCore.Mvc;

namespace MiningMonitor.Web.Controllers
{
    [Route("api/[controller]")]
    public class VersionController : Controller
    {
        [HttpGet]
        public Version Get()
        {
            return Assembly.GetEntryAssembly().GetName().Version;
        }
    }
}
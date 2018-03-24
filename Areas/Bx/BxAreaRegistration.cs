using System.Web.Mvc;

namespace Bx_Web.Areas.Bx
{
    public class BxAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Bx";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Bx_default",
                "Bx/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                 new string[] { "Bx_Web.Areas.Bx.Controllers" }
            );
        }
    }
}
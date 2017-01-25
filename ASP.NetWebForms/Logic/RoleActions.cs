using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ASP.NetWebForms.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ASP.NetWebForms.Logic
{
    internal class RoleActions
    {
        internal void AddUserAndRole()
        {
            //throw new NotImplementedException();
            ApplicationDbContext context = new ApplicationDbContext();
            IdentityResult IdRoleResult;
            IdentityResult IdUserResult;

            var roleStore = new RoleStore<IdentityRole>(context);

            var roleManager = new RoleManager<IdentityRole>(roleStore);

            if (!roleManager.RoleExists("canEdit"))
            {
                IdRoleResult = roleManager.Create(new IdentityRole("canEdit"));
            }

            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var appUser = new ApplicationUser
            {
                UserName = "user",
                Email = "user@aspnetwebforms.com"
            };
            IdUserResult = userManager.Create(appUser, "Pa$$word123");

            if(!userManager.IsInRole(userManager.FindByEmail("user@aspnetwebforms.com").Id, "canEdit"))
            {
                IdUserResult = userManager.AddToRole(userManager.FindByEmail("user@aspnetwebforms.com").Id, "canEdit");
            }
        }
    }
}
namespace TeamBuilder.Services.Data.Implementations
{
    using System.Data.Entity;

    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    public class TeamBuilderRoleStore : RoleStore<IdentityRole>, IRoleStore<IdentityRole>
    {
        public TeamBuilderRoleStore(DbContext context) : base(context)
        {
        }
    }
}

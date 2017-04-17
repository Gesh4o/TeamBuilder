namespace TeamBuilder.Data.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<TeamBuilderContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "TeamBuilder.Data.TeamBuilderContext";
        }

        protected override void Seed(TeamBuilderContext context)
        {
        }
    }
}

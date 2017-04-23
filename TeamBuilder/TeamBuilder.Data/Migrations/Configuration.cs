namespace TeamBuilder.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public sealed class Configuration : DbMigrationsConfiguration<TeamBuilderContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "TeamBuilder.Data.TeamBuilderContext";
        }

        protected override void Seed(TeamBuilderContext context)
        {
        }
    }
}

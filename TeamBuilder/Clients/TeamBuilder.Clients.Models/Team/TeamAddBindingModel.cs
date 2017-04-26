namespace TeamBuilder.Clients.Models.Team
{
    using System.ComponentModel;
    using System.Web;

    using FluentValidation.Attributes;

    using TeamBuilder.Clients.Common.Mappings;
    using TeamBuilder.Clients.Models.Team.Validation;
    using TeamBuilder.Data.Models;

    [Validator(typeof(TeamAdditionValidator))]
    public class TeamAddBindingModel : IMapFrom<Team>
    {
        public string Name { get; set; }

        public string Acronym { get; set; }

        public string Description { get; set; }

        [DisplayName("Logo")]
        public HttpPostedFileBase Image { get; set; }
    }
}

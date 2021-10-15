using BlazingTrails.Client.Features.Home.Shared;
using BlazingTrails.Shared.Features.Home.Shared;
using MediatR;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace BlazingTrails.Client.Features.Home
{
    public partial class HomePage : ComponentBase
    {
        private IEnumerable<Trail> _trails;
        private Trail _selectedTrail;

        [Inject] public IMediator Mediator { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                var response = await Mediator.Send(new GetTrailsRequest());
                _trails = response.Trails.Select(_ => new Trail
                {
                    Id = _.Id,
                    Name = _.Name,
                    Image = _.Image,
                    Description = _.Description,
                    Location = _.Location,
                    Length = _.Length,
                    TimeInMinutes = _.TimeInMinutes
                });
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"There was a problem loading trail data: {ex.Message}");
            }
        }

        private void HandleTrailSelected(Trail trail)
            => _selectedTrail = trail;

    }
}

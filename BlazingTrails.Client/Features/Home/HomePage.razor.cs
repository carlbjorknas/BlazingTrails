using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace BlazingTrails.Client.Features.Home
{
    public partial class HomePage : ComponentBase
    {
        private IEnumerable<Trail> _trails;
        private Trail _selectedTrail;

        [Inject] public HttpClient Http { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                _trails = await Http.GetFromJsonAsync<IEnumerable<Trail>>("trails/trail-data.json");
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"There was a problem loading trail data: {ex.Message}");
            }
        }

        private void HandleTrailSelected(Trail trail)
        {
            _selectedTrail = trail;
            StateHasChanged();
        }
    }
}

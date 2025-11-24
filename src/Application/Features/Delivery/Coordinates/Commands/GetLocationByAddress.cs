//using System.Net.Http.Json;
//using CleanArchitecture.Blazor.Application.Features.Delivery.Coordinates.Models;

//namespace CleanArchitecture.Blazor.Application.Features.Delivery.Coordinates.Commands;

//public class GetLocationByAddress : IRequest<Coordinate?>
//{
//    public string Address { get; set; }

//    public GetLocationByAddress(string address)
//    {
//        Address = address;
//    }
//}

//public class GetLocationByAddressHandler : IRequestHandler<GetLocationByAddress, Coordinate?>
//{
//    private readonly IHttpClientFactory _httpClientFactory;

//    public GetLocationByAddressHandler(IHttpClientFactory httpClientFactory)
//    {
//        _httpClientFactory = httpClientFactory;
//    }

//    public async Task<Coordinate?> Handle(GetLocationByAddress request, CancellationToken cancellationToken)
//    {
//        if (string.IsNullOrWhiteSpace(request.Address))
//            return null;

//        try
//        {
//            var httpClient = _httpClientFactory.CreateClient();
//            var encodedAddress = Uri.EscapeDataString(request.Address);
//            var url = $"https://nominatim.openstreetmap.org/search?format=json&q={encodedAddress}&limit=1";

//            // Add user agent as required by Nominatim usage policy
//            httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("YourApp/1.0");

//            var response = await httpClient.GetFromJsonAsync<List<NominatimResponse>>(url, cancellationToken);

//            if (response?.Count > 0)
//            {
//                return new Coordinate
//                {
//                    Latitude = double.Parse(response[0].lat),
//                    Longitude = double.Parse(response[0].lon)
//                };
//            }
//        }
//        catch (Exception ex)
//        {
//            // Log error here
//            Console.WriteLine($"Error geocoding address: {ex.Message}");
//        }

//        return null;
//    }

//    private class NominatimResponse
//    {
//        public string lat { get; set; }
//        public string lon { get; set; }
//        public string display_name { get; set; }
//    }
//}

using Newtonsoft.Json;

namespace AirportDistanceCalculator.Data.BackgroundServices
{
    public class Airport
    {
        [JsonProperty("iata")]
        public string IATA { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("city_iata")]
        public object CityIATA { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("country_iata")]
        public string CountryIATA { get; set; }

        [JsonProperty("location")]
        public Location Location { get; set; }

        [JsonProperty("rating")]
        public int Rating { get; set; }

        [JsonProperty("hubs")]
        public int Hubs { get; set; }

        [JsonProperty("timezone_region_name")]
        public string TimezoneRegionName { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }

    public class Location
    {
        [JsonProperty("lon")]
        public float Longitude { get; set; }

        [JsonProperty("lat")]
        public float Latitude { get; set; }
    }


}

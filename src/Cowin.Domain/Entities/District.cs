using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace Cowin.Domain.Entities
{
    public class DistrictsInState
    {
        [JsonPropertyName("districts")]
        public List<District> Districts { get; set; }

        [JsonPropertyName("ttl")]
        public int TTL { get; set; }

        public District GetDistrict(string district)
        {
            if (Districts == null || Districts.Count < 1)
                return null;

            var match = Districts.SingleOrDefault(d => d.Matches(district));
            if (default == match) return District.EmptyObject();
            return match;
        }

        public bool IsEmpty()
        {
            return ((Districts == null || Districts.Count < 1) && TTL == int.MinValue);
        }

        public static DistrictsInState EmptyObject()
        {
            return new DistrictsInState {
                Districts = new List<District>(),
                TTL = int.MinValue,
            };
        }
    }

    public class District
    {
        [JsonPropertyName("district_id")]
        public int Id { get; set; }


        [JsonPropertyName("district_name")]
        public string Name { get; set; }


        public bool Matches(string district)
        {
            return string.Equals(Name.ToLower(), district.Trim().ToLower());
        }


        public bool IsEmpty()
        {
            return Id == int.MinValue && string.IsNullOrEmpty(Name); 
        }

        public static District EmptyObject()
        {
            return new District { 
                Id = int.MinValue,
                Name = string.Empty,
            };
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace Cowin.Domain.Entities
{
    public class AllStates
    {
        [JsonPropertyName("states")]
        public List<State> States { get; set; }


        [JsonPropertyName("ttl")]
        public int TTL { get; set; }

        public State GetState(string state)
        {
            if (null == States || States.Count < 1)
                return null;

            var match = States.SingleOrDefault(s => s.Matches(state));
            if (default == match) return State.EmptyObject();
            return match;
        }

        public static AllStates EmptyObject()
        {
            return new AllStates {
                States = new List<State>(),
                TTL = int.MinValue,
            };
        }

        public bool IsEmpty()
        {
            return ((States == null || States.Count < 1) && TTL == int.MinValue);
        }
    }


    public class State
    {
        [JsonPropertyName("state_id")]
        public int Id { get; set; }


        [JsonPropertyName("state_name")]
        public string Name { get; set; }


        public bool IsEmpty()
        {
            return Id == int.MinValue && string.IsNullOrEmpty(Name);
        }

        public bool Matches(string state)
        {
            return (string.Equals(Name.ToLower(), state.Trim().ToLower()));
        }

        public static State EmptyObject()
        {
            return new State {
                Id = int.MinValue,
                Name = string.Empty,
            };
        }
    }
}

﻿using Newtonsoft.Json;
using Sakuno.ING.Game.Models.MasterData;

namespace Sakuno.ING.Game.Json.MasterData
{
    internal class BgmInfoJson : IRawBgmInfo
    {
        [JsonProperty("api_id")]
        public int Id { get; set; }
        [JsonProperty("api_name")]
        public string Name { get; set; }
    }
}

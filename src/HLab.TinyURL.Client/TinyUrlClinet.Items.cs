namespace HLab.TinyURL
{

    public partial class Items : CreateTinyURLRequest
    {
        /// <summary>
        /// Operation
        /// </summary>
        [Newtonsoft.Json.JsonProperty("operation", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public BulkReportDataOperation Operation { get; set; }

        [Newtonsoft.Json.JsonProperty("metadata", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        public System.Collections.Generic.IList<string> Metadata { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    }
}
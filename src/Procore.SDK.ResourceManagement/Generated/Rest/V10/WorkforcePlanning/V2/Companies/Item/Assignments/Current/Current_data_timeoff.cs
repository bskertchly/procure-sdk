// <auto-generated/>
#pragma warning disable CS0618
using Microsoft.Kiota.Abstractions.Extensions;
using Microsoft.Kiota.Abstractions.Serialization;
using System.Collections.Generic;
using System.IO;
using System;
namespace Procore.SDK.ResourceManagement.Rest.V10.WorkforcePlanning.V2.Companies.Item.Assignments.Current
{
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
    #pragma warning disable CS1591
    public partial class Current_data_timeoff : IAdditionalDataHolder, IParsable
    #pragma warning restore CS1591
    {
        /// <summary>Stores additional data not described in the OpenAPI description found when deserializing. Can be used for serialization as well.</summary>
        public IDictionary<string, object> AdditionalData { get; set; }
        /// <summary>End date of the time-off period (string format)</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? EndDay { get; set; }
#nullable restore
#else
        public string EndDay { get; set; }
#endif
        /// <summary>End time of the time-off period</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? EndTime { get; set; }
#nullable restore
#else
        public string EndTime { get; set; }
#endif
        /// <summary>Indicates whether the time off is paid</summary>
        public bool? IsPaid { get; set; }
        /// <summary>Reason for the time off</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? Reason { get; set; }
#nullable restore
#else
        public string Reason { get; set; }
#endif
        /// <summary>Recurrence pattern of the time off</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? Repeat { get; set; }
#nullable restore
#else
        public string Repeat { get; set; }
#endif
        /// <summary>Start date of the time-off period (string format)</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? StartDay { get; set; }
#nullable restore
#else
        public string StartDay { get; set; }
#endif
        /// <summary>Start time of the time-off period</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? StartTime { get; set; }
#nullable restore
#else
        public string StartTime { get; set; }
#endif
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.ResourceManagement.Rest.V10.WorkforcePlanning.V2.Companies.Item.Assignments.Current.Current_data_timeoff"/> and sets the default values.
        /// </summary>
        public Current_data_timeoff()
        {
            AdditionalData = new Dictionary<string, object>();
        }
        /// <summary>
        /// Creates a new instance of the appropriate class based on discriminator value
        /// </summary>
        /// <returns>A <see cref="global::Procore.SDK.ResourceManagement.Rest.V10.WorkforcePlanning.V2.Companies.Item.Assignments.Current.Current_data_timeoff"/></returns>
        /// <param name="parseNode">The parse node to use to read the discriminator value and create the object</param>
        public static global::Procore.SDK.ResourceManagement.Rest.V10.WorkforcePlanning.V2.Companies.Item.Assignments.Current.Current_data_timeoff CreateFromDiscriminatorValue(IParseNode parseNode)
        {
            _ = parseNode ?? throw new ArgumentNullException(nameof(parseNode));
            return new global::Procore.SDK.ResourceManagement.Rest.V10.WorkforcePlanning.V2.Companies.Item.Assignments.Current.Current_data_timeoff();
        }
        /// <summary>
        /// The deserialization information for the current model
        /// </summary>
        /// <returns>A IDictionary&lt;string, Action&lt;IParseNode&gt;&gt;</returns>
        public virtual IDictionary<string, Action<IParseNode>> GetFieldDeserializers()
        {
            return new Dictionary<string, Action<IParseNode>>
            {
                { "end_day", n => { EndDay = n.GetStringValue(); } },
                { "end_time", n => { EndTime = n.GetStringValue(); } },
                { "is_paid", n => { IsPaid = n.GetBoolValue(); } },
                { "reason", n => { Reason = n.GetStringValue(); } },
                { "repeat", n => { Repeat = n.GetStringValue(); } },
                { "start_day", n => { StartDay = n.GetStringValue(); } },
                { "start_time", n => { StartTime = n.GetStringValue(); } },
            };
        }
        /// <summary>
        /// Serializes information the current object
        /// </summary>
        /// <param name="writer">Serialization writer to use to serialize this model</param>
        public virtual void Serialize(ISerializationWriter writer)
        {
            _ = writer ?? throw new ArgumentNullException(nameof(writer));
            writer.WriteStringValue("end_day", EndDay);
            writer.WriteStringValue("end_time", EndTime);
            writer.WriteBoolValue("is_paid", IsPaid);
            writer.WriteStringValue("reason", Reason);
            writer.WriteStringValue("repeat", Repeat);
            writer.WriteStringValue("start_day", StartDay);
            writer.WriteStringValue("start_time", StartTime);
            writer.WriteAdditionalData(AdditionalData);
        }
    }
}
#pragma warning restore CS0618

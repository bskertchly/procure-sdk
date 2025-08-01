// <auto-generated/>
#pragma warning disable CS0618
using Microsoft.Kiota.Abstractions.Extensions;
using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Abstractions;
using System.Collections.Generic;
using System.IO;
using System;
namespace Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Safety_violation_logs.Item
{
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
    #pragma warning disable CS1591
    public partial class Safety_violation_logsPatchRequestBody_safety_violation_log : IAdditionalDataHolder, IParsable
    #pragma warning restore CS1591
    {
        /// <summary>Stores additional data not described in the OpenAPI description found when deserializing. Can be used for serialization as well.</summary>
        public IDictionary<string, object> AdditionalData { get; set; }
        /// <summary>Comments</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? Comments { get; set; }
#nullable restore
#else
        public string Comments { get; set; }
#endif
        /// <summary>The date the compliance for the safety violation is due by</summary>
        public Date? ComplianceDue { get; set; }
        /// <summary>Format: YYYY-MM-DD Example: 2016-04-19</summary>
        public Date? Date { get; set; }
        /// <summary>Datetime of record. Mutually exclusive with the date property.</summary>
        public DateTimeOffset? Datetime { get; set; }
        /// <summary>Person who the safety violation was issued to</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? IssuedTo { get; set; }
#nullable restore
#else
        public string IssuedTo { get; set; }
#endif
        /// <summary>Name/number of the safety notice issued</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? SafetyNotice { get; set; }
#nullable restore
#else
        public string SafetyNotice { get; set; }
#endif
        /// <summary>Reason for the safety violation</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? Subject { get; set; }
#nullable restore
#else
        public string Subject { get; set; }
#endif
        /// <summary>Time of safety violation - hour</summary>
        public int? TimeHour { get; set; }
        /// <summary>Time of safety violation - minute</summary>
        public int? TimeMinute { get; set; }
        /// <summary>
        /// Instantiates a new <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Safety_violation_logs.Item.Safety_violation_logsPatchRequestBody_safety_violation_log"/> and sets the default values.
        /// </summary>
        public Safety_violation_logsPatchRequestBody_safety_violation_log()
        {
            AdditionalData = new Dictionary<string, object>();
        }
        /// <summary>
        /// Creates a new instance of the appropriate class based on discriminator value
        /// </summary>
        /// <returns>A <see cref="global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Safety_violation_logs.Item.Safety_violation_logsPatchRequestBody_safety_violation_log"/></returns>
        /// <param name="parseNode">The parse node to use to read the discriminator value and create the object</param>
        public static global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Safety_violation_logs.Item.Safety_violation_logsPatchRequestBody_safety_violation_log CreateFromDiscriminatorValue(IParseNode parseNode)
        {
            _ = parseNode ?? throw new ArgumentNullException(nameof(parseNode));
            return new global::Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Safety_violation_logs.Item.Safety_violation_logsPatchRequestBody_safety_violation_log();
        }
        /// <summary>
        /// The deserialization information for the current model
        /// </summary>
        /// <returns>A IDictionary&lt;string, Action&lt;IParseNode&gt;&gt;</returns>
        public virtual IDictionary<string, Action<IParseNode>> GetFieldDeserializers()
        {
            return new Dictionary<string, Action<IParseNode>>
            {
                { "comments", n => { Comments = n.GetStringValue(); } },
                { "compliance_due", n => { ComplianceDue = n.GetDateValue(); } },
                { "date", n => { Date = n.GetDateValue(); } },
                { "datetime", n => { Datetime = n.GetDateTimeOffsetValue(); } },
                { "issued_to", n => { IssuedTo = n.GetStringValue(); } },
                { "safety_notice", n => { SafetyNotice = n.GetStringValue(); } },
                { "subject", n => { Subject = n.GetStringValue(); } },
                { "time_hour", n => { TimeHour = n.GetIntValue(); } },
                { "time_minute", n => { TimeMinute = n.GetIntValue(); } },
            };
        }
        /// <summary>
        /// Serializes information the current object
        /// </summary>
        /// <param name="writer">Serialization writer to use to serialize this model</param>
        public virtual void Serialize(ISerializationWriter writer)
        {
            _ = writer ?? throw new ArgumentNullException(nameof(writer));
            writer.WriteStringValue("comments", Comments);
            writer.WriteDateValue("compliance_due", ComplianceDue);
            writer.WriteDateValue("date", Date);
            writer.WriteDateTimeOffsetValue("datetime", Datetime);
            writer.WriteStringValue("issued_to", IssuedTo);
            writer.WriteStringValue("safety_notice", SafetyNotice);
            writer.WriteStringValue("subject", Subject);
            writer.WriteIntValue("time_hour", TimeHour);
            writer.WriteIntValue("time_minute", TimeMinute);
            writer.WriteAdditionalData(AdditionalData);
        }
    }
}
#pragma warning restore CS0618

// <auto-generated/>
using System.Runtime.Serialization;
using System;
namespace Procore.SDK.ProjectManagement.Rest.V10.Projects.Item.Budget_changes.Item
{
    /// <summary>Displays the state the ERP entity is in.</summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
    public enum Budget_changesGetResponse_data_erp_status
    {
        [EnumMember(Value = "not_in_erp")]
        #pragma warning disable CS1591
        Not_in_erp,
        #pragma warning restore CS1591
        [EnumMember(Value = "ready_to_export")]
        #pragma warning disable CS1591
        Ready_to_export,
        #pragma warning restore CS1591
        [EnumMember(Value = "exportging")]
        #pragma warning disable CS1591
        Exportging,
        #pragma warning restore CS1591
        [EnumMember(Value = "failed_to_export")]
        #pragma warning disable CS1591
        Failed_to_export,
        #pragma warning restore CS1591
        [EnumMember(Value = "rejected")]
        #pragma warning disable CS1591
        Rejected,
        #pragma warning restore CS1591
        [EnumMember(Value = "invalid")]
        #pragma warning disable CS1591
        Invalid,
        #pragma warning restore CS1591
        [EnumMember(Value = "synced")]
        #pragma warning disable CS1591
        Synced,
        #pragma warning restore CS1591
        [EnumMember(Value = "not_integrated")]
        #pragma warning disable CS1591
        Not_integrated,
        #pragma warning restore CS1591
    }
}

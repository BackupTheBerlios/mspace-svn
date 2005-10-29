using System;
using ComponentModel.Interfaces;

namespace ComponentBuilder.DTO {
    internal sealed class TagValuesDTO : IDataTransferObject {
        internal const string ComponentName = "${component_name}";
        internal const string NAntComponentName = "$@{component_name}";
        internal const string ViewName = "${view_name}";
        internal const string ExceptionManager = "${exception_manager_class_name}";
        internal const string ResponseName = "${response_name}";
        internal const string ReturnType = "${return_type}";
        internal const string MethodName = "${method_name}";
        internal const string Body = "${body}";
        internal const string Parameters = "${parameters}";
        internal const string Prefix = "${prefix}";
    }
}

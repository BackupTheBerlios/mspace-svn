using System;
using ComponentModel.Interfaces;

namespace ComponentBuilder.DTO {
    internal sealed class TemplateNamesDTO : IDataTransferObject {
        //Que ya son estáticos :D
        internal const string BusinessObject = "BusinessObject.template";
        internal const string ExceptionManager = "ExceptionManager.template";
        internal const string MethodBody = "MethodBody.template";
        internal const string ResponseMethod = "ResponseMethod.template";
        internal const string ViewHandler = "ViewHandler.template";
        internal const string NAntBuildfile = "NAnt.template";
        internal const string AssemblyInfo = "AssemblyInfo.template";
    }
}

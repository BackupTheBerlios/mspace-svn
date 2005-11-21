using System;

namespace ComponentBuilder.Interfaces {
    internal enum ActionState {
        Create,
        Delete,
        Edit,
        Selection,
        None
    }
    
    internal interface IViewHandlerActionState {
        ActionState ActionState {get; set;}
    }
}

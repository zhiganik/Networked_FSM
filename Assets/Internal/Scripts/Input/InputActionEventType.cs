using System;

namespace Shakhtarsk.Input
{
    [Flags]
    public enum InputActionEventType
    {
        Performed = 1,
        Started = 2,
        Canceled = 4,
        All = Performed | Started | Canceled,
        Default = Performed | Canceled
    }
}
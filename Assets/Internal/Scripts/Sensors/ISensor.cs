using System;

namespace Shakhtarsk.Sensors
{
    public interface ISensor
    {
        bool Enable { get; }
        Type Filter { get; set; }
        
        event Action<SensorInfo> Detected;
        event Action<SensorInfo> Lost;

        bool IsDetected { get; }
    }
}
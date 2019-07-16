﻿//创建者:Icarus
//手动滑稽,滑稽脸
//ヾ(•ω•`)o
//2018年07月30日-05:12
//Icarus.UnityGameFramework.Bolt

using Bolt;

namespace CabinIcarus.BoltExtensions.Event
{
    public interface IEventBaseUnit
    {
        EventTableScriptableObject EventTableAsset { get;}

        EventTable EventTable { get; }
        
        ValueInput EventId { get; }

        ValueInput EventName { get; }
    }
}
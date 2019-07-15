﻿//创建者:Icarus
//手动滑稽,滑稽脸
//ヾ(•ω•`)o
//2018年08月02日-02:18
//Icarus.UnityGameFramework.Bolt

using System.Collections.Generic;
using System.Linq;
using CabinIcarus.BoltExtensions.Event;
using Ludiq;
using Ludiq.Bolt;
using UnityEngine;

namespace CabinIcarus.BoltExtensions.Units
{
    [UnitCategory("Icarus/Util/Events")]
    [UnitTitle("TriggerCustomEvent")]
    [TypeIcon(typeof(CustomEvent))]
    [UnitOrder(1)]
    public class NewTriggerCustomEventUnit:IcUnit,IEventBaseUnit
    {
        [DoNotSerialize]
        [Inspectable, UnitHeaderInspectable("TableAsset")]
        public EventTableScriptableObject EventTableAsset { get; private set; }
        [Serialize]
        [Inspectable, UnitHeaderInspectable("Events")]
        public EventTable EventTable { get; private set; }

        [DoNotSerialize]
        public ValueInput EventId { get; private set; }
        [DoNotSerialize]
        public ValueInput EventName { get; private set; }

        [Serialize]
        [Inspectable, UnitHeaderInspectable("Arg Count")]
        public int EventArgCount { get; private set; }

        /// <summary>
        /// The target of the event.
        /// </summary>
        [DoNotSerialize]
        [UnitPortLabelHidden]
        [NullMeansSelf]
        public ValueInput target { get; private set; }

        [DoNotSerialize]
        public List<ValueInput> arguments { get; private set; }

        protected override void Definition()
        {
            base.Definition();

            EventName = ValueInput(nameof(EventName), string.Empty);

            target = ValueInput<GameObject>(nameof(target), null).NullMeansSelf();

            arguments = new List<ValueInput>();
            _setEventArgCountAndArgList();
            if (EventTable != null && EventTable.SelectEvent != null &&
                EventTable.SelectEvent.Args != null &&
                EventTable.SelectEvent.Args.Count == EventArgCount)
            {
                for (var i = 0; i < EventTable.SelectEvent.Args.Count; i++)
                {
                    var argName = EventTable.SelectEvent.Args[i].ArgName;
                    if (string.IsNullOrWhiteSpace(argName))
                    {
                        argName = $"argument_{i}";
                    }
                    var argument = ValueInput(EventTable.SelectEvent.Args[i].ArgType,argName);
                    arguments.Add(argument);
                    if(EventTable.SelectEvent.Args[i].NotNull)
                    {
                        Requirement(argument, _enter);
                    }
                    else
                    {
                        argument.Default(EventTable.SelectEvent.Args[i].ArgType.Default());
                    }
                }
            }
            else
            {
                for (var i = 0; i < EventArgCount; i++)
                {
                    var argument = ValueInput<object>("argument_" + i);
                    arguments.Add(argument);
                    Requirement(argument, _enter);
                }
            }
            
            Requirement(EventName, _enter);
            Requirement(target, _enter);
            Succession(_enter, _exit);
        }

        private void _setEventArgCountAndArgList()
        {
            //没有事件表资源初始化
            if (EventTable == null || EventTable.Events == null)
            {
                return;
            }

            EventArgCount = EventTable.GetArgCount();

        }

        protected override ControlOutput Enter(Flow flow)
        {
            var target = flow.GetValue<GameObject>(this.target);
            var name = flow.GetValue<string>(EventName);
            var arguments = this.arguments.Select(flow.GetConvertedValue).ToArray();

            NewCustomEventUnit.Trigger(target, name, arguments);

            return _exit;
        }

        
    }
}
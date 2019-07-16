﻿//创建者:Icarus
//手动滑稽,滑稽脸
//ヾ(•ω•`)o
//2018年07月29日-07:52
//Icarus.UnityGameFramework.Bolt

using System;
using System.Collections.Generic;
using System.Linq;
using Ludiq;
using UnityEngine;

namespace CabinIcarus.BoltExtensions.Event
{
    [System.Serializable]
    public partial class ArgEntity
    {
        [SerializeField]
        private string _argName;
        [SerializeField]
        private string _argTypeStr;
        [TextArea(2,5,order = 20)]
        [SerializeField]
        private string _argDesc;
        [SerializeField]
        private bool _notNull;
        [SerializeField]
        private bool _isDefault;
        [SerializeField]
        private object _default;

        public ArgEntity(ArgEntity arg)
        {
            _argName = arg.ArgName;
            _argTypeStr = arg.ArgTypeStr;
            _argDesc = arg.ArgDesc;
            _notNull = arg.NotNull;
            IsDefault = arg.IsDefault;
            Default = arg.Default;
        }

        public ArgEntity():this(string.Empty,typeof(object).AssemblyQualifiedName,string.Empty,true)
        {
        }

        public ArgEntity(string argName, string argAssemblyQualifiedName, string argDesc, bool notNull)
        {
            _argName = argName;
            _argTypeStr = argAssemblyQualifiedName;
            _argDesc = argDesc;
            _notNull = notNull;
        }

        public ArgEntity(string argName, string argAssemblyQualifiedName):this(argName,argAssemblyQualifiedName,string.Empty,true)
        {
        }

        /// <summary>
        /// 参数名
        /// </summary>
        public string ArgName => _argName;

        /// <summary>
        /// 参数类型
        /// </summary>
        public Type ArgType => Type.GetType(_argTypeStr);

        /// <summary>
        /// 参数描述
        /// </summary>
        public string ArgDesc => _argDesc;

        /// <summary>
        /// 参数不能为空
        /// </summary>
        public bool NotNull => _notNull;

        public string ArgTypeStr => _argTypeStr;

        /// <summary>
        /// 是否存在默认值
        /// </summary>
        public bool IsDefault
        {
            get { return _isDefault; }
            set { _isDefault = value; }
        }

        /// <summary>
        /// 默认值
        /// </summary>
        public object Default
        {
            get { return _default; }
            set { _default = value; }
        }

        public override string ToString()
        {
            return ArgName;
        }

        public static bool operator ==(ArgEntity a, ArgEntity b)
        {
            if (ReferenceEquals(a,null) && ReferenceEquals(b,null))
            {
                return true;
            }

            if (ReferenceEquals(a,null))
            {
                return false;
            }

            if (ReferenceEquals(b,null))
            {
                return false;
            }
            
            return  a.Equals(b);
        }

        public static bool operator !=(ArgEntity a, ArgEntity b)
        {
            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is ArgEntity other))
            {
                return false;
            }

            return Equals(other);
        }

        public bool Equals(ArgEntity other)
        {
            return _argName == other._argName && 
                   _argTypeStr == other._argTypeStr && 
                   _notNull == other._notNull && 
                   _isDefault == other._isDefault && 
                   _default == other._default;
        }

        public override int GetHashCode()
        {
            var baseCode = 98;

            var code = baseCode + _argName.GetHashCode();

            code += _argTypeStr?.GetHashCode() ?? 0;

            code += _notNull.GetHashCode();

            code += _isDefault.GetHashCode();

            code += _default?.GetHashCode() ?? 0;

            return code;
        }
    }

    [System.Serializable]
    public class EventEntity
    {
        [SerializeField]
        private string _eventName;
        [SerializeField]
        private int _eventId;
        [SerializeField]
        private List<ArgEntity> _args;

        public EventEntity(string eventName, int eventID)
        {
            this._eventName = eventName;
            this._eventId = eventID;
        }

        public string EventName
        {
            get
            {
                return _eventName;
            }

            set
            {
                _eventName = value;
            }
        }

        public int EventID
        {
            get
            {
                return _eventId;
            }

            set
            {
                _eventId = value;
            }
        }

        public List<ArgEntity> Args
        {
            get
            {
                return _args;
            }

            set
            {
                _args = value;
            }
        }

        public override string ToString()
        {
            return $"EventName:{EventName},EventID:{EventID}";
        }
    }

    public class EventTable
    {
        /// <summary>
        /// 表资源名
        /// </summary>
        public string TableAssetName;

        [DoNotSerialize]
        public List<EventEntity> Events;

        public EventEntity SelectEvent { get; set; }

        public string SelectEventOfAssetName { get; set; }

        public override string ToString()
        {
            return $"Select Event Name:{SelectEvent.EventName},ID:{SelectEvent.EventID}";
        }

        public EventEntity GetEvent(int eventID)
        {
            if (Events == null)
            {
                return SelectEvent;
            }

            var @event = Events.FirstOrDefault(x => x.EventID == eventID);

            return @event;
        }

        public int GetArgCount()
        {
            if (SelectEvent == null)
            {
                return 0;
            }

            var @event = GetEvent(SelectEvent.EventID);

            if (@event != null)
            {
                if (SelectEvent != null && @event.Args.Count != SelectEvent.Args.Count &&
                    SelectEventOfAssetName != TableAssetName)
                {
                    return SelectEvent.Args.Count;
                }
                else
                {
                    return @event.Args.Count;
                }
            }

            if (SelectEvent != null)
            {
                return SelectEvent.Args.Count;
            }

            return 0;
        }

        public List<ArgEntity> GetArgList()
        {
            var @event = GetEvent(SelectEvent.EventID);

            return @event?.Args;
        }

    }
}
﻿using System;
using System.Diagnostics;
using log4net.Core;
using weweave.DnnDevTools.Util;

namespace weweave.DnnDevTools.Dto
{
    public class LogMessage : IAction
    {
        public string Type => Globals.ActionTypeLogMessage;

        /// <summary>
        /// Unique Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Level of the log message (TRACE, DEBUG, INFO, WARN, ERROR or FATAL)
        /// </summary>
        public string Level { get; set; }

        /// <summary>
        /// Numeric log level
        /// </summary>
        public int LevelValue { get; set; }

        /// <summary>
        /// Name of the logger
        /// </summary>
        public string Logger { get; set; }

        /// <summary>
        /// Log message (contain HTML tags like <b/> or <br/>
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Time stamp when the log message was created
        /// </summary>
        public DateTime TimeStamp { get; set; }

        /// <summary>
        /// Full qualified name of class that logs the message (Example: DotNetNuke.Services.Scheduling.ScheduleHistoryItem)
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// Name of the method that logs the message (Example: DotNetNuke.Services.Scheduling.ScheduleHistoryItem)
        /// </summary>
        public string MethodName { get; set; }

        public LogMessage() { }

        internal LogMessage(LoggingEvent loggingEvent)
        {
            Id = HashUtil.GetMd5Hash(string.Concat(
                loggingEvent.TimeStamp.Ticks.ToString(),
                loggingEvent.LoggerName ?? string.Empty,
                loggingEvent.RenderedMessage ?? string.Empty
            ));
            Level = loggingEvent.Level?.DisplayName;
            Debug.Assert(loggingEvent.Level != null, "loggingEvent.Level != null");
            LevelValue = loggingEvent.Level.Value;
            Logger = loggingEvent.LoggerName;
            Message = loggingEvent.RenderedMessage;
            TimeStamp = loggingEvent.TimeStamp;
            MethodName = loggingEvent.LocationInformation?.MethodName;
            ClassName = loggingEvent.LocationInformation?.ClassName;
        }

        internal LogMessage Copy()
        {
            return new LogMessage
            {
                Level = Level,
                LevelValue = LevelValue,
                Id = Id,
                Logger = Logger,
                ClassName = ClassName,
                Message = Message,
                MethodName = MethodName,
                TimeStamp = TimeStamp
            };
        }
    }
}

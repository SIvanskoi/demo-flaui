using System;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Definitions;
using FlaUI.Core.Exceptions;
using FlaUI.Core.Tools;
using NLog;

namespace FLaUIDemo.Extensions
{
    public static class WaitExtension
    {
        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();
        private static readonly TimeSpan DefaultTimeout = TimeSpan.FromMilliseconds(5000);

        /*
        /// <summary>
        /// Waits until the specified element is enabled or the timeout in milliseconds is reached.
        /// </summary>
        public static bool WaitUntilEnabled(this AutomationElement element, TimeSpan? waitTimeout= null)
        {
            logger.Info($"Wait until element is enabled: {element}");
            var waitTime = waitTimeout ?? DefaultTimeout;
            return Retry.WhileFalse(
                () => element?.IsEnabled ?? false,
                waitTime).Success;
            
        }
        */

        /// <summary>
        /// Waits until the element exists.
        /// </summary>
        public static AutomationElement WaitUntilExists(this AutomationElement parent, Func<AutomationElement> findFunc, TimeSpan? waitTimeout = null, bool throwOnTimeout = true)
        {
            var waitTime = waitTimeout ?? DefaultTimeout;
            var result = Retry.WhileNull(findFunc, waitTime);
            if (!result.Success && throwOnTimeout)
                throw new ElementNotAvailableException($"Element is not found after {result.Duration.TotalSeconds} s");
                //logger.Error($"Element is not found after {result.Duration.TotalSeconds} s");
            return result.Result;
        }

        public static AutomationElement WaitUntilExists(this AutomationElement parent, string controlName, ControlType controlType, TimeSpan? waitTimeout = null, bool throwOnTimeout = true)
        {
            var waitTime = waitTimeout ?? DefaultTimeout;
            logger.Info($"Wait until element exists [ {controlType.ToString()} : {controlName} ]");
            var result = Retry.WhileNull(() => parent.FindFirstDescendant(cf => cf.ByControlType(controlType).And(cf.ByName(controlName.ToL10N()))), waitTime);
            if (!result.Success && throwOnTimeout)
                throw new ElementNotAvailableException($"Element [ {controlType.ToString()} : {controlName} ] is not found after {result.Duration.TotalSeconds} s");
            return result.Result;
        }


    }
}

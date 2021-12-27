// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Runtime.ExceptionServices;
#if WINDOWS10_0_17763_0
using Windows.ApplicationModel.Core;
using Windows.Foundation.Metadata;
#endif

namespace Microsoft.AppCenter.Utils
{
    public class ApplicationLifecycleHelperWinUI: IApplicationLifecycleHelper
    {
        public event EventHandler ApplicationSuspended;
        public event EventHandler ApplicationResuming;
        public event EventHandler<UnhandledExceptionOccurredEventArgs> UnhandledExceptionOccurred;
        
        // Considered to be suspended until can verify that has started
        private static bool _suspended = true;

        // Singleton instance of ApplicationLifecycleHelper
        private static ApplicationLifecycleHelperWinUI _instance;
        public static ApplicationLifecycleHelperWinUI Instance
        {
            get { return _instance ?? (_instance = new ApplicationLifecycleHelperWinUI()); }

            // Setter for testing
            internal set { _instance = value; }
        }

        /// <summary>
        /// Indicates whether the application is currently in a suspended state. 
        /// </summary>
        public bool IsSuspended => _suspended;

        public ApplicationLifecycleHelperWinUI()
        {

#if WINDOWS10_0_17763_0
            // If the "LeavingBackground" event is present, use that for Resuming. Else, use CoreApplication.Resuming.
            if (ApiInformation.IsEventPresent(typeof(CoreApplication).FullName, "LeavingBackground"))
            {
                CoreApplication.LeavingBackground += InvokeResuming;

                // If the application has anything visible, then it has already started,
                // so invoke the resuming event immediately.
                InvokeResuming(null, EventArgs.Empty);
            }
            else
            {
                InvokeResuming(null, EventArgs.Empty);
            }

            // Subscribe to unhandled errors events.
            CoreApplication.UnhandledErrorDetected += (sender, eventArgs) =>
            {
                try
                {
                    // Intentionally propagate exception to get the exception object that crashed the app.
                    eventArgs.UnhandledError.Propagate();
                }
                catch (Exception exception)
                {
                    InvokeUnhandledExceptionOccurred(sender, exception);

                    // Since UnhandledError.Propagate marks the error as Handled, rethrow in order to only Log and not Handle.
                    // Use ExceptionDispatchInfo to avoid changing the stack-trace.
                    ExceptionDispatchInfo.Capture(exception).Throw();
                }
            };
#endif
        }

        internal void InvokeUnhandledExceptionOccurred(object sender, Exception exception)
        {
            UnhandledExceptionOccurred?.Invoke(sender, new UnhandledExceptionOccurredEventArgs(exception));
        }

        private void InvokeResuming(object sender, object e)
        {
            _suspended = false;
            ApplicationResuming?.Invoke(sender, EventArgs.Empty);
        }
    }
}
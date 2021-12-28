// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.AppCenter.Utils;
using System;

namespace Microsoft.AppCenter
{
    public class ApplicationLifecycleHelper : IApplicationLifecycleHelper
    {
        private static IApplicationLifecycleHelper _instance = null;
        public bool IsSuspended => _instance.IsSuspended;

        public event EventHandler ApplicationSuspended
        {
            add
            {
                _instance.ApplicationSuspended += value;
            }
            remove
            {
                _instance.ApplicationSuspended -= value;
            }
        }

        public event EventHandler ApplicationResuming
        {
            add
            {
                _instance.ApplicationResuming += value;
            }
            remove
            {
                _instance.ApplicationResuming -= value;
            }
        }

        public event EventHandler<UnhandledExceptionOccurredEventArgs> UnhandledExceptionOccurred
        {
            add
            {
                _instance.UnhandledExceptionOccurred += value;
            }
            remove
            {
                _instance.UnhandledExceptionOccurred -= value;
            }
        }

        public static IApplicationLifecycleHelper Instance
        {
            get
            {
                if (_instance == null)
                {
                    if (WpfHelper.IsRunningAsUwp)
                    {
                        _instance = ApplicationLifecycleHelperWinUI.Instance;
                        AppCenterLog.Debug(AppCenterLog.LogTag, "Use lifecycle for WinUI applications.");
                    }
                    else
                    {
                        _instance = ApplicationLifecycleHelperDesktop.Instance;
                        AppCenterLog.Debug(AppCenterLog.LogTag, "Use lifecycle for desktop applications.");
                    }
                }
                return _instance;
            }

            // Setter for testing
            internal set { _instance = value; }
        }

        private ApplicationLifecycleHelper()
        {
        }
    }
}


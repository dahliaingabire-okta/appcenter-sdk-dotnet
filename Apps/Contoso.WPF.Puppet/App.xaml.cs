// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections;
using System.IO;
using System.Web;
using System.Windows;
using Contoso.WPF.Puppet.Properties;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;

namespace Contoso.WPF.Puppet
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            AppCenter.LogLevel = LogLevel.Verbose;
            AppCenter.SetLogUrl("https://in-integration.dev.avalanch.es");
            Crashes.GetErrorAttachments = report =>
            {
                var attachments = new ArrayList();

                // Text attachment
                if (!string.IsNullOrEmpty(Settings.Default.TextErrorAttachments))
                {
                    attachments.Add(
                        ErrorAttachmentLog.AttachmentWithText(Settings.Default.TextErrorAttachments, "text.txt"));
                }

                // Binary attachment
                if (!string.IsNullOrEmpty(Settings.Default.FileErrorAttachments))
                {
                    if (File.Exists(Settings.Default.FileErrorAttachments))
                    {
                        var fileName = new FileInfo(Settings.Default.FileErrorAttachments).Name;
                        var mimeType = MimeMapping.GetMimeMapping(Settings.Default.FileErrorAttachments);
                        var fileContent = File.ReadAllBytes(Settings.Default.FileErrorAttachments);
                        attachments.Add(ErrorAttachmentLog.AttachmentWithBinary(fileContent, fileName, mimeType));
                    }
                    else
                    {
                        Settings.Default.FileErrorAttachments = null;
                    }
                }

                return (ErrorAttachmentLog[])attachments.ToArray();
            };
            Crashes.SendingErrorReport += (sender, args) => Console.WriteLine($@"[App] Sending report Id={args.Report.Id} Exception={args.Report.Exception}");
            Crashes.SentErrorReport += (sender, args) => Console.WriteLine($@"[App] Sent report Id={args.Report.Id}");
            Crashes.FailedToSendErrorReport += (sender, args) => Console.WriteLine($@"[App] FailedToSend report Id={args.Report.Id} Error={args.Exception}");
            AppCenter.Start("42f4a839-c54c-44da-8072-a2f2a61751b2", typeof(Analytics), typeof(Crashes));
        }
    }
}

using System;
using System.Drawing;
using System.IO;
using FlaUI.Core.Definitions;
using FlaUI.Core.Input;
using FlaUI.Core.Tools;
using FlaUI.Core.WindowsAPI;
using FLaUIDemo.COM;
using FLaUIDemo.Common;
using FLaUIDemo.Extensions;
using FLaUIDemo.UI;
using NLog;
using NUnit.Framework;


namespace FLaUIDemo.Tests
{
    [TestFixture]
    public class Test : TestBase
    {

        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();
        
        [TestCase(TestName = "Mapping: Floating topic can be dragged to anothet floating topic")]
        public void Test_001()
        {
            Robot.AttachOrLaunch();
            Robot.AddBlankMap();
            ComInterop.Application.ActiveDocument.CentralTopic.Text = "Test";
            var topic1 = Robot.AddFloatingTopic(200, 100);
            topic1.Text = "Destination object";

            var topic2 = Robot.AddFloatingTopic(100, 600);
            topic2.Text = "Object to drag";

            Rectangle r1 = ComInterop.GetObjectRect(topic1);
            Rectangle r2 = ComInterop.GetObjectRect(topic2);

            Robot.SetSelection();       

            //Robot.Drag(r1.Center(), r2.Center(), VirtualKeyShort.CONTROL);
            Robot.Drag(r2.Center(), r1.Center());
            
            Robot.MainWindow.RibbonBar.Click("Insert")
                                      .Click("Smart Shapes", ControlType.SplitButton);
            Robot.ClickMenuItem("Swim Lanes")
                 .ClickMenuItem("Horizontal Swim Lanes")
                 .ClickMenuItem("More...");
            //Robot.MainWindow.ActiveDocumentWindow.LClick(10, 10);
            
            string filePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Robot.SaveAs(filePath);
            Assert.That(File.Exists($"{filePath}.mmap"), Is.True, "File does not exist");
        }

        [TestCase(TestName = "Mapping: Open existing document")]
        [Ignore("")]
        public void Test_002()
        {
            string fileName = "MMD_3140";
            Robot.AttachOrLaunch();
            var doc = Robot.AddExistingMap($"Maps\\{fileName}.mmap");
            Assert.That(doc.Name, Is.EqualTo(fileName), "Document title");    
        }

        [TestCase]
        [Ignore("")]
        public void Uninstall()
        {
            Robot.Kill();
            if (Directory.Exists(RegistrySettingsManager.LocalUserDataDirectory))
                Directory.Delete(RegistrySettingsManager.LocalUserDataDirectory, recursive: true);
            MsiInstaller.Unistall();
            RegistrySettingsManager.DeleteSettingsEntry();
        }

         
    }
}

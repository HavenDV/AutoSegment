using System;
using WixSharp;
using WixSharp.CommonTasks;

namespace AutoSegmentInstaller
{
    internal static class Program
    {
        #region Properties

        private static string CompanyName { get; } = @"AutoSegmentCompany";
        private static string ProgramName { get; } = @"AutoSegment";
        private static string ProgramShortName { get; } = @"AutoSegment";
        private static string ProgramFilesPath { get; } = $@"%ProgramFiles%\{CompanyName}\{ProgramName}";
        private static string ApplicationPath { get; } = @"..\AutoSegment\bin\Release\";

        #endregion

        #region Main

        private static void Main()
        {
            try
            {
                CreateMsi();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        #endregion

        #region Methods

        private static void CreateMsi()
        {
            var mainExe = new File(ApplicationPath + ProgramShortName + ".exe")
            {
                Shortcuts = new[]
                {
                    new FileShortcut(ProgramName, "%ProgramMenu%"),
                    new FileShortcut(ProgramName, "%Desktop%")
                }
            };
            var project = new Project(
                ProgramShortName,
                new Dir(
                    ProgramFilesPath,
                    new DirFiles(ApplicationPath + "*.dll"),
                    new DirFiles(ApplicationPath + "*.config"),
                    new DirFiles(ApplicationPath + "*.dat"),
                    mainExe
                )
            )
            {
                GUID = new Guid("800B101E-B71D-4F7A-A66D-86DE71FA5917"),
                ControlPanelInfo =
                {
                    Manufacturer = CompanyName,
                    ProductIcon = @"..\AutoSegment\icon.ico",
                },
                Version = new Version(1, 0, 0),
                UI = WUI.WixUI_ProgressOnly,
                MajorUpgrade = MajorUpgrade.Default
            };

            project.SetNetFxPrerequisite(Condition.Net452_Installed, "Please install .NET 4.5.2 first.");

            Compiler.WixLocation = @"..\packages\WixSharp.wix.bin.3.11.0\tools\bin";

            Compiler.BuildMsi(project);
        }

        #endregion
    }
}
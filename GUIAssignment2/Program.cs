using SemesterAssignment2;

namespace GUIAssignment2
{
    internal static class Program
    {

        public static ApplicationLogicDH ApplicationLogic { get; set; } = new ApplicationLogicDH();
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new MainForm());
        }
    }
}
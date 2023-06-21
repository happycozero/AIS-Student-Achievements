using System.Globalization;
using System.Windows.Forms;

namespace Student_Achievements.Classes
{
    public static class KeyboardLayout
    {
        public static void SetToRussian()
        {
            InputLanguage.CurrentInputLanguage = InputLanguage.FromCulture(new CultureInfo("ru-RU"));
        }
        public static void SetToEnglish()
        {
            InputLanguage.CurrentInputLanguage = InputLanguage.FromCulture(new CultureInfo("en-EN"));
        }
    }
}

using System.Reflection;
using System.Xml;
using System.Xml.XPath;

namespace ProjectEtoPrototype.Models
{
    internal static class QuranHandler
    {
        private static XPathNavigator nav;
        private static XPathDocument docNav;

        private static XPathNavigator chapter;
        public static XPathNavigator Chapter { 
            get { return chapter; } 
        }
        public static int ChapterID
        {
            get { return Convert.ToInt32(chapter.GetAttribute("ChapterID", chapter.NamespaceURI)); }
        }
        public static string ChapterName
        {
            get { return Chapter.GetAttribute("ChapterName", Chapter.NamespaceURI); }
        }
        public static int ChapterVersesCount
        {
            get { return chapter.SelectChildren("Verse", verse.NamespaceURI).Count; }
        }

        private static XPathNavigator verse;
        public static XPathNavigator Verse {
            get { return verse; }
        }
        public static int VerseID
        {
            get { return Convert.ToInt32(verse.GetAttribute("VerseID", verse.NamespaceURI)); }
        }

        private const string PATH_TO_XML = "C:\\Users\\Family\\source\\repos\\Eto\\ProjectEtoPrototype\\Models\\FullQuran.xml";

        static QuranHandler()
        {

            //string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Resources);
            //string quranXMLPath = Path.Join(documentsPath, "Arabic-(Original-Book)-1.xml");

            docNav = new XPathDocument(PATH_TO_XML);
            nav = docNav.CreateNavigator();
        }

        public static string SetAndGetVerse(int chapterID, int verseID)
        {
            string chapterExpression = $"/HolyQuran/Chapter[@ChapterID={chapterID}]";
            string verseExpression = $"/HolyQuran/Chapter[@ChapterID={chapterID}]/Verse[@VerseID={verseID}]";
            chapter = nav.SelectSingleNode(chapterExpression);
            verse = nav.SelectSingleNode(verseExpression);
            return verse.Value;
        }
        public static string NextVerse()
        {
            if (VerseID+1 > ChapterVersesCount) {
                if (ChapterID == 114) {
                    return SetAndGetVerse(1, 1);
                }

                else {
                    return SetAndGetVerse(ChapterID + 1, 1);
                }
            }
            else {
                return SetAndGetVerse(ChapterID, VerseID+1);
            }
        }
        public static string PreviousVerse()
        {
            if (VerseID-1 <= 0)
            {
                if (ChapterID == 1)
                {
                    return SetAndGetVerse(114, 6);
                }
                else
                {
                    SetAndGetVerse(ChapterID-1, 1);
                    return SetAndGetVerse(ChapterID, ChapterVersesCount);
                }
            }
            else
            {
                return SetAndGetVerse(ChapterID, VerseID-1);
            }
        }   
    }

}

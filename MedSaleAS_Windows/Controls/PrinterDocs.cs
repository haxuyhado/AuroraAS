using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Word = Microsoft.Office.Interop.Word;
using Microsoft.Win32;

namespace WPFModernVerticalMenu
{
    class PrinterDocs
    {
        public static string PersonData(string WorkerFIO)
        {
            try
            {
                Word.Application wordApp = new Word.Application();
                string currentDirectory = Directory.GetCurrentDirectory();
                Word.Document doc = wordApp.Documents.Open(Path.Combine(currentDirectory, @"Шаблоны для печати\PersDann.docx"));
                wordApp.Visible = false;
                doc.Content.Find.Execute(FindText: "ФиоМенеджера", ReplaceWith: WorkerFIO, Replace: Word.WdReplace.wdReplaceAll);
                doc.PrintOut();
                doc.Content.Find.Execute(FindText: WorkerFIO, ReplaceWith: "ФиоМенеджера", Replace: Word.WdReplace.wdReplaceAll);
                doc.Close();
                wordApp.Quit();
                return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public static string NoPersonData()
        {
            try
            {
                Word.Application wordApp = new Word.Application();
                string currentDirectory = Directory.GetCurrentDirectory();
                Word.Document doc = wordApp.Documents.Open(Path.Combine(currentDirectory, @"Шаблоны для печати\NoPersDann.docx"));
                wordApp.Visible = false;
                doc.PrintOut();
                doc.Close();
                wordApp.Quit();
                return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public static string DKP(string Firm, string FIO, string Adress, string Phone, string INN, string OGRN)
        {
            try
            {
                string file = Path.Combine(Directory.GetCurrentDirectory(), @"Шаблоны для печати\DKP.docx");
                string newfile = Path.Combine(Directory.GetCurrentDirectory(), @"Шаблоны для печати\DKP(1).docx");

                File.Copy(file, newfile, true);
                Word.Application wordApp = new Word.Application();
                Word.Document doc = wordApp.Documents.Open(newfile);
                wordApp.Visible = false;
                doc.Content.Find.Execute(FindText: "ФирмаПолучатель", ReplaceWith: Firm, Replace: Word.WdReplace.wdReplaceAll);
                doc.Content.Find.Execute(FindText: "ФИОПолучателя", ReplaceWith: FIO, Replace: Word.WdReplace.wdReplaceAll);
                doc.Content.Find.Execute(FindText: "АдресПолучателя", ReplaceWith: Adress, Replace: Word.WdReplace.wdReplaceAll);
                doc.Content.Find.Execute(FindText: "ТелефонПолучателя", ReplaceWith: Phone, Replace: Word.WdReplace.wdReplaceAll);
                doc.Content.Find.Execute(FindText: "ИННПолучателя", ReplaceWith: INN, Replace: Word.WdReplace.wdReplaceAll);
                doc.Content.Find.Execute(FindText: "ОГРНПолучателя", ReplaceWith: OGRN, Replace: Word.WdReplace.wdReplaceAll);
                doc.PrintOut();

                doc.Close();
                wordApp.Quit();
                File.Delete(newfile);

                return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }

}

using System;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Win32;

namespace WPFModernVerticalMenu
{
    /// <summary>
    /// Логика взаимодействия для PrintOrders.xaml
    /// </summary>
    public partial class PrintOrders : Window
    {
        public PrintOrders()
        {
            InitializeComponent();
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!DateTime.TryParse(txtStartDate.Text, out DateTime startTime) || !DateTime.TryParse(txtEndDate.Text, out DateTime endTime) || endTime < startTime)
                    throw new Exception("Даты указаны некорректно");

                ReporGenerate(startTime, endTime);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void ReporGenerate(DateTime start, DateTime end)
        {
            try
            {
                SaveFileDialog sfd = new SaveFileDialog()
                {
                    FileName = $"Отчет - {DateTime.Now:dd.MM.yyyy}.pdf",
                    Filter = "PDF Files (*.pdf)|*.pdf|All Files (*.*)|*.*",
                    DefaultExt = "pdf",
                    AddExtension = true
                };

                if (sfd.ShowDialog() == true)
                {
                    var ordList = MSDataEntities.GetContext().Orders.ToList().Where(x => DateTime.Parse(x.creationDate) >= start && DateTime.Parse(x.creationDate) <= end);

                    // Создаем документ PDF
                    Document doc = new Document(PageSize.A4, 50, 50, 25, 25);
                    string fileName = sfd.FileName;
                    PdfWriter.GetInstance(doc, new FileStream(fileName, FileMode.Create));
                    doc.Open();

                    // Заголовок
                    var titleFont = GetRussianFont(16, Font.BOLD);
                    var headerFont = GetRussianFont(12, Font.BOLD);
                    var regularFont = GetRussianFont(10);

                    doc.Add(new Paragraph("Заказы", titleFont));
                    doc.Add(new Paragraph($"Дата и время формирования: {DateTime.Now:dd.MM.yyyy HH:mm}", regularFont));
                    doc.Add(Chunk.NEWLINE);

                    // Таблица с данными
                    PdfPTable table = new PdfPTable(6);
                    table.WidthPercentage = 100;
                    table.SetWidths(new float[] { 3, 2, 2, 2, 3, 2 });

                    // Заголовки таблицы
                    table.AddCell(new Phrase("Номер заказа", headerFont));
                    table.AddCell(new Phrase("ФИО Клиента", headerFont));
                    table.AddCell(new Phrase("ФИО Менеджера", headerFont));
                    table.AddCell(new Phrase("Сумма", headerFont));
                    table.AddCell(new Phrase("Статус", headerFont));
                    table.AddCell(new Phrase("Дата", headerFont));

                    // Данные
                    foreach (var o in ordList)
                    {
                        table.AddCell(new Phrase(o.orderNumber, regularFont));
                        table.AddCell(new Phrase(o.Client.recipientFullName, regularFont));
                        table.AddCell(new Phrase(o.Employee.fullName, regularFont));
                        table.AddCell(new Phrase(o.price.ToString() + "руб.", regularFont));
                        table.AddCell(new Phrase(o.orderStatus, regularFont));
                        table.AddCell(new Phrase(o.creationDate, regularFont));
                    }

                    doc.Add(table);
                    doc.Add(Chunk.NEWLINE);
                    doc.Add(Chunk.NEWLINE);
                    doc.Add(new Paragraph($"Активных: {ordList.Count(x => x.orderStatus != "Завершён" && x.orderStatus != "Отменён")}", regularFont));
                    doc.Add(new Paragraph($"Завершённых: {ordList.Count(x => x.orderStatus == "Завершён")}", regularFont));
                    doc.Add(new Paragraph($"Отменённых: {ordList.Count(x => x.orderStatus == "Отменён")}", regularFont));
                    doc.Add(new Paragraph($"ВСЕГО: {ordList.Count()}", headerFont));

                    doc.Close();

                    Process.Start(fileName);
                    MessageBox.Show("Отчет успешно сгенерирован!");
                    Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при генерации отчета: {ex.Message}");
            }

        }

        private Font GetRussianFont(float size, int style = Font.NORMAL)
        {
            // Путь к шрифту (можно использовать стандартные шрифты Windows)
            string fontPath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "arial.ttf");

            // Если шрифт не найден, попробуем использовать базовый шрифт (менее надежно)
            if (!File.Exists(fontPath))
            {
                return FontFactory.GetFont("Arial", BaseFont.IDENTITY_H, BaseFont.EMBEDDED, size, style);
            }

            // Используем шрифт с поддержкой кириллицы
            BaseFont baseFont = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            return new Font(baseFont, size, style);
        }

    }
}

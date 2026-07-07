using System;
using System.IO;
using System.Diagnostics;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Win32;

namespace WPFModernVerticalMenu.Pages
{
    /// <summary>
    /// Логика взаимодействия для ProductionPlanPage.xaml
    /// </summary>
    public partial class ProductionPlanPage : Page
    {
        ApiService api;
        public ProductionPlanPage()
        {
            InitializeComponent();
            api = new ApiService();
            ReloadList();
            FilterCB.SelectedIndex = 0;
        }

        async void ReloadList()
        {
            lvProductionPlan.ItemsSource = null;
            IEnumerable<CreateProduct> ppList;
            if (FilterCB.SelectedIndex == 2)
                ppList = (await api.GetCreateProductsAsync()).Where(x => x.status == "Выполнено");
            else if (FilterCB.SelectedIndex == 1)
                ppList = (await api.GetCreateProductsAsync()).ToList().Where(x => x.status == "Выполняется");
            else
                ppList = (await api.GetCreateProductsAsync()).ToList();

            foreach (var x in ppList)
            {
                x.Order = await api.GetOrderAsync(int.Parse(x.forOrder.ToString()));
                x.Product = await api.GetProductAsync(x.productId);
            }

            lvProductionPlan.ItemsSource = ppList;
        }

        async private void lvProductionPlan_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (Helper.employee.Position.position1 == "Нач. Производства" && MessageBox.Show("Данные позиции были переданы на склад?", "План", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                var cProduct = lvProductionPlan.SelectedItem as CreateProduct;
                cProduct.status = "Выполнено";
                try
                {
                    await api.UpdateCreateProductAsync(cProduct.id, cProduct);
                }
                catch
                {

                }
                if ((await api.GetCreateProductsAsync()).Count(x => x.forOrder == cProduct.forOrder && x.status == "Выполняется") == 0)
                {
                    var order2 = await api.GetOrderAsync(int.Parse(cProduct.forOrder.ToString()));
                    order2.orderStatus = "Собирается";
                    try
                    {
                        await api.UpdateOrderAsync(order2.id, order2);
                    }
                    catch
                    {

                    }
                }

            }
            ReloadList();
        }

        private void FilterCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ReloadList();
        }

        private async void btnPrintProductionReport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SaveFileDialog sfd = new SaveFileDialog()
                {
                    FileName = $"Отчет по производству за {DateTime.Now:dd.MM.yyyy}.pdf",
                    Filter = "PDF Files (*.pdf)|*.pdf|All Files (*.*)|*.*",
                    DefaultExt = "pdf",
                    AddExtension = true
                };

                if (sfd.ShowDialog() == true)
                {
                    // Получаем данные из базы
                    var productionPlan = await api.GetCreateProductsAsync();
                    var products = await api.GetProductsAsync();
                    var orders = await api.GetOrdersAsync();

                    // Создаем документ PDF
                    Document doc = new Document(PageSize.A4, 50, 50, 25, 25);
                    string fileName = sfd.FileName;
                    PdfWriter.GetInstance(doc, new FileStream(fileName, FileMode.Create));
                    doc.Open();

                    // Шрифты
                    var titleFont = GetRussianFont(14, Font.BOLD);
                    var headerFont = GetRussianFont(10, Font.BOLD);
                    var regularFont = GetRussianFont(8);
                    var smallFont = GetRussianFont(7);

                    // Заголовок отчета
                    doc.Add(new Paragraph("ОТЧЕТ ПО ПРОИЗВОДСТВЕННОМУ ПЛАНУ", titleFont) { Alignment = Element.ALIGN_CENTER });
                    doc.Add(new Paragraph($"Дата формирования: {DateTime.Now:dd.MM.yyyy HH:mm:ss}", regularFont));
                    doc.Add(new Paragraph($"АО 'Аврора'", regularFont));
                    doc.Add(Chunk.NEWLINE);

                    // 1. Общая статистика производства
                    doc.Add(new Paragraph("ОБЩАЯ СТАТИСТИКА ПРОИЗВОДСТВА", headerFont));
                    doc.Add(new Paragraph($"Всего записей в плане: {productionPlan.Count}", regularFont));

                    var totalPlanned = productionPlan.Sum(p => p.countProduct);
                    doc.Add(new Paragraph($"Всего запланировано к производству: {totalPlanned} единиц", regularFont));

                    var inProgressCount = productionPlan.Count(p => p.status == "В производстве");
                    doc.Add(new Paragraph($"В производстве: {inProgressCount} позиций", regularFont));

                    var completedCount = productionPlan.Count(p => p.status == "Завершено");
                    doc.Add(new Paragraph($"Завершено производство: {completedCount} позиций", regularFont));
                    doc.Add(Chunk.NEWLINE);

                    // 2. Текущие производственные задачи
                    doc.Add(new Paragraph("ТЕКУЩИЕ ПРОИЗВОДСТВЕННЫЕ ЗАДАЧИ", headerFont));

                    var inProgressItems = productionPlan.Where(p => p.status == "В производстве")
                                                       .OrderByDescending(p => p.countProduct)
                                                       .Take(5)
                                                       .ToList();

                    if (inProgressItems.Any())
                    {
                        var table = new PdfPTable(4) { WidthPercentage = 100 };
                        table.AddCell(new PdfPCell(new Phrase("Продукт", headerFont)));
                        table.AddCell(new PdfPCell(new Phrase("Количество", headerFont)));
                        table.AddCell(new PdfPCell(new Phrase("Для заказа", headerFont)));
                        table.AddCell(new PdfPCell(new Phrase("Статус", headerFont)));

                        foreach (var item in inProgressItems)
                        {
                            var product = products.FirstOrDefault(p => p.id == item.productId);
                            var order = orders.FirstOrDefault(o => o.id == item.forOrder);

                            table.AddCell(new Phrase(product?.productName ?? "Неизвестно", regularFont));
                            table.AddCell(new Phrase(item.countProduct.ToString(), regularFont));
                            table.AddCell(new Phrase(order?.orderNumber ?? "Склад", regularFont));
                            table.AddCell(new Phrase(item.status, regularFont));
                        }
                        doc.Add(table);
                    }
                    else
                    {
                        doc.Add(new Paragraph("Нет текущих производственных задач", regularFont));
                    }
                    doc.Add(Chunk.NEWLINE);

                    // 3. Завершенные производственные задачи
                    doc.Add(new Paragraph("ЗАВЕРШЕННЫЕ ПРОИЗВОДСТВЕННЫЕ ЗАДАЧИ", headerFont));

                    var completedItems = productionPlan.Where(p => p.status == "Завершено")
                                                     .OrderByDescending(p => p.countProduct)
                                                     .Take(5)
                                                     .ToList();

                    if (completedItems.Any())
                    {
                        var table = new PdfPTable(4) { WidthPercentage = 100 };
                        table.AddCell(new PdfPCell(new Phrase("Продукт", headerFont)));
                        table.AddCell(new PdfPCell(new Phrase("Количество", headerFont)));
                        table.AddCell(new PdfPCell(new Phrase("Для заказа", headerFont)));
                        table.AddCell(new PdfPCell(new Phrase("Статус", headerFont)));

                        foreach (var item in completedItems)
                        {
                            var product = products.FirstOrDefault(p => p.id == item.productId);
                            var order = orders.FirstOrDefault(o => o.id == item.forOrder);

                            table.AddCell(new Phrase(product?.productName ?? "Неизвестно", regularFont));
                            table.AddCell(new Phrase(item.countProduct.ToString(), regularFont));
                            table.AddCell(new Phrase(order?.orderNumber ?? "Склад", regularFont));
                            table.AddCell(new Phrase(item.status, regularFont));
                        }
                        doc.Add(table);
                    }
                    else
                    {
                        doc.Add(new Paragraph("Нет завершенных производственных задач", regularFont));
                    }
                    doc.Add(Chunk.NEWLINE);

                    // 4. Статистика по продуктам
                    doc.Add(new Paragraph("СТАТИСТИКА ПО ПРОДУКТАМ", headerFont));

                    var productStats = productionPlan
                        .GroupBy(p => p.productId)
                        .Select(g => new {
                            ProductId = g.Key,
                            TotalCount = g.Sum(x => x.countProduct),
                            ProductName = products.FirstOrDefault(pr => pr.id == g.Key)?.productName ?? "Неизвестно"
                        })
                        .OrderByDescending(x => x.TotalCount)
                        .Take(3)
                        .ToList();

                    if (productStats.Any())
                    {
                        doc.Add(new Paragraph("Топ-3 продукта по объему производства:", regularFont));

                        foreach (var stat in productStats)
                        {
                            doc.Add(new Paragraph($"• {stat.ProductName}: {stat.TotalCount} единиц", regularFont));
                        }
                    }
                    doc.Add(Chunk.NEWLINE);

                    doc.Close();
                    Process.Start(fileName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при генерации отчёта: {ex.Message}");
            }
        }

        private Font GetRussianFont(float size, int style = Font.NORMAL)
        {
            string fontPath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "arial.ttf");

            if (!File.Exists(fontPath))
            {
                return FontFactory.GetFont("Arial", BaseFont.IDENTITY_H, BaseFont.EMBEDDED, size, style);
            }

            BaseFont baseFont = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            return new Font(baseFont, size, style);
        }
    }

}

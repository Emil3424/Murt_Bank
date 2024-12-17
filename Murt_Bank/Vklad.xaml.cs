using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout.Properties;
using System;
using System.Drawing;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Document = iText.Layout.Document;
using Paragraph = iText.Layout.Element.Paragraph;
using Table = iText.Layout.Element.Table;
using TextAlignment = iText.Layout.Properties.TextAlignment;

namespace Murt_Bank
{
    /// <summary>
    /// Логика взаимодействия для Vklad.xaml
    /// </summary>
    public partial class Vklad : Window
    {
        public Vklad()
        {
            InitializeComponent();
            stabdohod.Content = Dohod.stabdohod;
            optimdohod.Content = Dohod.optimdohod;
            standartdohod.Content = Dohod.standartdohod;

            string stabdohodi = Dohod.stabdohod.Remove(Dohod.stabdohod.IndexOf(" "), 5);
            stabkonec.Content = Convert.ToInt32(stabdohodi) + Convert.ToInt32(Dohod.stabsumma) + " Руб.";
            stabdohodi = Dohod.optimdohod.Remove(Dohod.optimdohod.IndexOf(" "), 5);
            optimkonec.Content = Convert.ToInt32(stabdohodi) + Convert.ToInt32(Dohod.stabsumma) + " Руб.";
            stabdohodi = Dohod.standartdohod.Remove(Dohod.standartdohod.IndexOf(" "), 5);
            standartkonec.Content = Convert.ToInt32(stabdohodi) + Convert.ToInt32(Dohod.stabsumma) + " Руб.";
        }

        private void vklad_Click(object sender, RoutedEventArgs e)
        {
            auto auto = new auto();
            auto.Show();
            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Получение данных из интерфейса
            string stabdohodValue = stabdohod.Content.ToString(); // Данные для стабильного дохода
            string stabkonecValue = stabkonec.Content.ToString(); // Данные для стабильного срока
            string optimdohodValue = optimdohod.Content.ToString(); // Данные для оптимального дохода
            string optimkonecValue = optimkonec.Content.ToString(); // Данные для оптимального срока
            string standartdohodValue = standartdohod.Content.ToString(); // Данные для стандартного дохода
            string standartkonecValue = standartkonec.Content.ToString(); // Данные для стандартного срока

            // Путь для сохранения PDF
            string pdfPath = @"C:\Users\Эмиль Муртазин\Desktop\Document.pdf";

            // Создание PDF-документа
            CreatePdf(pdfPath, stabdohodValue, stabkonecValue, optimdohodValue, optimkonecValue, standartdohodValue, standartkonecValue);
        }

        private void CreatePdf(string pdfPath, string stabdohod, string stabkonec, string optimdohod, string optimkonec, string standartdohod, string standartkonec)
        {
            // Создание объекта PdfWriter
            using (PdfWriter writer = new PdfWriter(pdfPath))
            {
                // Создание документа
                using (PdfDocument pdf = new PdfDocument(writer))
                {
                    Document document = new Document(pdf);

                    var font = PdfFontFactory.CreateFont("C:\\Windows\\Fonts\\Arial.ttf", "Identity-H");
                    document.SetFont(font);

                    // Заголовок
                    document.Add(new Paragraph("Деньги в каждый карман")
                        .SetFontSize(18)
                        .SetFontColor(new DeviceRgb(27, 49, 80))
                        .SetTextAlignment(TextAlignment.RIGHT));

                    // Добавление изображения (путь к файлу изображения)
                    string imagePath = "C:\\Users\\Эмиль Муртазин\\Pictures\\wallet.png";
                    iText.Layout.Element.Image image = new iText.Layout.Element.Image(iText.IO.Image.ImageDataFactory.Create(imagePath))
                        .SetWidth(42)
                        .SetHeight(39)
                        .SetMarginLeft(500)
                        .SetMarginBottom(20);
                    document.Add(image);

                    // Заголовок таблицы
                    Table table = new Table(new float[] { 1, 1, 1, 1 })
                        .SetWidth(UnitValue.CreatePercentValue(100))
                        .SetMarginBottom(20);

                    table.AddHeaderCell("Название")
                        .SetFontSize(18)
                        .SetTextAlignment(TextAlignment.CENTER);
                    table.AddHeaderCell("Доход")
                        .SetFontSize(18)
                        .SetTextAlignment(TextAlignment.CENTER);
                    table.AddHeaderCell("Сумма к концу срока")
                        .SetFontSize(18)
                        .SetTextAlignment(TextAlignment.CENTER);
                    table.AddHeaderCell("Ставка")
                        .SetFontSize(18)
                        .SetTextAlignment(TextAlignment.CENTER);

                    // Данные для "Стабильного"
                    table.AddCell("Стабильный")
                        .SetFontSize(18);
                    table.AddCell(stabdohod)
                        .SetFontSize(18)
                        .SetTextAlignment(TextAlignment.CENTER);
                    table.AddCell(stabkonec)
                        .SetFontSize(18)
                        .SetTextAlignment(TextAlignment.CENTER);
                    table.AddCell("6,8 % Руб.")
                        .SetFontSize(18)
                        .SetTextAlignment(TextAlignment.CENTER);

                    // Данные для "Оптимального"
                    table.AddCell("Оптимальный")
                        .SetFontSize(18);
                    table.AddCell(optimdohod)
                        .SetFontSize(18)
                        .SetTextAlignment(TextAlignment.CENTER);
                    table.AddCell(optimkonec)
                        .SetFontSize(18)
                        .SetTextAlignment(TextAlignment.CENTER);
                    table.AddCell("5,6 % Руб.")
                        .SetFontSize(18)
                        .SetTextAlignment(TextAlignment.CENTER);

                    // Данные для "Стандартного"
                    table.AddCell("Стандарт")
                        .SetFontSize(18);
                    table.AddCell(standartdohod)
                        .SetFontSize(18)
                        .SetTextAlignment(TextAlignment.CENTER);
                    table.AddCell(standartkonec)
                        .SetFontSize(18)
                        .SetTextAlignment(TextAlignment.CENTER);
                    table.AddCell("6,05 % Руб.")
                        .SetFontSize(18)
                        .SetTextAlignment(TextAlignment.CENTER);

                    // Добавление таблицы в документ
                    document.Add(table);

                    // Добавление кнопки (Текст кнопки)
                    document.Add(new Paragraph("Сформировать выписку")
                        .SetFontSize(16)
                        .SetFontColor(iText.Kernel.Colors.ColorConstants.WHITE)
                        .SetBackgroundColor(new DeviceRgb(0, 120, 215))
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetPadding(10)
                        .SetMarginTop(30));
                }
            }

            MessageBox.Show("PDF создан успешно!");
        }
        private RenderTargetBitmap CopyUIElementToBitmap(FrameworkElement element)
        {
            double width = element.ActualWidth;
            double height = element.ActualHeight;
            RenderTargetBitmap bmpCopied = new RenderTargetBitmap((int)Math.Round(width), (int)Math.Round(height), 96, 96, PixelFormats.Default);
            DrawingVisual dv = new DrawingVisual();
            using (DrawingContext dc = dv.RenderOpen())
            {
                VisualBrush vb = new VisualBrush(element);
                dc.DrawRectangle(vb, null, new Rect(new System.Windows.Point(), new System.Windows.Size(width, height)));
            }
            bmpCopied.Render(dv);
            return bmpCopied;
        }

        //Функция для изменения размера картинки
        private Bitmap ResizeImage(Bitmap img, int width, int height)
        {
            Bitmap newImage = new Bitmap(width, height);
            using (Graphics graphics = Graphics.FromImage(newImage))
            {
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                graphics.DrawImage(img, 0, 0, width, height);
            }
            return newImage;
        }
    }
}

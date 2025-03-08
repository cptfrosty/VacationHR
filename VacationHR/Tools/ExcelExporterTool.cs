using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using OfficeOpenXml;

// Используется Nuget пакет EPPlus
// Ссылка Nuget https://www.nuget.org/packages/EPPlus/7.6.1#show-readme-container
// Сайт https://epplussoftware.com/
// Документация https://epplussoftware.com/docs/5.7/index.html

namespace VacationHR.Tools
{
    internal class ExcelExporterTool
    {
        public static bool ExportToExcel<T>(ObservableCollection<T> data, string filePath, string sheetName = "Sheet1")
        {
            try
            {
                // Лицензия EPPlus (необходимо для коммерческого использования)
                ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

                using (ExcelPackage excelPackage = new ExcelPackage())
                {
                    // Создаем лист Excel
                    ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add(sheetName);

                    // Получаем свойства класса T
                    var properties = typeof(T).GetProperties();

                    // Записываем заголовки столбцов
                    for (int i = 0; i < properties.Length; i++)
                    {
                        worksheet.Cells[1, i + 1].Value = properties[i].Name;
                    }

                    // Записываем данные
                    for (int row = 0; row < data.Count; row++)
                    {
                        for (int col = 0; col < properties.Length; col++)
                        {
                            var propertyValue = properties[col].GetValue(data[row]);
                            worksheet.Cells[row + 2, col + 1].Value = propertyValue?.ToString(); //Учитываем null значения
                        }
                    }

                    // Автоматическая ширина столбцов
                    worksheet.Cells.AutoFitColumns(0);

                    // Сохраняем файл
                    FileInfo excelFile = new FileInfo(filePath);
                    excelPackage.SaveAs(excelFile);

                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при экспорте в Excel: {ex.Message}");
                return false;
            }
        }
    }
}

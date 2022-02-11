using System;
using System.IO;
using System.Linq;
using iTextSharp.text.pdf;
using iTextSharp.text;

namespace ConsoleApp1
{
    class SortPDF
    {
        //Метод для создания необходимых директорий
        static void CreateDirectory(string sourceDirect)
        {
            for (int i = 0; i <= 4; i++)
            {
                Directory.CreateDirectory(sourceDirect + @"\Formating\А" + i);
            }
            Directory.CreateDirectory(sourceDirect + @"\Formating\Incorrect");
        }

        //Копирование файлов
        static void CopyPDF(string sourceDirect, string pdfDirect, string format)
        {
            try
            {
                File.Copy(pdfDirect, @$"{sourceDirect}\Formating\{format}\{Path.GetFileNameWithoutExtension(pdfDirect)} {format}{ Path.GetExtension(pdfDirect)}");
                return;
            }
            catch (Exception)
            {
                //Проверка на дубликаты, остается только последний измененный файл
                if (File.GetLastWriteTime(pdfDirect) > File.GetLastWriteTime(@$"{sourceDirect}\Formating\{format}\{Path.GetFileNameWithoutExtension(pdfDirect)} {format}{ Path.GetExtension(pdfDirect)}"))
                {
                    File.Copy(pdfDirect, @$"{sourceDirect}\Formating\{format}\{Path.GetFileNameWithoutExtension(pdfDirect)} {format}{ Path.GetExtension(pdfDirect)}", true);
                }
                else
                    return;
            }
        }

        //Метод сортировки файлов PDF
        static void Sorting(string sourceDirect, string pdfDirect, int pageSize)
        {
            if (84 >= (pageSize / 100000) & (pageSize / 100000) >= 76)// Формат А0
            {
                CopyPDF(sourceDirect, pdfDirect, "А0");
                return;
            }
            if (44 >= (pageSize / 100000) & (pageSize / 100000) >= 36)// Формат А1
            {
                CopyPDF(sourceDirect, pdfDirect, "А1");
                return;
            }
            if (24 >= (pageSize / 100000) & (pageSize / 100000) >= 16)// Формат А2
            {
                CopyPDF(sourceDirect, pdfDirect, "А2");
                return;
            }
            if (14 >= (pageSize / 100000) & (pageSize / 100000) >= 10)// Формат А3
            {
                CopyPDF(sourceDirect, pdfDirect, "А3");
                return;
            }
            if (9 >= (pageSize / 100000) & (pageSize / 100000) >= 3)// Формат А4 
            {
                CopyPDF(sourceDirect, pdfDirect, "А4");
                return;
            }
            else
                CopyPDF(sourceDirect, pdfDirect, "Incorrect");
            Console.WriteLine($"Размеры листа PDF {Path.GetFileName(pdfDirect)} не поддерживаются, он помещен в папку Incorrect, его размер: {pageSize}.");
            return;
        }

        static void Main()
        {
            Console.WriteLine("Укажите путь к папке в которой необходимо выполнить сортировку PDF по форматам:");
            string sourcelDirect = Console.ReadLine();
            CreateDirectory(sourcelDirect);

            //Поиск коллекции всех PDF по директории в которой находится файл программы
            var files = from file in Directory.EnumerateFiles(sourcelDirect, "*.pdf", SearchOption.AllDirectories)
                        select new
                        {
                            File = file
                        };
            int countFiles = 0;
            string[] allDirect = new string[files.Count()];
            foreach (var item in files)
            {
                allDirect[countFiles] = item.File;
                countFiles++;
            }

            //Основная часть программы, которая перебирает все PDF и сортирует их по папкам
            for (int i = 0; i < allDirect.Length; i++)
            {
                //Дополнение itextsharp.dll
                PdfReader reader = new PdfReader(allDirect[i]);
                Rectangle pagesize = reader.GetPageSize(1);

                //Попытка выполнить программу или выкинуть ошибку
                try
                {
                    Sorting(sourcelDirect, allDirect[i], Convert.ToInt32(pagesize.Width) * Convert.ToInt32(pagesize.Height));
                }
                catch
                {
                    Console.WriteLine("Произошла ошибка с файлом:" + Path.GetFileNameWithoutExtension(allDirect[i]));
                    continue;
                }
            }

            Console.WriteLine("Количество обработанных файлов: " + countFiles);
            Console.ReadLine();
        }
    }
}
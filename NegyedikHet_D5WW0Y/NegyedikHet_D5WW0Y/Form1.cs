using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;   //5.
using System.Reflection;                        

namespace NegyedikHet_D5WW0Y
{
    public partial class Form1 : Form
    {

        List<Flat> Flats;       //4.1
        RealEstateEntities context = new RealEstateEntities();      //4.2

        Excel.Application xlApp; // A Microsoft Excel alkalmazás        //6.1
        Excel.Workbook xlWB; // A létrehozott munkafüzet
        Excel.Worksheet xlSheet; // Munkalap a munkafüzeten belül

        public Form1()
        {
            InitializeComponent();
            LoadData();
            CreateExcel();
        }

        private void LoadData()                     //4.3
        {
            Flats = context.Flat.ToList();              //4.4
        }

        private void CreateExcel()
        {
            try
            {
                // Excel elindítása és az applikáció objektum betöltése
                xlApp = new Excel.Application();

                // Új munkafüzet
                xlWB = xlApp.Workbooks.Add(Missing.Value);

                // Új munkalap
                xlSheet = xlWB.ActiveSheet;

                // Tábla létrehozása
                CreateTable();

                // Control átadása a felhasználónak
                xlApp.Visible = true;
                xlApp.UserControl = true;
            }

            catch (Exception ex) // Hibakezelés a beépített hibaüzenettel
            {
                string errMsg = string.Format("Error: {0}\nLine: {1}", ex.Message, ex.Source);
                MessageBox.Show(errMsg, "Error");

                // Hiba esetén az Excel applikáció bezárása automatikusan
                xlWB.Close(false, Type.Missing, Type.Missing);
                xlApp.Quit();
                xlWB = null;
                xlApp = null;
            }
        }

        private void CreateTable()              //7.1
        {
            string[] headers = new string[] {   //7.2
                 "Kód",
                 "Eladó",
                 "Oldal",
                 "Kerület",
                 "Lift",
                 "Szobák száma",
                 "Alapterület (m2)",
                 "Ár (mFt)",
                 "Négyzetméter ár (Ft/m2)" };

            for (int i = 0; i < headers.Length; i++)        //7.3
            {
                xlSheet.Cells[1, 1] = headers[0];
            }

            object[,] values = new object[Flats.Count, headers.Length];     //7.4

            int counter = 0;
            foreach (Flat f in Flats)                   //7.5
            {
                values[counter, 0] = f.Code;
                values[counter, 1] = f.Vendor;
                values[counter, 2] = f.Side;
                values[counter, 3] = f.District;

                if (f.Elevator) { values[counter, 4] = "Van"; }     //7.6
                else { values[counter, 4] = "Nincs"; }

                values[counter, 5] = f.NumberOfRooms;
                values[counter, 6] = f.FloorArea;
                values[counter, 7] = f.Price;
                values[counter, 8] = "Négyzetméter ár (Ft/m2)";     //7.9?
                counter++;
            }

            xlSheet.get_Range(                                      //7.8
             GetCell(2, 1),
             GetCell(1 + values.GetLength(0), values.GetLength(1))).Value2 = values;
        }

        private string GetCell(int x, int y)            //7.7 Excel koordináták meghatározása
        {
            string ExcelCoordinate = "";
            int dividend = y;
            int modulo;

            while (dividend > 0)
            {
                modulo = (dividend - 1) % 26;
                ExcelCoordinate = Convert.ToChar(65 + modulo).ToString() + ExcelCoordinate;
                dividend = (int)((dividend - modulo) / 26);
            }
            ExcelCoordinate += x.ToString();

            return ExcelCoordinate;
        }
    }
}

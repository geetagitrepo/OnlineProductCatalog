using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using OfficeOpenXml.Table;
using ProductCatalog.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace OnlineCatalogApp.Helper
{
    public class ExcelHelper
    {
        public static ExcelPackage CreateExcelPackage<T>(IList<T> data)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            var dateFormat = "DD/MM/YYYY";
            int headerRowIndex = 1;
            int indexCounter = 0;
            var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Records");
            int columnsCounter = 0;

            foreach (PropertyDescriptor prop in properties)
            {
                if(Convert.ToString(prop.PropertyType).Contains("Byte"))
                {
                    continue;
                }
                else
                {
                    columnsCounter = headerRowIndex + indexCounter;
                    worksheet.Cells[headerRowIndex, columnsCounter].Value = prop.Name;
                    indexCounter++;
                }
            }

            headerRowIndex = 1;
            indexCounter = 1;

            foreach (T item in data)
            {
                foreach (PropertyDescriptor prop in properties)
                {
                    if (Convert.ToString(prop.PropertyType).Contains("Byte"))
                    {
                        continue;
                    }
                    else
                    {
                        worksheet.Cells[headerRowIndex + 1, indexCounter].Value = prop.GetValue(item) ?? DBNull.Value;
                        if (Convert.ToString(prop.PropertyType).Contains("Date"))
                        {
                            worksheet.Cells[headerRowIndex + 1, indexCounter].Style.Numberformat.Format = dateFormat;
                        }

                        indexCounter++;
                    }
                }
                headerRowIndex++;
                indexCounter = 1;
            }
                        
            // Add to table / Add summary row
            var tbl = worksheet.Tables.Add(new ExcelAddressBase(fromRow: 1, fromCol: 1, toRow: headerRowIndex, toColumn: columnsCounter), "Data");
            tbl.ShowHeader = true;
            tbl.TableStyle = TableStyles.Light1;
            tbl.ShowTotal = true;
            worksheet.Cells[1, 1, columnsCounter - 1, columnsCounter - 1].AutoFitColumns();
            
            return package;

        }
    }
}

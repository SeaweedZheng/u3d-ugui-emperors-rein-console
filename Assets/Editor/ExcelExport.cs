using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;
using System.IO;
using System.Data;

namespace Kengine.Tools
{
    public class ExcelExport
    {
        [MenuItem("Tools/Export Language")]
        public static void SerializeLanguageJson()
        {
            foreach (var kvp in ReadExcels())
            {
                string outFilePath = kvp.Key + ".json";
                DataTable table = kvp.Value;
                if (File.Exists(outFilePath)) File.Delete(outFilePath);
                string content = "";
                DataRow langTypes = table.Rows[2];
                content += "{\n";
                for (int i = 1; i < langTypes.ItemArray.Length; i++)
                {
                    content += "    @" + langTypes[i].ToString() + "@:{\n";
                    for (int j = 3; j < table.Rows.Count; j++)
                    {
                        DataRow dr = table.Rows[j];
                        string strFormat = j < table.Rows.Count - 1 ? "," : "";
                        content += "        @" + dr[0].ToString() + "@:@" + dr[i] + "@" + strFormat + "\n";
                    }
                    string str = i < langTypes.ItemArray.Length - 1 ? ",\n" : "\n";
                    content += "    }" + str;
                }
                content += "}";
                content = content.Replace('@', '"');
                Utils.FileWriteByCreate(content, outFilePath);
                AssetDatabase.Refresh();
            }
        }

        /// <summary>
        /// 文件写入
        /// </summary>
        /// <param name="content"></param>
        /// <param name="outFilePath"></param>
        private static void FileWriteByCreate(string content, string outFilePath)
        {
            FileStream fs = new FileStream(outFilePath, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            sw.Write(content);
            sw.Flush();
            sw.Close();
            fs.Close();
        }

        /// <summary>
        /// 读取excel表
        /// </summary>
        /// <returns></returns>
        private static Dictionary<string, DataTable> ReadExcels()
        {
            UnityEngine.Object[] selects = Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.DeepAssets);
            if (selects.Length < 0)
            {
                Debug.LogError("Please select somthing");
                return null;
            }

            Clear();

            Dictionary<string, DataTable> dict = new Dictionary<string, DataTable>();
            for (int i = 0; i < selects.Length; i++)
            {
                string oriPath = AssetDatabase.GetAssetPath(selects[i]);
                if (oriPath.EndsWith(".xlsx") || oriPath.EndsWith(".xls"))
                {
                    string key = oriPath.Substring(0, oriPath.Length - Path.GetExtension(oriPath).Length);
                    dict.Add(key, GetDataTable(oriPath));
                }
            }
            return dict;
        }

        public static void Clear()
        {
            UnityEngine.Object[] selects = Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.DeepAssets);
            if (selects.Length < 0)
            {
                Debug.LogError("Please select somthing");
                return;
            }
            
            for (int i = 0; i < selects.Length; i++)
            {
                string oriPath = AssetDatabase.GetAssetPath(selects[i]);
                if (oriPath.EndsWith(".xlsx") || oriPath.EndsWith(".xls")) continue;
                if (Path.HasExtension(oriPath))
                {
                    File.Delete(oriPath);
                }
            }
            AssetDatabase.Refresh();
        }

        private static DataTable GetDataTable(string fliePath)
        {
            IWorkbook workbook = null;
            ISheet sheet;
            DataTable data = new DataTable();
            FileStream fs = new FileStream(fliePath, FileMode.Open, FileAccess.Read);

            if (Path.GetExtension(fliePath) == ".xlsx")
            {
                workbook = new XSSFWorkbook(fs);
            }
            else 
            {
                workbook = new HSSFWorkbook(fs);
            }

            sheet = workbook.GetSheetAt(0);

            if (sheet != null)
            {
                IRow row;
                DataRow dataRow;
                int colMax = 0;
                for (int i = 0; i <= sheet.LastRowNum; i++)
                {
                    row = sheet.GetRow(i);
                    if (row != null && row.LastCellNum > colMax)
                    {
                        colMax = row.LastCellNum;
                    }
                }
                for (int i = 0; i < colMax; i++)
                {
                    data.Columns.Add(new DataColumn());
                }
                for (int i = 0; i <= sheet.LastRowNum; i++)
                {
                    row = sheet.GetRow(i);   //row读入第i行数据
                    if (row != null)
                    {
                        dataRow = data.NewRow();
                        for (int j = 0; j < row.LastCellNum; j++)  //对工作表每一列
                        {
                            ICell cell = row.GetCell(j);
                            string cellValue = cell == null ? "" : cell.ToString(); //获取i行j列数据
                            dataRow[j] = cellValue;
                        }
                        data.Rows.Add(dataRow);
                    }
                }
            }
            fs.Close();
            return data;
        }
    }
}

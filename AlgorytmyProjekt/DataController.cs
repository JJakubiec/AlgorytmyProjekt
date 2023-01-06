using FastExcel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace AlgorytmyProjekt
{
    public enum DataSize { 
        SmallestData,
        SmallData,
        MediumData,
        LargeData
    }

    public class DataController
    {
        public float[,] data;
        public int dataLength;
        private Dictionary<DataSize, string> dic = new Dictionary<DataSize, string> { { DataSize.SmallestData, "C:\\Users\\jaros\\source\\repos\\AlgorytmyProjekt\\AlgorytmyProjekt\\data\\TSP_29.xlsx" },
                                                                                      { DataSize.SmallData, "C:\\Users\\jaros\\source\\repos\\AlgorytmyProjekt\\AlgorytmyProjekt\\data\\TSP_48.xlsx" },
                                                                                      { DataSize.MediumData, "C:\\Users\\jaros\\source\\repos\\AlgorytmyProjekt\\AlgorytmyProjekt\\data\\TSP_76_fix.xlsx" },
                                                                                      { DataSize.LargeData, "C:\\Users\\jaros\\source\\repos\\AlgorytmyProjekt\\AlgorytmyProjekt\\data\\TSP_127.xlsx" }};
        public DataController(DataSize dataSize) {
            ReadData(dic[dataSize]);
        }

        private void ReadData(string dataPath) {
            var inputFile = new FileInfo(dataPath);

            using (FastExcel.FastExcel fastExcel = new FastExcel.FastExcel(inputFile, true))
            {
                var worksheet = fastExcel.Read(1);
                var rows = worksheet.Rows.Count();
                this.dataLength = rows - 1; 
                var rowIndex = 0;

                data = new float[rows,rows];

                foreach (var row in worksheet.Rows) 
                {

                    if (rowIndex != 0)
                    {
                        var colIndex = 0;
                        foreach (var cell in row.Cells)
                        {
                            if (colIndex != 0)
                            {
                                var dis = float.Parse(cell.ToString(), CultureInfo.InvariantCulture.NumberFormat);
                                if (dis > 0)
                                {
                                    data[rowIndex, colIndex] = dis;
                                }
                            }
                            
                            colIndex++;
                        }
                    }
                    rowIndex++;
                }
            }
        }

        public float countPathDistance(List<int> path)
        {
            var currentRowIndex = path.First();
            var sum = 0.0f;

            path.ForEach(a => {
                sum += data[currentRowIndex, a];
                currentRowIndex = a;
            });

            return sum + data[path.Last(), path.First()];
        }
    }
}

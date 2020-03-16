using System.Data;
using System.Text;

namespace Persistity.Endpoints.Database.Extensions
{
    public static class IDataSetExtensions
    {
        public static string ToJson(this DataSet dataset)
        {
            var jsonString = new StringBuilder();
            if (dataset == null || dataset.Tables[0].Rows.Count <= 0) 
            { return null; }
            
            jsonString.Append("[");
            for (var i = 0; i < dataset.Tables[0].Rows.Count; i++)
            {
                jsonString.Append("{");
                for (var j = 0; j < dataset.Tables[0].Columns.Count; j++)
                {
                    if (j < dataset.Tables[0].Columns.Count - 1)
                    {
                        jsonString.Append($"\"{dataset.Tables[0].Columns[j].ColumnName}\":\"{dataset.Tables[0].Rows[i][j]}\",");
                    }
                    else if (j == dataset.Tables[0].Columns.Count - 1)
                    {
                        jsonString.Append($"\"{dataset.Tables[0].Columns[j].ColumnName}\":\"{dataset.Tables[0].Rows[i][j]}\"");
                    }
                }
                if (i == dataset.Tables[0].Rows.Count - 1)
                {
                    jsonString.Append("}");
                }
                else
                {
                    jsonString.Append("},");
                }
            }
            jsonString.Append("]");
            return jsonString.ToString();

        }

        public static string ToJson(this DataTable dataTable)
        {
            var dataset = new DataSet();
            dataset.Merge(dataTable);
            var jsonString = new StringBuilder();
            if (dataset.Tables[0].Rows.Count <= 0) { return null; }

            jsonString.Append("[");
            for (var i = 0; i < dataset.Tables[0].Rows.Count; i++)
            {
                jsonString.Append("{");
                for (var j = 0; j < dataset.Tables[0].Columns.Count; j++)
                {
                    if (j < dataset.Tables[0].Columns.Count - 1)
                    {
                        jsonString.Append($"\"{dataset.Tables[0].Columns[j].ColumnName}\":\"{dataset.Tables[0].Rows[i][j]}\",");
                    }
                    else if (j == dataset.Tables[0].Columns.Count - 1)
                    {
                        jsonString.Append($"\"{dataset.Tables[0].Columns[j].ColumnName}\":\"{dataset.Tables[0].Rows[i][j]}\"");
                    }
                }
                if (i == dataset.Tables[0].Rows.Count - 1)
                {
                    jsonString.Append("}");
                }
                else
                {
                    jsonString.Append("},");
                }
            }
            jsonString.Append("]");
            return jsonString.ToString();

        }

        public static string ToXml(this DataTable dataTable)
        {
            var dataset = new DataSet();
            dataset.Merge(dataTable);
            return dataset.GetXml();
        }
    }
}
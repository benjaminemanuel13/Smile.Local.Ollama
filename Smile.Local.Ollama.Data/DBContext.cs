using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Smile.Local.Ollama.Data
{
    public class DBContext : IDBContext
    {
        private string conn = "Server=DESKTOP-CH8V84O\\SQLEXPRESS;Database=smile-local;User Id=aiuser;Password=n7gHiuB27.;TrustServerCertificate=True;";

        public void SaveDocumentEmbeddings(int id, float[] embeddings)
        {
            SqlConnection con = new SqlConnection(conn);
            con.Open();
            
            var sp = "Upsert_Documents_Embeddings";
            SqlCommand cmd = con.CreateCommand();
            cmd.Connection = con;

            cmd.CommandText= sp;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.Add(new SqlParameter("@id", id));
            cmd.Parameters.Add(new SqlParameter("@embeddings", JsonSerializer.Serialize(embeddings)));

            cmd.ExecuteNonQuery();
        }

        public int SaveDocument(string title, string text)
        {
            SqlConnection con = new SqlConnection(conn);
            con.Open();

            var sp = "Add_Document";
            SqlCommand cmd = con.CreateCommand();
            cmd.Connection = con;

            cmd.CommandText = sp;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.Add(new SqlParameter("@title", title));
            cmd.Parameters.Add(new SqlParameter("@text", text));
            cmd.Parameters.Add(new SqlParameter("@externalId", "0"));
            cmd.Parameters.Add(new SqlParameter("@startTime", DateTime.Now));
            cmd.Parameters.Add(new SqlParameter("@endTime", DateTime.Now));
            cmd.Parameters.Add(new SqlParameter("@require", true));

            var outId = new SqlParameter("@newId", SqlDbType.Int) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(outId);

            cmd.ExecuteNonQuery();

            int id = int.Parse(outId.Value.ToString());

            return id;
        }

        public List<string> GetDocuments(string text/*, float[] embeddings*/)
        {
            SqlConnection con = new SqlConnection(conn);
            con.Open();

            var sp = "Get_Documents";
            SqlCommand cmd = con.CreateCommand();
            cmd.Connection = con;

            cmd.CommandText = sp;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.Add(new SqlParameter("@text", text));
            cmd.Parameters.Add(new SqlParameter("@threshold", 0.45));

            //cmd.Parameters.Add(new SqlParameter("@embeddings", JsonSerializer.Serialize(embeddings)));

            var recs = cmd.ExecuteReader();

            List<string> result = new List<string>();

            while (recs.Read())
            {
                string doc = recs.GetString(1);
                result.Add(doc);
            }

            return result;
        }
    }
}

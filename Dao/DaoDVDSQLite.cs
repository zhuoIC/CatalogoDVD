using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace Dao
{
    class DaoDVDSQLite: IDAO
    {
        private SQLiteConnection conexion;
        public bool Conectar(string srv, string db, string user, string pwd)
        {
            string cadenaConexion = "Data Source=" + db + ";Version=3;" + "FailIfMissing=true;";
            try
            {
                conexion = new SQLiteConnection(cadenaConexion);
                conexion.Open();
                return true;
            }
            catch (SQLiteException ex)
            {

                throw new Exception("Error de conexión" + ex.Data);
            }
        }

        public void Desconectar()
        {
            try
            {
                if (conexion != null)
                {
                    conexion.Close();
                }
            }
            catch (SQLiteException)
            {
                
                throw;
            }
        }

        public bool Conectado()
        {
            if (conexion != null)
            {
                return conexion.State == ConnectionState.Open;
            }
            else
            {
                return false;
            }
        }

        public DataTable SeleccionarTB(string codigo)
        {
            DataTable dt = new DataTable();
            string sql;
            if (codigo == null)
            {
                sql = "select codigo, titulo, artista, pais, compania, precio, anio from dvd";
            }
            else
            {
                sql = "select codigo, titulo, artista, pais, compania, precio, anio from dvd where codigo = " + codigo;
            }
            SQLiteCommand cmd = new SQLiteCommand(sql, conexion);
            SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
            da.Fill(dt);
            return dt;
        }

        public ObservableCollection<Dvd> Seleccionar(string codigo)
        {
            throw new NotImplementedException();
        }

        public int Borrar(string codigo)
        {
            throw new NotImplementedException();
        }


        public ObservableCollection<Dvd> SeleccionarPA(string codigo)
        {
            return null;
        }
    }
}

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

        public Pais SeleccionarPais(string iso2)
        {
            Pais resultado = null;
            string orden;
            if (iso2 != null)
            {
                resultado = new Pais();
                orden = "select nombre from pais where iso2 ='" + iso2 + "'";

                SQLiteCommand cmd = new SQLiteCommand(orden, conexion);
                try
                {
                    object salida = cmd.ExecuteScalar();
                    if (salida != null)
                    {
                        resultado.Iso2 = iso2;
                        resultado.Nombre = salida.ToString();
                    }
                }
                catch (SQLiteException)
                {

                    throw new Exception("No tiene permisos para ejecutar esta orden");
                }
            }
            return resultado;
        }

        public ObservableCollection<Dvd> Seleccionar(string codigo)
        {
            ObservableCollection<Dvd> resultado = new ObservableCollection<Dvd>();
            Dvd unDvd;
            string orden;
            if (codigo == null)
            {
                orden = "select codigo, titulo, artista, pais, compania, precio, anio from dvd";
            }
            else
            {
                orden = "select codigo, titulo, artista, pais, compania, precio, anio from dvd where codigo = '" + codigo + "'";
            }
            SQLiteCommand cmd = new SQLiteCommand(orden, conexion);

            try
            {
                SQLiteDataReader lector = cmd.ExecuteReader();

                while (lector.Read())
                {
                    unDvd = new Dvd();
                    unDvd.Codigo = int.Parse(lector["codigo"].ToString());
                    unDvd.Titulo = lector["titulo"].ToString();
                    unDvd.Artista = lector["artista"].ToString();
                    unDvd.Pais = lector["pais"].ToString();
                    unDvd.Compania = lector["compania"].ToString();
                    unDvd.Precio = double.Parse(lector["precio"].ToString());
                    unDvd.Anio = lector["anio"].ToString();
                    resultado.Add(unDvd);
                }
                lector.Close();
                return resultado;
            }
            catch (SQLiteException)
            {
                throw new Exception("No tiene permisos para ejecutar esta orden");
            }
        }

        public int Borrar(Dvd unDvd)
        {
            string orden;
            if (unDvd != null)
            {
                orden = "delete from dvd where codigo = " + unDvd.Codigo;
                SQLiteCommand cmd = new SQLiteCommand(orden, conexion);
                return cmd.ExecuteNonQuery();
            }
            else
            {
                return -1;
            }
        }

        public int Actualizar(Dvd unDVD)
        {
            string orden;
            if (unDVD != null)
            {
                orden = "update dvd set titulo = '" + unDVD.Titulo + "',artista = '" + unDVD.Artista + "', pais = '" + unDVD.Pais + 
                    "', compania = '" + unDVD.Compania + "', precio = '" + unDVD.Precio.ToString().Replace(',','.') + "', anio = '" + unDVD.Anio + "' where codigo = " + unDVD.Codigo;
                SQLiteCommand cmd = new SQLiteCommand(orden, conexion);
                return cmd.ExecuteNonQuery();
            }
            else
            {
                return -1;
            }
        }


        public ObservableCollection<Dvd> SeleccionarPA(string codigo)
        {
            return null;
        }
    }
}

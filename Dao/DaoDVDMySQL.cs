using System;
using System.Collections.ObjectModel;
using System.Data;
using MySql.Data.MySqlClient;

namespace Dao
{
    public class DaoDVDMySQL: IDAO
    {
        public MySqlConnection conexion; // Encapsula odos los datos para realizar una conexion
        public bool Conectar(string srv, string db, string user, string pwd)
        {
            string cadenaConexion = "server=" + srv + ";database=" + db 
                + ";uid=" + user + ";pwd=" + pwd + ";";
            try
            {
                conexion = new MySqlConnection(cadenaConexion);
                conexion.Open();
                return true;
            }
            catch (MySqlException exception)
            {
                switch (exception.Number)
                {
                    case 0:
                        throw new Exception("Error de conexión");
                    case 1045:
                        throw new Exception("Error de identificación");
                    default:
                        throw;
                }
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
            catch (MySqlException)
            {
                
                throw;
            }      
        }

        public bool Conectado()
        {
            if (conexion != null)
            {
                return conexion.State == System.Data.ConnectionState.Open;
            }
            else
            {
                return false;
            }

        }

        public DataTable SeleccionarTB(string codigo) 
        {
            DataTable dt = new DataTable();

            string orden;
            if (codigo == null)
            {
                orden = "select codigo, titulo, artista, pais, compania, precio, anio from dvd";
            }
            else
            {
                orden = "select codigo, titulo, artista, pais, compania, precio, anio from dvd where codigo = '" + codigo + "'";
            }
            MySqlCommand cmd = new MySqlCommand(orden, conexion);
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
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

                MySqlCommand cmd = new MySqlCommand(orden, conexion);
                try
                {
                    object salida = cmd.ExecuteScalar();
                    if (salida != null)
                    {
                        resultado.Iso2 = iso2;
                        resultado.Nombre = salida.ToString();
                    }
                }
                catch (MySqlException)
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
                orden="select codigo, titulo, artista, pais, compania, precio, anio from dvd where codigo = '" + codigo + "'";
            }
            MySqlCommand cmd = new MySqlCommand(orden, conexion);

            try
            {
                MySqlDataReader lector = cmd.ExecuteReader();

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
            catch (MySqlException)
            {
                throw new Exception("No tiene permisos para ejecutar esta orden");
            }
        }

        public ObservableCollection<Dvd> SeleccionarPA(string codigo)
        {
            ObservableCollection<Dvd> resultado= new ObservableCollection<Dvd>();
            int resul;
            Dvd unDvd;
            MySqlCommand cmd = new MySqlCommand("selectDVD", conexion);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            
            // Armado de parámetros del procedimiento almacenado selectDVD
            cmd.Parameters.AddWithValue("@codi", codigo);
            cmd.Parameters.AddWithValue("@titu", null);
            cmd.Parameters.AddWithValue("@arti", null);
            cmd.Parameters.AddWithValue("@elPais", null);
            cmd.Parameters.AddWithValue("@comp", null);
            cmd.Parameters.AddWithValue("@prec", null);
            cmd.Parameters.AddWithValue("@elAnio", null);

            cmd.Parameters.AddWithValue("@resul", MySqlDbType.Int16);
            cmd.Parameters["@resul"].Direction = System.Data.ParameterDirection.Output;

            try
            {
                MySqlDataReader lector = cmd.ExecuteReader();
                resul = (int)cmd.Parameters["@resul"].Value;    // Por definir que hacer en cado de seul != 0

                while (lector.Read())
                {
                    unDvd = new Dvd();
                    unDvd.Codigo = int.Parse(lector["codigo"].ToString());
                    unDvd.Titulo = lector["titulo"].ToString();
                    unDvd.Artista = lector["artista"].ToString();
                    unDvd.Pais = lector["pais"].ToString();
                    unDvd.Compania = lector["compania"].ToString();
                    if (!DBNull.Value.Equals(lector["precio"]))
                    {
                        unDvd.Precio = double.Parse(lector["precio"].ToString()); 
                    }
                    unDvd.Anio = lector["anio"].ToString();
                    resultado.Add(unDvd);
                }
                lector.Close();
                return resultado;
            }
            catch (MySqlException)
            {
                throw new Exception("No tiene permisos para ejecutar esta orden");
            }
        }

        /// <summary>
        /// Borrado de filas de la tabla DVD
        /// </summary>
        /// <param name="codigo">Clave a eliminar</param>
        /// <returns>N=Numero de filas afectadas, -1 parametro dado es null</returns>
        public int Borrar(Dvd unDvd) 
        {
            string orden;
            if (unDvd != null)
            {
                orden = "delete from dvd where codigo = '" + unDvd.Codigo + "'";
                MySqlCommand cmd = new MySqlCommand(orden, conexion);
                return cmd.ExecuteNonQuery();//Devuelve el numero de filas afectadas por la orden
            }
            return -1;
        }

        public int Actualizar(Dvd unDVD)
        {
            string orden;
            if (unDVD != null)
            {
                orden = "update dvd set titulo = '" + unDVD.Titulo + "',artista = '" + unDVD.Artista + "', pais = '" + unDVD.Pais +
                    "', compania = '" + unDVD.Compania + "', precio = '" + unDVD.Precio.ToString().Replace(',', '.') + "', anio = '" + unDVD.Anio + "' where codigo = " + unDVD.Codigo;
                MySqlCommand cmd = new MySqlCommand(orden, conexion);
                return cmd.ExecuteNonQuery();
            }
            else
            {
                return -1;
            }
        }

        /*public int BorrarPA(string codigo) 
        {

        }*/
    }
}

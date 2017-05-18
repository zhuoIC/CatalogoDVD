using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Dao;

namespace Dao
{
    class CatalogoVM:INotifyPropertyChanged
    {
        // https://www.codeproject.com/articles/30905/WPF-DataGrid-Practical-Examples#layered_updates

        #region campos
        IDAO _dao;
        bool _tipoConexion = true; //Mysql: true, Sqlite: False
        ObservableCollection<Dvd> _listado;
        string _mensaje = "Sin datos";
        string _nombrePais = "<sin identificar>";


        Dvd _dvd;
        #endregion

        #region propiedades

        public Dvd DVDSeleccionado
        {
            get { return _dvd; }
            set
            {
                if (_dvd != value)
                {
                    _dvd = value;
                    if (_dao.Conectado() && _dvd != null)
                    {
                        NombrePais = _dao.SeleccionarPais(_dvd.Pais).Nombre;
                    }
                    else
                    {
                        NombrePais = "<sin identificar>";
                    }
                    NotificarCambioDePropiedad("DVDNoSeleccionado");
                }
            }
        }

        public bool DVDNoSeleccionado
        {
            get { return DVDSeleccionado != null; }
        }

        public string NombrePais
        {
            get { return _nombrePais; }
            set { _nombrePais = value; }
        }

        public string Mensaje
        {
            get { return _mensaje; }
            set
            {
                if (_mensaje != value)
                {
                    _mensaje = value;
                    NotificarCambioDePropiedad("Mensaje");
                }
            }
        }

        public ObservableCollection<Dvd> Listado
        {
            get { return _listado; }
            set
            {
                if (_listado != value)
                {
                    _listado = value;
                    NotificarCambioDePropiedad("Listado");
                }
            }
        }

        public bool Conectado 
        {
            get 
            {
                if (_dao==null)
                {
                    return false;
                }
                else
                {
                    return _dao.Conectado();
                }
            }
        }

        public bool NoConectado
        {
            get
            {
                return !Conectado;
            }
        }

        public bool TipoConexion
        {
            get { return _tipoConexion; }
            set
            {
                if (_tipoConexion != value)
                {
                    _tipoConexion = value;
                    NotificarCambioDePropiedad("TipoConexion");
                }
            }
        }

        public string ColorConectar
        {
            get 
            {
                if (Conectado)
                {
                    return "green";
                }
                else
                {
                    return "red";
                }
            }
            set
            {
                NotificarCambioDePropiedad("ColorConectar");
            }
        }
        #endregion

        #region eventos
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotificarCambioDePropiedad(string propiedad) 
        {
            PropertyChangedEventHandler manejador = PropertyChanged;
            if (manejador!=null)
            {
                manejador(this, new PropertyChangedEventArgs(propiedad));
            }
        }
        #endregion

        #region comandos
        private void ConectarBD() 
        {
            try
            {
                _dao = null;
                if (TipoConexion)//Mysql
                {
                    _dao = new DaoDVDMySQL();
                    _dao.Conectar("localhost", "catalogo", "usr_catalogo", "12345678");
                    Mensaje = "Conectado a MySQL / MariaDB";
                }
                else//SqLite
                {
                    _dao = new DaoDVDSQLite();
                    _dao.Conectar(null, "catalogo.db", null, null);
                    Mensaje = "Conectado a SQLite";
                }
            }
            catch (Exception e)
            {
                Mensaje = e.Message;
            }
            NotificarCambioDePropiedad("ColorConectar");
            NotificarCambioDePropiedad("Conectado");
            NotificarCambioDePropiedad("NoConectado");
        }
        private void DesconectarBD()
        {
            _dao.Desconectar();
            Mensaje = "Desconexión de la BD con éxito";
            Listado = null;
            NotificarCambioDePropiedad("ColorConectar");
            NotificarCambioDePropiedad("Conectado");
            NotificarCambioDePropiedad("NoConectado");
        }

        public void LeerTodosDVD()
        {
            if (TipoConexion)
            {
                Listado = _dao.SeleccionarPA(null);
            }
            else
            {
                Listado = _dao.Seleccionar(null);
            }
            Mensaje = "Datos cargados";
        }

        public void BorrarDVD()
        {
            if (DVDSeleccionado != null) // Mysql vs SQlite
            {
                if (_dao.Borrar(DVDSeleccionado)==1)
                {
                    Mensaje = "DVD eliminado correctamente";
                }
                else
                {
                    Mensaje = "Error al intentar eliminar el DVD";
                }
            }
        }

        public void ActualizarDVD()
        {
            if (DVDSeleccionado != null) // Mysql vs SQlite
            {
                if (_dao.Actualizar(DVDSeleccionado) == 1)
                {
                    Mensaje = "DVD actualizado correctamente";
                }
                else
                {
                    Mensaje = "Error al intentar actualizar el DVD";
                }
            }
        }

        public ICommand ConectarBD_Click // Implementacion de ICommand
        {
            get 
            {
                return new RelayComand(o => ConectarBD(), o => true);
            }

        }
        public ICommand DesconectarBD_Click // Implementacion de ICommand
        {
            get
            {
                return new RelayComand(o => DesconectarBD(), o => true);
            }

        }
        public ICommand Listado_Click // Implementacion de ICommand
        {
            get
            {
                return new RelayComand(o => LeerTodosDVD(), o => true);
            }

        }
        public ICommand Borrar_Click // Implementacion de ICommand
        {
            get
            {
                return new RelayComand(o => BorrarDVD(), o => true);
            }

        }

        public ICommand Actualizar_Click // Implementacion de ICommand
        {
            get
            {
                return new RelayComand(o => ActualizarDVD(), o => true);
            }

        }
        #endregion
    }
}

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

        #region campos
        IDAO _dao;
        bool _tipoConexion = true; //Mysql: true, Sqlite: False
        ObservableCollection<Dvd> _listado;
        string _mensaje = "Sin datos";
        #endregion

        #region propiedades
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
                    _dao.Conectar("localhost", "catalogo", "usr_catalogo", "12345678");
                    Mensaje = "Conectado a MySQL / MariaDB";
                }
            }
            catch (Exception e)
            {
                Mensaje = e.Message;
            }
            NotificarCambioDePropiedad("ColorConectar");
            NotificarCambioDePropiedad("Conectado");
        }
        private void DesconectarBD()
        {
            _dao.Desconectar();
            Mensaje = "Desconexión de la BD con éxito";
            Listado = null;
            NotificarCambioDePropiedad("ColorConectar");
            NotificarCambioDePropiedad("Conectado");
        }

        public void LeerTodosDVD()
        {
            if (TipoConexion)
            {
                Listado = _dao.SeleccionarPA(null);
            }
            else
            {
                Listado = 
            }
            Mensaje = "Datos cargados";
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
        #endregion
    }
}

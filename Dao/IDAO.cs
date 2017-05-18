using System.Collections.ObjectModel;
using System.Data;

namespace Dao
{
    interface IDAO
    {
        bool Conectar(string srv, string db, string user, string pwd);
        void Desconectar();
        bool Conectado();
        DataTable SeleccionarTB(string codigo);
        ObservableCollection<Dvd> Seleccionar(string codigo);
        ObservableCollection<Dvd> SeleccionarPA(string codigo);
        Pais SeleccionarPais(string iso2);
        int Borrar(Dvd unDvd);
        int Actualizar(Dvd unDvd);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//-------------------
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Dao
{
    class ListadoDvd : ObservableCollection<Dvd>, IEditableObject
    {
        public void BeginEdit()
        {
            throw new NotImplementedException();
        }

        public void CancelEdit()
        {
            throw new NotImplementedException();
        }

        public delegate void ItemEndEditEventHandler(IEditableObject sender);

        public event ItemEndEditEventHandler ItemEdit;


        public void EndEdit()
        {
            if (ItemEdit != null)
            {
                
            }
        }
    }
}

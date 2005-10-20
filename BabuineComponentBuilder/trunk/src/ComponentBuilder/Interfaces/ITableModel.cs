using System;
using ComponentModel.Interfaces;
using System.Collections;
using Gtk;

namespace ComponentBuilder.Interfaces {
    internal interface ITableModel {
        void Clear ();
        void Add (IDataTransferObject dto);
        ListStore ListStore {get;}
    }
}

using Assets.Scripts.Core.Event;
using Assets.Scripts.Event;
using Assets.Scripts.Role;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.UI.Event
{
    class Door : MonoBehaviour
    {
        public Tile tile1;
        public Tile tile2;
        int _state;
        EventMachinery _evtMachinery;

        private void Start()
        {
            //tile1.onTread += OnTileTread;
            //tile2.onTread += OnTileTread;
        }

        private void OnDestroy()
        {
            //tile1.onTread -= OnTileTread;
            //tile2.onTread -= OnTileTread;
            _evtMachinery.Recycle();
        }

        public void RecvOpenEvent(SosObject sender, SosEventArgs args)
        {

        }

        private void Open()
        {
            
        }
    }
}

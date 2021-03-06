﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Heroes.Game
{
    public abstract class GameObject
    {
        public GameObject(String Name, String Description, String FileName)
        {
            _Name = Name;
            _FileName = FileName;
            _Description = Description;
        }
        public String FileName
        {
            get
            {
                return _FileName;
            }
        }
        public String Name
        {
            get
            {
                return _Name;
            }
        }
        public String Descritpion
        {
            get
            {
                return _Description;
            }
        }

        protected String _Name;
        protected String _Description;
        protected String _FileName;

    }
}

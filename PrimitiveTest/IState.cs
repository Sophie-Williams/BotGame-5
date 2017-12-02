﻿using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace PrimitiveTest
{
    public interface IState
    {
        void Enter(Bot bot);

        void Update(Bot bot);

        void Exit(Bot bot);

        string ToString();
    }
}

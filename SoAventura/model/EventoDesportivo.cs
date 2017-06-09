using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoAventura.model
{
    abstract class EventoDesportivo
    {
        int 
            Id_Evento,
            ano, 
            idade_min, 
            idade_max, 
            min_participantes, 
            max_participantes;
        DateTime 
            data_da_realização, 
            data_limite_pagamento, 
            fim_data_subscrição, 
            inicio_data_subscrição;
        String 
            estado,
            descrição;
        SqlMoney 
            preço_por_participante;
        bool 
            Processed = false;
        public EventoDesportivo()
        {

        }
    }
    class canoagem : EventoDesportivo {

    }
}

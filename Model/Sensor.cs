﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartPoleAPI.Model
{
    public class SensorArray
    {
        public List<Sensor> Vazao{ get; set; }
        public List<Sensor> Luminosidade { get; set; }
        public List<Sensor> Energia { get; set; }
        public List<Sensor> Temperatura { get; set; }

        public string Collection { get; set; }
        public SensorArray()
        {
            Vazao = new List<Sensor>();
            Luminosidade = new List<Sensor>();
            Energia = new List<Sensor>();
            Temperatura = new List<Sensor>();
        }
    }
    public class Sensor
    {
        public string Nome { get; set; }
        public string Valor { get; set; }
        public DateTime Data { get; set; }
    }
}

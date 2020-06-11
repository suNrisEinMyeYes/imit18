using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace imit18
{
    class Agent
    {
        float t0 = 0, La , Tau;
        Random rnd = new Random();
        public Agent(float La)
        {
            this.La = La;
        }
        public float GetNextEvent()
        {
            var ti = (float)(-Math.Log(rnd.NextDouble())/La);
            Tau = ti - t0;
            t0 = ti;
            return Tau;

        }

        public void ProccessEvent()
        {

        }
    }
}

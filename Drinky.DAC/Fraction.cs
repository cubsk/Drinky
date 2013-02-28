using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Drinky.DAC
{
    public class Fraction
    {
        protected double AccuracyFactor = 0.00001;
        protected int MaxInterations = 10;

        public int Integeral { get; protected set; }
        public int Denominator { get; protected set; }
        public int Numerator { get; protected set; }
        public double Value {get; protected set;}

        public Fraction(double value)
        {
            Value = value;
            ConvertValueToFraction();
        }

        public Fraction(int integeral, int numerator, int denominator)
        {
            Integeral = integeral;
            Denominator = denominator;
            Numerator = numerator;
            if (Numerator > Denominator)
            {
                Integeral += Numerator / Denominator;
                Numerator = Numerator - (Numerator / Denominator) * Denominator; // note, this does not reduce due to C# integer math rules
            }
            Value = integeral + ((double)numerator / ((double)denominator));

            if (Numerator == 0)
                Denominator = 1;
        }

        /// <summary>
        /// See http://homepage.smc.edu/kennedy_john/DEC2FRAC.PDF for algorithim
        /// </summary>
        protected void ConvertValueToFraction()
        {
            if (Value > 1)
                Integeral = (int)Value;

            // already whole number
            if (Value - (int)Value == 0)
            {
                Integeral = (int)Value;
                Numerator = 0; 
                Denominator = 1;
                return;
            }

            // rounds off to zero
            if (Value < Math.Pow(10, -19))
            {
                Integeral = 0;
                Numerator = 0;
                Denominator = 1;
            }


            double Z = Value - (double)Integeral;
            double originalValue = Z;
            int previousDenominator = 0;
            Denominator = 1;
            int i = 0;
            double delta = 0;

            do
            {
                Z = 1.0 / (Z - (int)Z);
                int temp = Denominator;
                Denominator = Denominator * (int)Z + previousDenominator;
                previousDenominator = temp;
                Numerator = (int)Math.Round(originalValue * (double)Denominator);

                delta = Math.Abs(originalValue - ((double)Numerator / (double)Denominator));

                
            } while (i++ < MaxInterations && delta > AccuracyFactor);


        }

        public override string ToString()
        {
            return string.Format("[{3}]: {0} {1}/{2} ", Integeral, Numerator, Denominator, Value);
        }

        public string Describe()
        {
            if (Integeral > 0)
            {
                if (Numerator == 0)
                    return Integeral.ToString();
                else
                    return string.Format("{0} {1}/{2}", Integeral, Numerator, Denominator);
            }
            else
            {
                if (Numerator == 0)
                    return "0";
                else
                    return string.Format("{0}/{1}", Numerator, Denominator);

            }

        }
    }
}

using System;

namespace GeneticAlgorithm
{
    class DNA
    {
        public int DNALength { get; set; }
        public byte[] Genes { get; set; }
        public double Fitness { get; set; }
        public double FitnessPercent { get; set; }
        public bool isLikeTarget { get; set; }


        public DNA(int length)
        {
            DNALength = length;
            Genes = new byte[DNALength];
            isLikeTarget = false;
            GenerateGens();

        }

        public DNA(byte[] genes)
        {
            DNALength = genes.Length;
            Genes = genes;
            isLikeTarget = false;
        }


        public static DNA GenerateModel(int size, Func<byte> randomFunction)
        {
            var genes = new byte[size];

            for (int i = 0; i < size; i++)
                genes[i] = randomFunction();

            return new DNA(genes);
        }

        public void GenerateGens()
        {
            for (int i = 0; i < DNALength; i++)
                Genes[i] = NewGen(1);
        }


        byte NewGen(byte max)
        {
            return (byte)Program.rnd.Next(max + 1);
        }

        public void CalcFitness(byte[] modelGenes)
        {
            Fitness = 0;
            for (int i = 0; i < DNALength; i++)
            {
                if (this.Genes[i] == modelGenes[i])
                    Fitness++;
            }
            FitnessPercent = Fitness / modelGenes.Length * 100;

            if (Fitness == modelGenes.Length)
                isLikeTarget = true;


            Fitness *= Fitness * Fitness * Fitness;

            //Fitness *= Fitness;
            //Fitness *= Fitness * Fitness * Fitness * Fitness * Fitness;
        }

        public DNA Crossover(DNA partner)
        {
            var crossoveredGenes = new byte[DNALength];
            for (int i = 0; i < DNALength; i++)
            {
                if (0.5 < Program.rnd.NextDouble())
                    crossoveredGenes[i] = this.Genes[i];
                else
                    crossoveredGenes[i] = partner.Genes[i];
            }

            return new DNA(crossoveredGenes);
        }

        public void Mutate(float chance)
        {
            for (int i = 0; i < DNALength; i++)
            {
                if (chance >= Program.rnd.NextDouble())
                    Genes[i] = Genes[i] == 0 ? (byte)1 : (byte)0;
            }
        }


    }
}





// Author: Wojciech Szweda
// Date: 2017 April
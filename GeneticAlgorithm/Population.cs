using System;
using System.Collections.Generic;
using System.IO;

namespace GeneticAlgorithm
{
    class Population
    {
        static Random rnd = new Random();

        private int PopulationCount { get; set; }
        private DNA Model { get; set; }
        private DNA[] PopulationArray { get; set; }
        private DNA MarkedDna { get; set; }
        private int MarkedDnaFitSize { get; set; }
        private DNA BestOne { get; set; }
        private bool ShowConsoleOutput { get; set; }
        private int TournamentSize { get; set; }
        private float MutationChance { get; set; }
        private int[] ModelPrecision { get; set; }
        private int GenerationCounter = 0;

        private int PrecisionCounter = 0;
        private List<DNA> BestDNAs = new List<DNA>();
        private bool Finish = false;

        public Population(DNA _model, int _popCount, float _mutationChance, int _tournamentSize, int[] _modelPrecision, bool consoleGUI)
        {
            Model = _model;
            TournamentSize = _tournamentSize;
            MutationChance = _mutationChance;
            PopulationCount = _popCount;
            ModelPrecision = _modelPrecision;
            PopulationArray = new DNA[PopulationCount];
            GenerateFirstPopulation(PopulationArray);
            ShowConsoleOutput = consoleGUI;
        }

        void GenerateFirstPopulation(DNA[] population)
        {
            for (int i = 0; i < PopulationCount; i++)
            {
                population[i] = new DNA(Model.DNALength);
                population[i].CalcFitness(Model.Genes);
            }
        }



        void SelectionWithTournament()
        {
            var newPopulation = new DNA[PopulationCount];
            for (int i = 0; i < PopulationCount; i++)
            {
                var parentA = tournamentSelection();
                var parentB = tournamentSelection();
                var child = parentA.Crossover(parentB);
                child.Mutate(MutationChance);
                child.CalcFitness(Model.Genes);
                newPopulation[i] = child;
            }

            PopulationArray = newPopulation;
        }



        public void Run()
        {
            while (true)
            {
                //PrintPopulation();
                SelectionWithTournament();
                BestOne = GetBestDna(PopulationArray);

                if (ShowConsoleOutput)
                    GUI(GenerationCounter);
                CheckPoints();

                //FullLog();
                if (Finish)
                {
                    Console.WriteLine("TARGET REACHED");
                    break;
                }
                GenerationCounter++;
            }
        }

        DNA tournamentSelection()
        {
            var tournament = new DNA[TournamentSize];
            for (int i = 0; i < TournamentSize; i++)
            {
                int randomId = (int)(rnd.NextDouble() * PopulationCount);
                tournament[i] = PopulationArray[randomId];
            }
            return GetBestDna(tournament);
        }

        DNA GetBestDna(DNA[] pop)
        {
            DNA best = null;
            foreach (var item in pop)
            {
                if (best == null)
                {
                    best = item;
                }
                else
                {
                    if (item.Fitness > best.Fitness)
                        best = item;
                }
            }
            return best;
        }

        void CheckPoints()
        {
            foreach (var item in PopulationArray)
            {
                for (int i = PrecisionCounter; i < ModelPrecision.Length; i++)
                {
                    if (Math.Sqrt(Math.Sqrt(item.Fitness)) == ModelPrecision[i])
                    {
                        PrecisionCounter++;
                        BestDNAs.Add(item);
                        ShortLog(item, ModelPrecision[i]);
                        break;
                    }
                }

            }
            if (PrecisionCounter == ModelPrecision.Length)
            {
                Finish = true;
            }
        }

        void ShortLog(DNA checkPointDNA, int precisePoint)
        {
            using (var sw = File.AppendText("ShortLog.txt"))
            {
                sw.WriteLine(DateTime.Now);
                Console.WriteLine($"Model size: { Model.DNALength}");
                sw.WriteLine($"Model size: { Model.DNALength}");
                Console.WriteLine($"Checkpoint: {precisePoint}");
                sw.WriteLine($"Checkpoint: {precisePoint}");
                Console.WriteLine($"Population size: {PopulationCount}");
                sw.WriteLine($"Population size: {PopulationCount}");
                Console.WriteLine($"Successfull Generation: {GenerationCounter}");
                sw.WriteLine($"Successfull Generation: {GenerationCounter}");
                var index = Array.IndexOf<DNA>(PopulationArray, checkPointDNA);
                Console.WriteLine($"N = {GenerationCounter * PopulationCount + index }");
                sw.WriteLine($"N = {GenerationCounter * PopulationCount + index}");
                sw.WriteLine();
            }
        }

        void FullLog()
        {

            using (var sw = File.AppendText("log.txt"))
            {
                sw.WriteLine($"GENERATION {PopulationCount}");
                foreach (var item in PopulationArray)
                {
                    foreach (var gen in item.Genes)
                    {
                        sw.Write(gen);
                    }
                    sw.Write($" {item.FitnessPercent}");
                    sw.WriteLine();
                }
                sw.WriteLine();
            }
        }

        void GUI(int populationCounter)
        {
            Console.WriteLine($"GENERATION #{GenerationCounter}");

            //Console.WriteLine($"BEST ONE IN GENERATION #{populationCounter}: ");
            //foreach (var item in BestOne.Genes)
            //{
            //    Console.Write(item);
            //}
            Console.Write($" - Fitness: {Math.Round(BestOne.FitnessPercent, 2)}%");
            Console.WriteLine();
        }


        void PrintPopulation()
        {
            Console.WriteLine("Target: ");
            foreach (var item in Model.Genes)
            {
                Console.Write(item);
            }

            Console.WriteLine("\n---------------------");

            foreach (var item in PopulationArray)
            {
                foreach (var gen in item.Genes)
                {
                    Console.Write(gen);
                }
                Console.WriteLine($" - Fitness: {item.Fitness}");
                Console.WriteLine();
            }
        }
    }
}


// Author: Wojciech Szweda
// Date: 2017 April
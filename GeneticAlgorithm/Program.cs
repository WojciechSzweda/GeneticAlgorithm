using System;

namespace GeneticAlgorithm
{
    class Program
    {
        public static Random rnd = new Random();

        struct ModelInfo
        {
            public int ModelSize;
            public int[] ModelPrecision;
        };

        static void Main(string[] args)
        {
            Func<byte>[] modelGenerationFunctions = new Func<byte>[]
            {
                () => rnd.NextDouble() < 0.5 ? (byte)0 : (byte)1,
                () => rnd.NextDouble() < 0.125 ? (byte)0 : (byte)1,
                () => rnd.NextDouble() < 0.03125  ? (byte)0 : (byte)1
            };


            var Models = new ModelInfo[]
            {
                new ModelInfo {ModelSize = 200, ModelPrecision = new int[] {198,200} },
                new ModelInfo {ModelSize = 800, ModelPrecision = new int[] {792,800} },
                new ModelInfo {ModelSize = 3200, ModelPrecision = new int[] {3160,3195} },
                new ModelInfo {ModelSize = 12800, ModelPrecision = new int[] {12670,12770} }
            };

            var selectedModel = Models[0];

            var model = DNA.GenerateModel(selectedModel.ModelSize, modelGenerationFunctions[0]);



            int populationSize = selectedModel.ModelSize < 1000 ? 50 : 200;
            float mutationChance = selectedModel.ModelSize < 500 ? 0.007f : 0.001f;
            bool ShowConsoleOutput = false;
            int tourneySize = populationSize / 10;

            var population = new Population(model, populationSize, mutationChance, tourneySize, selectedModel.ModelPrecision, ShowConsoleOutput);
            population.Run();
        }
    }
}

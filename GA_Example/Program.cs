using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA_Example
{
    class Program
    {
        static Selector s;
        static Breeder b;
        static Mutator m;
        static Chromosome[] parents;
        static Chromosome[] population;
        static int generation = 1;
        static void Main(string[] args)
        {
            Chromosome[] population = new Chromosome[16];
            Console.WriteLine("First Generation Random Init");
            for (int i = 0; i < population.Length; i++)
            {
                population[i] = new Chromosome();
                Console.WriteLine(population[i].ToString());
            }

            Console.WriteLine();
            Console.ReadKey();
            Console.Clear();
            while (true)
            {
                s = new Selector(population, 2);
                parents = s.select();


                Console.WriteLine("Parents with Fitness of Generation {0}", generation++);
                for (int i = 0; i < parents.Length; i++)
                {
                    Console.WriteLine(new FunctionDriver(parents[i]).getChromosome().ToString());
                    Console.WriteLine("Fitness: {0}", new FunctionDriver(parents[i]).getFittness());
                    Console.WriteLine();
                }


                b = new Breeder(parents);
                population = b.breed();
                Console.WriteLine("New Generation");
                for (int i = 0; i < population.Length; i++)
                {
                    Console.WriteLine(population[i].ToString());
                }


                m = new Mutator(population, 5);
                population = m.mutate();

                Console.WriteLine();
                Console.WriteLine("New Generation mutated");
                for (int i = 0; i < population.Length; i++)
                {
                    Console.WriteLine(population[i].ToString());
                }


                Console.ReadKey();
                Console.Clear();
            }

        }
    }

    public class FunctionDriver
    {
        private Chromosome c;

        public FunctionDriver(Chromosome c)
        {
            this.c = c;
        }
        public Chromosome getChromosome()
        {
            return c;
        }
        public double getFittness()
        {
            double i = c.getFirst() + c.getSecond() + c.getThird() + c.getForth();
            return (100 / Math.Abs(1000 - i));
        }


    }

    public class Chromosome
    {
        private static Random r = new Random();
        double[] genes;
        public Chromosome()
        {
            genes = new double[] { r.NextDouble() * 100, r.NextDouble() * 100, r.NextDouble() * 100, r.NextDouble() * 100 };
        }

        public override String ToString()
        {
            return getFirst() + " " + getSecond() + " " + getThird() + " " + getForth();
        }

        public Chromosome(double first, double second, double third, double forth)
        {
            genes = new double[] { first, second, third, forth };
        }

        public double getFirst()
        {
            return genes[0];
        }
        public double getSecond()
        {
            return genes[1];
        }
        public double getThird()
        {
            return genes[2];
        }
        public double getForth()
        {
            return genes[3];
        }

        public void setFirst(double value)
        {
            genes[0] = value;
        }
        public void setSecond(double value)
        {
            genes[1] = value;
        }
        public void setThird(double value)
        {
            genes[2] = value;
        }
        public void setForth(double value)
        {
            genes[3] = value;
        }

    }

    public class Selector
    {
        Chromosome[] population;
        int countParents;
        public Selector(Chromosome[] d, int countParents)
        {
            population = d;
            this.countParents = countParents;
        }

        public Chromosome[] select()
        {
            ArrayList parents = new ArrayList();
            FunctionDriver fittest = new FunctionDriver(population[0]);
            FunctionDriver first = new FunctionDriver(population[0]);
            double value = fittest.getFittness();
            FunctionDriver n;
            for (int i = 0; i < countParents; i++)
            {
                for (int j = 0; j < population.Length; j++)
                {
                    if ((n = new FunctionDriver(population[j])).getFittness() > value)
                    {
                        value = n.getFittness();
                        fittest = n;

                    }

                }

                List<Chromosome> temp = population.ToList();
                temp.Remove(fittest.getChromosome());
                population = temp.ToArray();


                parents.Add(fittest.getChromosome());
                first = new FunctionDriver(population[0]);
                value = first.getFittness();
            }

            return (Chromosome[])parents.ToArray(typeof(Chromosome));
        }

    }

    public class Breeder
    {
        Chromosome[] parents;

        public Breeder(Chromosome[] parents)
        {
            this.parents = parents;
        }

        public Chromosome[] breed()
        {
            List<Chromosome> newPopulation = new List<Chromosome>();

            for (int i = 0; i < parents.Length; i++)
            {
                for (int j = 0; j < parents.Length; j++)
                {
                    for (int k = 0; k < parents.Length; k++)
                    {
                        for (int l = 0; l < parents.Length; l++)
                        {
                            newPopulation.Add(new Chromosome(parents[i].getFirst(), parents[j].getSecond(), parents[k].getThird(), parents[l].getForth()));
                        }
                    }
                }
            }
            return newPopulation.ToArray();
        }
    }

    public class Mutator
    {
        private Chromosome[] newGen;
        private int probability;
        public Mutator(Chromosome[] newGen, int probability)
        {
            this.newGen = newGen;
            this.probability = probability;
        }

        public Chromosome[] mutate()
        {
            int boundaries = 50;
            if (probability == 0)
            {
                return newGen;
            }
            Random r = new Random();
            Random r2 = new Random();
            for (int i = 0; i < newGen.Length; i++)
            {
                if (r.Next(0, 100 / probability) == 1)
                {
                    switch (r2.Next(0, 4))
                    {
                        case 0: newGen[i].setFirst(newGen[i].getFirst() + randomValue(-boundaries, boundaries));
                            break;
                        case 1: newGen[i].setSecond(newGen[i].getSecond() + randomValue(-boundaries, boundaries));
                            break;
                        case 2: newGen[i].setThird(newGen[i].getThird() + randomValue(-boundaries, boundaries));
                            break;
                        case 3: newGen[i].setForth(newGen[i].getForth() + randomValue(-boundaries, boundaries));
                            break;
                    }
                }
            }
            return newGen;
        }

        private double randomValue(double minimum, double maximum)
        {
            Random random = new Random();
            return Math.Round(random.NextDouble() * (maximum - minimum) + minimum, 2);
        }

    }

}
